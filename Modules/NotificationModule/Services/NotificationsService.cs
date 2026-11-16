using DotNetSkeleton.Modules.AuthModule.Enums;
using DotNetSkeleton.Modules.NotificationModule.Enums;
using DotNetSkeleton.Modules.NotificationModule.DTOs.Requests;
using DotNetSkeleton.Modules.NotificationModule.DTOs.Responses;
using DotNetSkeleton.Modules.NotificationModule.Entities;
using DotNetSkeleton.Modules.NotificationModule.Exceptions;
using DotNetSkeleton.Modules.NotificationModule.Services.Interfaces;
using DotNetSkeleton.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.NotificationModule.Services;

public sealed class NotificationsService(AppDbContext context) : INotificationsService
{
    public async Task<PagedNotificationsResponseDto> ListAsync(
        ListNotificationsRequestDto request,
        CancellationToken cancellationToken)
    {
        var query = context.Notifications.AsNoTracking();
        var pagination = request.GetPaginationOptions();
        if (request.SendTo is { } sendTo)
            query = query.Where(notification => notification.SendTo == sendTo);

        var search = request.Search?.Trim();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(notification =>
                notification.Title.Contains(search) || notification.Content.Contains(search));

        var totalCount = await query.CountAsync(cancellationToken);
        IQueryable<NotificationEntity> notificationsQuery = query.OrderBy(notification => notification.Id);
        if (pagination.IsEnabled)
        {
            var offset = ((long)pagination.Page - 1) * pagination.PageSize;
            if (offset > int.MaxValue)
                return new PagedNotificationsResponseDto([], pagination.Page, pagination.PageSize, totalCount);

            notificationsQuery = notificationsQuery.Skip((int)offset).Take(pagination.PageSize);
        }

        var notificationRows = await notificationsQuery
            .Select(notification => new
            {
                notification.Id,
                notification.Title,
                notification.Content,
                notification.SendTo,
                notification.CreatedAt,
                CreatorId = notification.Creator.Id,
                CreatorPublicId = notification.Creator.PublicId,
                TotalRecipients = notification.Details.Count,
                ReadCount = notification.Details.Count(detail => detail.ReadAt != null)
            })
            .ToListAsync(cancellationToken);
        var creatorIds = notificationRows.Select(item => item.CreatorId).Distinct().ToArray();
        var creatorNames = await context.Profiles
            .AsNoTracking()
            .Where(profile => creatorIds.Contains(profile.UserId))
            .ToDictionaryAsync(profile => profile.UserId, profile => profile.Fullname, cancellationToken);
        var items = notificationRows
            .Select(item => new NotificationSummaryResponseDto(
                item.Id,
                item.Title,
                item.Content,
                item.SendTo,
                item.TotalRecipients,
                item.ReadCount,
                item.CreatedAt,
                new NotificationListCreatorResponseDto(
                    item.CreatorId,
                    item.CreatorPublicId,
                    creatorNames.GetValueOrDefault(item.CreatorId) ?? "Quản trị viên")))
            .ToArray();

        return new PagedNotificationsResponseDto(
            items,
            pagination.Page,
            pagination.IsEnabled ? pagination.PageSize : items.Length,
            totalCount);
    }

    public async Task<NotificationDetailResponseDto> GetByIdAsync(
        int id,
        ListNotificationRecipientsRequestDto request,
        CancellationToken cancellationToken)
    {
        var notification = await context.Notifications
            .AsNoTracking()
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken)
            ?? throw new NotificationNotFoundException();

        var pagination = request.GetPaginationOptions();
        var recipientQuery = context.NotificationDetails
            .AsNoTracking()
            .Where(detail => detail.NotificationId == id);
        var totalCount = await recipientQuery.CountAsync(cancellationToken);
        var readCount = await recipientQuery.CountAsync(detail => detail.ReadAt != null, cancellationToken);

        IQueryable<NotificationDetailEntity> recipientsQuery = recipientQuery.OrderBy(detail => detail.Id);
        var offset = ((long)pagination.Page - 1) * pagination.PageSize;
        if (pagination.IsEnabled && offset <= int.MaxValue)
            recipientsQuery = recipientsQuery.Skip((int)offset).Take(pagination.PageSize);

        List<NotificationRecipientData> recipientRows = pagination.IsEnabled && offset > int.MaxValue
            ? []
            : await recipientsQuery
                .Select(detail => new NotificationRecipientData(
                    detail.Id,
                    detail.ReadAt,
                    detail.User.Id,
                    detail.User.PublicId,
                    detail.User.Phone))
                .ToListAsync(cancellationToken);
        var recipientUserIds = recipientRows.Select(item => item.UserId).Distinct().ToArray();
        var profiles = await context.Profiles
            .AsNoTracking()
            .Where(profile => recipientUserIds.Contains(profile.UserId))
            .ToDictionaryAsync(
                profile => profile.UserId,
                profile => new { profile.Fullname, profile.Avatar },
                cancellationToken);
        var recipients = recipientRows
            .Select(item =>
            {
                profiles.TryGetValue(item.UserId, out var profile);
                return new NotificationRecipientResponseDto(
                    item.Id,
                    item.ReadAt,
                    new NotificationRecipientUserResponseDto(
                        item.UserId,
                        item.PublicId,
                        item.Phone,
                        profile?.Fullname ?? string.Empty,
                        profile?.Avatar));
            })
            .ToArray();
        var creatorName = await context.Profiles
            .AsNoTracking()
            .Where(profile => profile.UserId == notification.CreatedBy)
            .Select(profile => profile.Fullname)
            .SingleOrDefaultAsync(cancellationToken);

        return new NotificationDetailResponseDto(
            notification.Id,
            notification.Title,
            notification.Content,
            notification.SendTo,
            notification.CreatedAt,
            new NotificationDetailCreatorResponseDto(notification.CreatedBy, creatorName ?? "Quản trị viên"),
            new PagedNotificationRecipientsResponseDto(
                recipients,
                pagination.Page,
                pagination.IsEnabled ? pagination.PageSize : recipients.Length,
                totalCount,
                readCount));
    }

    public async Task<NotificationDetailResponseDto> CreateAsync(
        int creatorId,
        CreateNotificationRequestDto request,
        CancellationToken cancellationToken)
    {
        var title = request.Title.Trim();
        var content = request.Content.Trim();
        if (title.Length == 0 || content.Length == 0)
            throw new NotificationRequestInvalidException("Tiêu đề và nội dung thông báo không được để trống.");

        var sendTo = request.SendTo!.Value;
        var recipientIds = await ResolveRecipientIdsAsync(sendTo, request.UserId, cancellationToken);
        if (recipientIds.Count == 0)
            throw new NotificationRecipientNotFoundException();

        var notification = new NotificationEntity
        {
            Title = title,
            Content = content,
            SendTo = sendTo,
            CreatedBy = creatorId,
            CreatedAt = DateTimeOffset.UtcNow,
            Details = recipientIds.Select(userId => new NotificationDetailEntity
            {
                UserId = userId
            }).ToList()
        };

        context.Notifications.Add(notification);
        await context.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(
            notification.Id,
            new ListNotificationRecipientsRequestDto(),
            cancellationToken);
    }

    private async Task<List<int>> ResolveRecipientIdsAsync(
        NotificationSendTo sendTo,
        int? userId,
        CancellationToken cancellationToken)
    {
        if (sendTo == NotificationSendTo.SpecificUser)
        {
            if (userId is not > 0)
                throw new NotificationRequestInvalidException("userId là bắt buộc khi gửi thông báo cho một người.");

            var recipientId = await context.Users
                .Where(user => user.Id == userId && user.Role == UserRole.User)
                .Select(user => (int?)user.Id)
                .SingleOrDefaultAsync(cancellationToken);
            return recipientId is { } id
                ? [id]
                : throw new NotificationRecipientNotFoundException();
        }

        if (userId is not null)
            throw new NotificationRequestInvalidException("Không gửi userId khi chọn nhóm người nhận.");

        var recipients = context.Users
            .Where(user => user.Role == UserRole.User);
        if (sendTo == NotificationSendTo.VerifiedUsers)
            recipients = recipients.Where(user => user.IdentityVerifiedAt != null);
        else if (sendTo != NotificationSendTo.All)
            throw new NotificationRequestInvalidException(
                "sendTo chỉ chấp nhận ALL, VERIFIED_USERS hoặc SPECIFIC_USER.");

        return await recipients
            .OrderBy(user => user.Id)
            .Select(user => user.Id)
            .ToListAsync(cancellationToken);
    }

    private sealed record NotificationRecipientData(
        int Id,
        DateTimeOffset? ReadAt,
        int UserId,
        Guid PublicId,
        string Phone);
}
