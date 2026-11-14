using DotNetSkeleton.Modules.NotificationModule.Constants;
using DotNetSkeleton.Modules.NotificationModule.Entities;
using DotNetSkeleton.Modules.ProfileModule.Entities;
using DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;
using DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;
using DotNetSkeleton.Modules.ProfileModule.Exceptions;
using DotNetSkeleton.Modules.ProfileModule.Mappings;
using DotNetSkeleton.Modules.ProfileModule.Services.Interfaces;
using DotNetSkeleton.Shared.Extensions;
using DotNetSkeleton.Shared.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.ProfileModule.Services;

public sealed class VerificationRequestsService(AppDbContext context) : IVerificationRequestsService
{
    public async Task<PagedVerificationRequestsResponseDto> ListAsync(
        ListVerificationRequestsRequestDto request,
        CancellationToken cancellationToken)
    {
        var query = context.VerificationRequests.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Status))
            query = query.Where(item => item.Status == request.Status);
        if (request.SubmittedFrom is { } submittedFrom)
            query = query.Where(item => item.CreatedAt >= submittedFrom);
        if (request.SubmittedTo is { } submittedTo)
            query = query.Where(item => item.CreatedAt <= submittedTo);

        var totalCount = await query.CountAsync(cancellationToken);
        var offset = ((long)request.Page - 1) * request.PageSize;
        if (offset > int.MaxValue)
            return new PagedVerificationRequestsResponseDto([], request.Page, request.PageSize, totalCount);

        var items = await query
            .OrderByDescending(item => item.CreatedAt)
            .ThenByDescending(item => item.Id)
            .Skip((int)offset)
            .Take(request.PageSize)
            .ProjectToType<VerificationRequestSummaryResponseDto>(VerificationRequestMappingConfiguration.Config)
            .ToListAsync(cancellationToken);

        return new PagedVerificationRequestsResponseDto(items, request.Page, request.PageSize, totalCount);
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
        var profile = await context.Profiles
            .AsNoTracking()
            .Where(item => item.UserId == verificationRequest.SenderId)
            .Select(item => new VerificationRequestProfileComparisonResponseDto(
                item.Fullname,
                item.Birthdate,
                item.Address,
                item.Avatar,
                item.Identity == null
                    ? null
                    : new VerificationRequestIdentityComparisonResponseDto(
                        item.Identity.CardId,
                        item.Identity.FullNameOnCard,
                        item.Identity.CardFrontImage,
                        item.Identity.CardBackImage)))
            .SingleOrDefaultAsync(cancellationToken);

        return response with { Profile = profile };
    }

    public async Task<VerificationRequestDetailResponseDto> UpdateStatusAsync(
        int id,
        int reviewerId,
        UpdateVerificationRequestStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var newStatus = request.Status;
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

        var currentStatus = existing.Status.ToUpperInvariant();
        if (currentStatus == newStatus)
            return await GetByIdAsync(id, cancellationToken);
        if (currentStatus != VerificationRequestStatuses.Pending)
            throw new VerificationRequestAlreadyProcessedException();

        var now = DateTimeOffset.UtcNow;
        var isAccepted = newStatus == VerificationRequestStatuses.Accepted;
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
            SendTo = NotificationSendTo.User,
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
}
