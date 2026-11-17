using DotNetSkeleton.Modules.AuthModule.Entities;
using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.JobModule.Entities;
using DotNetSkeleton.Modules.JobModule.Enums;
using DotNetSkeleton.Modules.JobModule.Exceptions;
using DotNetSkeleton.Modules.JobModule.Services.Interfaces;
using DotNetSkeleton.Modules.NotificationModule.Entities;
using DotNetSkeleton.Modules.NotificationModule.Enums;
using DotNetSkeleton.Shared.Extensions;
using DotNetSkeleton.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.JobModule.Services;

public sealed class AdminJobsService(AppDbContext context) : IAdminJobsService
{
    public async Task<AdminPagedJobsResponseDto> ListAsync(
        AdminListJobsRequestDto request,
        CancellationToken cancellationToken)
    {
        var pagination = request.GetPaginationOptions();
        var query = context.Jobs.AsNoTracking();

        if (request.Status is { } status)
            query = query.Where(job => job.Status == status);
        if (request.PricingModel is { } pricingModel)
            query = query.Where(job => job.PricingModel == pricingModel);
        if (request.CategoryId is { } categoryId)
            query = query.Where(job => job.JobCategoryId == categoryId);
        if (request.CreatedBy is { } createdBy)
            query = query.Where(job => job.CreatedBy == createdBy);
        if (request.CreatedFrom is { } createdFrom)
            query = query.Where(job => job.CreatedAt >= createdFrom);
        if (request.CreatedTo is { } createdTo)
            query = query.Where(job => job.CreatedAt <= createdTo);

        var search = request.Search?.Trim();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(job => EF.Functions.ILike(job.Title, $"%{search}%"));

        var totalCount = await query.CountAsync(cancellationToken);
        IQueryable<JobEntity> jobsQuery = query.OrderBy(job => job.Id);
        if (pagination.IsEnabled)
        {
            var offset = ((long)pagination.Page - 1) * pagination.PageSize;
            if (offset > int.MaxValue)
                return new AdminPagedJobsResponseDto([], pagination.Page, pagination.PageSize, totalCount);

            jobsQuery = jobsQuery.Skip((int)offset).Take(pagination.PageSize);
        }

        var rows = await jobsQuery
            .Select(job => new
            {
                job.Id,
                job.Title,
                job.SalaryValue,
                job.SalaryUnit,
                job.PricingModel,
                job.Status,
                ApplicantCount = job.Applications.Count,
                job.CreatedAt,
                CategoryId = job.JobCategory.Id,
                CategoryName = job.JobCategory.Name,
                AuthorId = job.Creator.Id,
                AuthorPublicId = job.Creator.PublicId,
                AuthorPhone = job.Creator.Phone
            })
            .ToListAsync(cancellationToken);

        var authorIds = rows.Select(row => row.AuthorId).Distinct().ToArray();
        var profiles = await context.Profiles
            .AsNoTracking()
            .Where(profile => authorIds.Contains(profile.UserId))
            .ToDictionaryAsync(
                profile => profile.UserId,
                profile => new { profile.Fullname, profile.Avatar },
                cancellationToken);

        var items = rows
            .Select(row =>
            {
                profiles.TryGetValue(row.AuthorId, out var profile);
                return new AdminJobListItemResponseDto(
                    row.Id,
                    row.Title,
                    row.SalaryValue,
                    row.SalaryUnit,
                    row.PricingModel,
                    row.Status,
                    row.ApplicantCount,
                    row.CreatedAt,
                    new AdminJobCategoryResponseDto(row.CategoryId, row.CategoryName),
                    new JobAuthorResponseDto(
                        row.AuthorId,
                        row.AuthorPublicId,
                        row.AuthorPhone,
                        profile?.Fullname ?? string.Empty,
                        profile?.Avatar));
            })
            .ToArray();

        return new AdminPagedJobsResponseDto(
            items,
            pagination.Page,
            pagination.IsEnabled ? pagination.PageSize : items.Length,
            totalCount);
    }

    public async Task<AdminJobDetailResponseDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var job = await context.Jobs
            .AsNoTracking()
            .Include(item => item.JobCategory)
            .Include(item => item.PostPackage)
            .Include(item => item.Creator)
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken)
            ?? throw new JobNotFoundException();

        var authorProfile = await context.Profiles
            .AsNoTracking()
            .Where(profile => profile.UserId == job.CreatedBy)
            .Select(profile => new { profile.Fullname, profile.Avatar })
            .SingleOrDefaultAsync(cancellationToken);
        var images = await context.JobImages
            .AsNoTracking()
            .Where(image => image.JobId == id)
            .OrderBy(image => image.Id)
            .Select(image => new JobImageResponseDto(image.Id, image.Image))
            .ToListAsync(cancellationToken);
        var requiredSkills = await context.JobRequiredSkills
            .AsNoTracking()
            .Where(requiredSkill => requiredSkill.JobId == id)
            .OrderBy(requiredSkill => requiredSkill.SkillId)
            .Select(requiredSkill => new JobRequiredSkillResponseDto(
                requiredSkill.SkillId,
                requiredSkill.Skill.Name,
                requiredSkill.Skill.Icon))
            .ToListAsync(cancellationToken);

        return new AdminJobDetailResponseDto(
            job.Id,
            job.Title,
            job.Description,
            job.RequiredDescription,
            job.AutoReplyMessage,
            job.SalaryValue,
            job.SalaryUnit,
            job.TotalSalaryPayment,
            job.TotalPlatformFee,
            job.TotalExpense,
            job.Address,
            job.Longitude,
            job.Latitude,
            job.RequiredNumberOfWorkers,
            job.PricingModel,
            job.PostFeeCoin,
            job.IsPaid,
            job.Status,
            job.TimeStartAt,
            job.TimeEndAt,
            job.CreatedAt,
            job.AcceptedAt,
            job.RejectedAt,
            job.CancelledAt,
            job.ExpiredAt,
            job.RejectionReason,
            job.CancelReason,
            new AdminJobCategoryResponseDto(job.JobCategory.Id, job.JobCategory.Name),
            job.PostPackage is null
                ? null
                : new JobPostPackageResponseDto(
                    job.PostPackage.Id,
                    job.PostPackage.Name,
                    job.PostPackage.DurationDays,
                    job.PostPackage.PriceCoin),
            new JobAuthorResponseDto(
                job.Creator.Id,
                job.Creator.PublicId,
                job.Creator.Phone,
                authorProfile?.Fullname ?? string.Empty,
                authorProfile?.Avatar),
            images,
            requiredSkills);
    }

    public async Task<AdminJobModerationStatusResponseDto> UpdateModerationStatusAsync(
        int id,
        int adminId,
        AdminUpdateJobModerationStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var newStatus = request.Status!.Value;
        var job = await context.Jobs
            .AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new
            {
                item.Status,
                item.CreatedBy,
                item.AcceptedAt,
                item.RejectedAt,
                item.CancelledAt,
                item.RejectionReason,
                item.CancelReason
            })
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new JobNotFoundException();

        if (job.Status == newStatus)
            return ToStatusResponse(id, job.Status, job.AcceptedAt, job.RejectedAt, job.CancelledAt,
                job.RejectionReason, job.CancelReason);

        ValidateStatusTransition(job.Status, newStatus);

        var now = DateTimeOffset.UtcNow;
        var rejectionReason = newStatus == JobStatus.Rejected
            ? request.RejectionReason.NormalizeOptionalText()
            : null;
        var cancelReason = newStatus == JobStatus.Cancelled
            ? request.CancelReason.NormalizeOptionalText()
            : null;
        var acceptedAt = newStatus == JobStatus.Accepted ? now : job.AcceptedAt;
        var rejectedAt = newStatus == JobStatus.Rejected ? now : job.RejectedAt;
        var cancelledAt = newStatus == JobStatus.Cancelled ? now : job.CancelledAt;

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        var updatedCount = await context.Jobs
            .Where(item => item.Id == id && item.Status == job.Status)
            .ExecuteUpdateAsync(update => update
                .SetProperty(item => item.Status, newStatus)
                .SetProperty(item => item.AcceptedAt, acceptedAt)
                .SetProperty(item => item.RejectedAt, rejectedAt)
                .SetProperty(item => item.CancelledAt, cancelledAt)
                .SetProperty(item => item.RejectionReason, rejectionReason)
                .SetProperty(item => item.CancelReason, cancelReason), cancellationToken);
        if (updatedCount != 1)
            throw new JobModerationConflictException();

        var message = BuildNotification(newStatus, rejectionReason, cancelReason);
        context.Notifications.Add(new NotificationEntity
        {
            Title = message.Title,
            Content = message.Content,
            SendTo = NotificationSendTo.SpecificUser,
            CreatedAt = now,
            CreatedBy = adminId,
            Details = { new NotificationDetailEntity { UserId = job.CreatedBy } }
        });
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return ToStatusResponse(id, newStatus, acceptedAt, rejectedAt, cancelledAt, rejectionReason, cancelReason);
    }

    private static void ValidateStatusTransition(JobStatus current, JobStatus requested)
    {
        var allowed = requested switch
        {
            JobStatus.Accepted or JobStatus.Rejected => current == JobStatus.Pending,
            JobStatus.Cancelled => current is JobStatus.Pending or JobStatus.Accepted or JobStatus.Working,
            _ => false
        };

        if (!allowed)
            throw new JobModerationConflictException();
    }

    private static AdminJobModerationStatusResponseDto ToStatusResponse(
        int id,
        JobStatus status,
        DateTimeOffset? acceptedAt,
        DateTimeOffset? rejectedAt,
        DateTimeOffset? cancelledAt,
        string? rejectionReason,
        string? cancelReason) =>
        new(id, status, acceptedAt, rejectedAt, cancelledAt, rejectionReason, cancelReason);

    private static (string Title, string Content) BuildNotification(
        JobStatus status,
        string? rejectionReason,
        string? cancelReason)
    {
        if (status == JobStatus.Accepted)
            return ("Bài đăng đã được duyệt", "Bài đăng công việc của bạn đã được Admin duyệt.");

        if (status == JobStatus.Rejected)
        {
            var content = rejectionReason is null
                ? "Bài đăng công việc của bạn đã bị Admin từ chối."
                : $"Bài đăng công việc của bạn đã bị Admin từ chối. Lý do: {rejectionReason}";
            return ("Bài đăng chưa được duyệt", content);
        }

        var cancelContent = $"Bài đăng công việc của bạn đã bị Admin gỡ. Lý do: {cancelReason}";
        return ("Bài đăng đã bị gỡ", cancelContent);
    }
}
