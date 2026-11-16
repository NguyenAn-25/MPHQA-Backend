using DotNetSkeleton.Modules.NotificationModule.Entities;
using DotNetSkeleton.Modules.NotificationModule.Enums;
using DotNetSkeleton.Modules.AuthModule.Exceptions;
using DotNetSkeleton.Modules.ProfileModule.Entities;
using DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;
using DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;
using DotNetSkeleton.Modules.ProfileModule.Exceptions;
using DotNetSkeleton.Modules.ProfileModule.Enums;
using DotNetSkeleton.Modules.ProfileModule.Mappings;
using DotNetSkeleton.Modules.ProfileModule.Services.Interfaces;
using DotNetSkeleton.Shared.Extensions;
using DotNetSkeleton.Shared.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DotNetSkeleton.Modules.ProfileModule.Services;

public sealed class VerificationRequestsService(AppDbContext context) : IVerificationRequestsService
{
    public async Task<PagedVerificationRequestsResponseDto> ListAsync(
        ListVerificationRequestsRequestDto request,
        CancellationToken cancellationToken) =>
        await ListCoreAsync(request, null, cancellationToken);

    public async Task<PagedVerificationRequestsResponseDto> ListMineAsync(
        int senderId,
        ListVerificationRequestsRequestDto request,
        CancellationToken cancellationToken) =>
        await ListCoreAsync(request, senderId, cancellationToken);

    private async Task<PagedVerificationRequestsResponseDto> ListCoreAsync(
        ListVerificationRequestsRequestDto request,
        int? senderId,
        CancellationToken cancellationToken)
    {
        var query = context.VerificationRequests.AsNoTracking();
        if (senderId.HasValue)
            query = query.Where(item => item.SenderId == senderId.Value);

        var pagination = request.GetPaginationOptions();
        if (request.Status is { } status)
            query = query.Where(item => item.Status == status);
        if (request.CreatedFrom is { } createdFrom)
            query = query.Where(item => item.CreatedAt >= createdFrom);
        if (request.CreatedTo is { } createdTo)
            query = query.Where(item => item.CreatedAt <= createdTo);

        var totalCount = await query.CountAsync(cancellationToken);
        IQueryable<VerificationRequestEntity> requestsQuery = query
            .OrderBy(item => item.Id);
        if (pagination.IsEnabled)
        {
            var offset = ((long)pagination.Page - 1) * pagination.PageSize;
            if (offset > int.MaxValue)
                return new PagedVerificationRequestsResponseDto([], pagination.Page, pagination.PageSize, totalCount);

            requestsQuery = requestsQuery.Skip((int)offset).Take(pagination.PageSize);
        }

        var requests = await requestsQuery
            .Include(item => item.Sender)
            .ToListAsync(cancellationToken);
        var senderIds = requests.Select(item => item.SenderId).Distinct().ToArray();
        var profilesByUserId = await context.Profiles
            .AsNoTracking()
            .Where(profile => senderIds.Contains(profile.UserId))
            .ToDictionaryAsync(profile => profile.UserId, cancellationToken);
        var items = requests
            .Select(item =>
            {
                profilesByUserId.TryGetValue(item.SenderId, out var profile);
                return new VerificationRequestSummaryResponseDto(
                    item.Id,
                    item.FullNameOnCard,
                    item.IdNumber,
                    item.CardFrontImage,
                    item.CardBackImage,
                    item.PortraitImage,
                    item.Status,
                    item.RejectionReason,
                    item.VerifiedAt,
                    item.RejectedAt,
                    item.CreatedAt,
                    new VerificationRequestSenderSummaryResponseDto(
                        item.Sender.Id,
                        item.Sender.PublicId,
                        item.Sender.Phone,
                        profile?.Fullname ?? item.FullNameOnCard,
                        profile?.Avatar));
            })
            .ToArray();

        return new PagedVerificationRequestsResponseDto(
            items,
            pagination.Page,
            pagination.IsEnabled ? pagination.PageSize : items.Length,
            totalCount);
    }

    public async Task<VerificationRequestDetailResponseDto> CreateAsync(
        int senderId,
        CreateVerificationRequestRequestDto request,
        CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        if (await context.VerificationRequests.AnyAsync(
                item => item.SenderId == senderId && item.Status == VerificationRequestStatus.Pending,
                cancellationToken))
        {
            throw new VerificationRequestPendingException();
        }

        var senderExists = await context.Users.AnyAsync(user => user.Id == senderId, cancellationToken);
        if (!senderExists)
            throw new AuthUserNotFoundException();

        var profile = await context.Profiles.SingleOrDefaultAsync(
            item => item.UserId == senderId,
            cancellationToken);
        if (profile is null)
        {
            context.Profiles.Add(new ProfileEntity
            {
                UserId = senderId,
                Fullname = request.FullNameOnCard.Trim()
            });
        }

        var now = DateTimeOffset.UtcNow;
        var verificationRequest = new VerificationRequestEntity
        {
            FullNameOnCard = request.FullNameOnCard.Trim(),
            IdNumber = request.IdNumber.Trim(),
            CardFrontImage = request.CardFrontImage.Trim(),
            CardBackImage = request.CardBackImage.Trim(),
            PortraitImage = request.PortraitImage.Trim(),
            Status = VerificationRequestStatus.Pending,
            CreatedAt = now,
            SenderId = senderId
        };
        context.VerificationRequests.Add(verificationRequest);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (IsPendingRequestUniqueViolation(exception))
        {
            throw new VerificationRequestPendingException();
        }

        await transaction.CommitAsync(cancellationToken);
        return await GetByIdAsync(verificationRequest.Id, cancellationToken);
    }

    public async Task<VerificationRequestDetailResponseDto> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var verificationRequest = await context.VerificationRequests
            .AsNoTracking()
            .Include(item => item.Sender)
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken)
            ?? throw new VerificationRequestNotFoundException();

        var response = verificationRequest.Adapt<VerificationRequestDetailResponseDto>(
            VerificationRequestMappingConfiguration.Config);
        var senderProfile = await context.Profiles
            .AsNoTracking()
            .Where(item => item.UserId == verificationRequest.SenderId)
            .Select(item => new
            {
                Profile = new VerificationRequestSenderProfileResponseDto(
                    item.Id,
                    item.Fullname,
                    item.Gender,
                    item.Birthdate,
                    item.Avatar),
                CurrentIdentity = item.Identity == null
                    ? null
                    : new VerificationRequestIdentityResponseDto(
                        item.Identity.CardId,
                        item.Identity.FullNameOnCard,
                        item.Identity.CardFrontImage,
                        item.Identity.CardBackImage)
            })
            .SingleOrDefaultAsync(cancellationToken);

        return response with
        {
            Sender = response.Sender with
            {
                Profile = senderProfile?.Profile,
                CurrentIdentity = senderProfile?.CurrentIdentity
            }
        };
    }

    public async Task<VerificationRequestDetailResponseDto> UpdateStatusAsync(
        int id,
        int reviewerId,
        UpdateVerificationRequestStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var newStatus = request.Status!.Value;
        var existing = await context.VerificationRequests
            .AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new
            {
                item.Status,
                item.SenderId,
                item.IdNumber,
                item.FullNameOnCard,
                item.CardFrontImage,
                item.CardBackImage
            })
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new VerificationRequestNotFoundException();

        var currentStatus = existing.Status;
        if (currentStatus == newStatus)
            return await GetByIdAsync(id, cancellationToken);
        if (currentStatus != VerificationRequestStatus.Pending)
            throw new VerificationRequestAlreadyProcessedException();

        var now = DateTimeOffset.UtcNow;
        var isAccepted = newStatus == VerificationRequestStatus.Accepted;
        var rejectionReason = isAccepted ? null : request.RejectionReason.NormalizeOptionalText();

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        var updatedCount = await context.VerificationRequests
            .Where(item => item.Id == id && item.Status == existing.Status)
            .ExecuteUpdateAsync(update => update
                .SetProperty(item => item.Status, newStatus)
                .SetProperty(item => item.VerifiedAt, isAccepted ? now : (DateTimeOffset?)null)
                .SetProperty(item => item.RejectedAt, isAccepted ? (DateTimeOffset?)null : now)
                .SetProperty(item => item.RejectionReason, rejectionReason), cancellationToken);
        if (updatedCount != 1)
            throw new VerificationRequestAlreadyProcessedException();

        var sender = await context.Users.SingleOrDefaultAsync(
            item => item.Id == existing.SenderId,
            cancellationToken);
        if (sender is null)
            throw new VerificationRequestNotFoundException();

        sender.IdentityVerifiedAt = isAccepted ? now : null;
        sender.IdentityRejectedAt = isAccepted ? null : now;

        if (isAccepted)
        {
            var profile = await context.Profiles.SingleOrDefaultAsync(
                item => item.UserId == sender.Id,
                cancellationToken);
            if (profile is null)
                throw new VerificationProfileNotFoundException();

            var identity = await context.ProfileIdentities.SingleOrDefaultAsync(
                item => item.ProfileId == profile.Id,
                cancellationToken);
            if (identity is null)
            {
                identity = new ProfileIdentityEntity { ProfileId = profile.Id };
                context.ProfileIdentities.Add(identity);
            }

            identity.CardId = existing.IdNumber;
            identity.FullNameOnCard = existing.FullNameOnCard;
            identity.CardFrontImage = existing.CardFrontImage;
            identity.CardBackImage = existing.CardBackImage;
        }

        context.Notifications.Add(new NotificationEntity
        {
            Title = isAccepted ? "Yêu cầu xác thực đã được chấp nhận" : "Yêu cầu xác thực đã bị từ chối",
            Content = isAccepted
                ? "Thông tin định danh của bạn đã được xác thực thành công."
                : $"Yêu cầu xác thực của bạn bị từ chối. Lý do: {rejectionReason}",
            CreatedAt = now,
            SendTo = NotificationSendTo.SpecificUser,
            CreatedBy = reviewerId,
            Details =
            {
                new NotificationDetailEntity { UserId = sender.Id }
            }
        });

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    private static bool IsPendingRequestUniqueViolation(DbUpdateException exception) =>
        exception.InnerException is PostgresException
        {
            SqlState: PostgresErrorCodes.UniqueViolation,
            ConstraintName: "IX_verification_requests_sender_id_pending"
        };
}
