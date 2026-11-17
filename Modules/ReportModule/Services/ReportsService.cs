using DotNetSkeleton.Modules.AuthModule.Entities;
using DotNetSkeleton.Modules.AuthModule.Enums;
using DotNetSkeleton.Modules.JobModule.Entities;
using DotNetSkeleton.Modules.NotificationModule.Entities;
using DotNetSkeleton.Modules.NotificationModule.Enums;
using DotNetSkeleton.Modules.ProfileModule.Entities;
using DotNetSkeleton.Modules.ReportModule.DTOs.Requests;
using DotNetSkeleton.Modules.ReportModule.DTOs.Responses;
using DotNetSkeleton.Modules.ReportModule.Entities;
using DotNetSkeleton.Modules.ReportModule.Enums;
using DotNetSkeleton.Modules.ReportModule.Exceptions;
using DotNetSkeleton.Modules.ReportModule.Services.Interfaces;
using DotNetSkeleton.Shared.Extensions;
using DotNetSkeleton.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.ReportModule.Services;

public sealed class ReportsService(AppDbContext context) : IReportsService
{
    public async Task<PagedReportsResponseDto> ListAsync(
        ListReportsRequestDto request,
        CancellationToken cancellationToken)
    {
        var pagination = request.GetPaginationOptions();
        var query = context.Reports.AsNoTracking();
        if (request.ObjectType is { } objectType)
            query = query.Where(report => report.ObjectType == objectType);
        if (request.Status is { } status)
            query = query.Where(report => report.Status == status);
        if (request.CreatedFrom is { } createdFrom)
            query = query.Where(report => report.CreatedAt >= createdFrom);
        if (request.CreatedTo is { } createdTo)
            query = query.Where(report => report.CreatedAt <= createdTo);

        var totalCount = await query.CountAsync(cancellationToken);
        IQueryable<ReportEntity> reportsQuery = query.OrderBy(report => report.Id);
        if (pagination.IsEnabled)
        {
            var offset = ((long)pagination.Page - 1) * pagination.PageSize;
            if (offset > int.MaxValue)
                return new PagedReportsResponseDto([], pagination.Page, pagination.PageSize, totalCount);

            reportsQuery = reportsQuery.Skip((int)offset).Take(pagination.PageSize);
        }

        var rows = await reportsQuery
            .Select(report => new
            {
                report.Id,
                report.ObjectType,
                report.ObjectId,
                report.Title,
                report.Reason,
                report.Status,
                report.CreatedAt,
                report.ProcessedAt,
                SenderId = report.Sender.Id,
                SenderPublicId = report.Sender.PublicId,
                SenderPhone = report.Sender.Phone
            })
            .ToListAsync(cancellationToken);

        var senderIds = rows.Select(row => row.SenderId).Distinct().ToArray();
        var profiles = await context.Profiles
            .AsNoTracking()
            .Where(profile => senderIds.Contains(profile.UserId))
            .ToDictionaryAsync(
                profile => profile.UserId,
                profile => new { profile.Fullname, profile.Avatar },
                cancellationToken);

        var items = rows
            .Select(row =>
            {
                profiles.TryGetValue(row.SenderId, out var profile);
                return new ReportSummaryResponseDto(
                    row.Id,
                    row.ObjectType,
                    row.ObjectId,
                    row.Title,
                    row.Reason,
                    row.Status,
                    row.CreatedAt,
                    row.ProcessedAt,
                    new ReportSenderResponseDto(
                        row.SenderId,
                        row.SenderPublicId,
                        row.SenderPhone,
                        profile?.Fullname ?? string.Empty,
                        profile?.Avatar));
            })
            .ToArray();

        return new PagedReportsResponseDto(
            items,
            pagination.Page,
            pagination.IsEnabled ? pagination.PageSize : items.Length,
            totalCount);
    }

    public async Task<ReportDetailResponseDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var report = await context.Reports
            .AsNoTracking()
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken)
            ?? throw new ReportNotFoundException();

        var sender = await context.Users
            .AsNoTracking()
            .Where(user => user.Id == report.SenderId)
            .Select(user => new { user.Id, user.PublicId, user.Phone })
            .SingleAsync(cancellationToken);
        var senderProfile = await context.Profiles
            .AsNoTracking()
            .Where(profile => profile.UserId == sender.Id)
            .Select(profile => new { profile.Fullname, profile.Avatar })
            .SingleOrDefaultAsync(cancellationToken);
        var senderResponse = new ReportSenderResponseDto(
            sender.Id,
            sender.PublicId,
            sender.Phone,
            senderProfile?.Fullname ?? string.Empty,
            senderProfile?.Avatar);

        var target = await BuildTargetAsync(report.ObjectType, report.ObjectId, cancellationToken);
        return new ReportDetailResponseDto(
            report.Id,
            report.ObjectType,
            report.ObjectId,
            report.Title,
            report.Reason,
            report.Image,
            report.Status,
            report.CreatedAt,
            report.ProcessedAt,
            senderResponse,
            target);
    }

    public async Task<ReportProcessResponseDto> ProcessAsync(
        int id,
        int reviewerId,
        ProcessReportRequestDto request,
        CancellationToken cancellationToken)
    {
        var newStatus = request.Status!.Value;
        var existing = await context.Reports
            .AsNoTracking()
            .Where(report => report.Id == id)
            .Select(report => new
            {
                report.Status,
                report.SenderId,
                report.ProcessedAt,
                report.ProcessedNote
            })
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new ReportNotFoundException();

        if (existing.Status == newStatus)
        {
            return new ReportProcessResponseDto(
                id,
                existing.Status,
                existing.ProcessedAt ?? DateTimeOffset.UtcNow,
                existing.ProcessedNote);
        }

        if (existing.Status != ReportStatus.Pending)
            throw new ReportAlreadyProcessedException();

        var now = DateTimeOffset.UtcNow;
        var note = request.Note.NormalizeOptionalText();
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var updatedCount = await context.Reports
            .Where(report => report.Id == id && report.Status == ReportStatus.Pending)
            .ExecuteUpdateAsync(update => update
                .SetProperty(report => report.Status, newStatus)
                .SetProperty(report => report.ProcessedAt, now)
                .SetProperty(report => report.ProcessedNote, note), cancellationToken);
        if (updatedCount != 1)
            throw new ReportAlreadyProcessedException();

        var isResolved = newStatus == ReportStatus.Resolved;
        context.Notifications.Add(new NotificationEntity
        {
            Title = isResolved ? "Báo cáo đã được xử lý" : "Báo cáo đã bị từ chối",
            Content = note is null
                ? isResolved
                    ? "Báo cáo bạn gửi đã được Admin xử lý."
                    : "Báo cáo bạn gửi đã bị Admin từ chối."
                : isResolved
                    ? $"Báo cáo bạn gửi đã được Admin xử lý. Ghi chú: {note}"
                    : $"Báo cáo bạn gửi đã bị Admin từ chối. Ghi chú: {note}",
            SendTo = NotificationSendTo.SpecificUser,
            CreatedAt = now,
            CreatedBy = reviewerId,
            Details =
            {
                new NotificationDetailEntity { UserId = existing.SenderId }
            }
        });

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return new ReportProcessResponseDto(id, newStatus, now, note);
    }

    private async Task<ReportTargetResponseDto> BuildTargetAsync(
        ReportObjectType objectType,
        int objectId,
        CancellationToken cancellationToken)
    {
        if (objectType == ReportObjectType.Job)
        {
            var job = await context.Jobs
                .AsNoTracking()
                .Where(item => item.Id == objectId)
                .Select(item => new ReportJobTargetResponseDto(
                    item.Id,
                    item.Title,
                    item.Status,
                    item.CreatedAt))
                .SingleOrDefaultAsync(cancellationToken);

            return new ReportTargetResponseDto(objectType, job, null);
        }

        var user = await context.Users
            .AsNoTracking()
            .Where(item => item.Id == objectId)
            .Select(item => new
            {
                item.Id,
                item.PublicId,
                item.Phone,
                item.IsActive
            })
            .SingleOrDefaultAsync(cancellationToken);
        if (user is null)
            return new ReportTargetResponseDto(objectType, null, null);

        var fullname = await context.Profiles
            .AsNoTracking()
            .Where(profile => profile.UserId == user.Id)
            .Select(profile => profile.Fullname)
            .SingleOrDefaultAsync(cancellationToken);

        return new ReportTargetResponseDto(
            objectType,
            null,
            new ReportUserTargetResponseDto(
                user.Id,
                user.PublicId,
                fullname ?? string.Empty,
                user.Phone,
                user.IsActive));
    }

}
