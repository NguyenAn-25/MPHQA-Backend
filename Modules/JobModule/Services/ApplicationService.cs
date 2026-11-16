using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.JobModule.Entities;
using DotNetSkeleton.Modules.JobModule.Exceptions;
using DotNetSkeleton.Modules.JobModule.Enums;
using DotNetSkeleton.Modules.WalletModule.Entities;
using DotNetSkeleton.Modules.WalletModule.Enums;
using DotNetSkeleton.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text;

using DotNetSkeleton.Modules.JobModule.Services.Interfaces;

namespace DotNetSkeleton.Modules.JobModule.Services;

public sealed class ApplicationService(
    AppDbContext context,
    IConnectionFeeSettingService connectionFeeSettingService) : IApplicationService
{
    public async Task<MyApplicationListResponseDto> ListMineAsync(
        ListJobsRequestDto request,
        int currentUserId,
        CancellationToken cancellationToken)
    {
        var query = context.Applications.AsNoTracking()
            .Where(application => application.UserId == currentUserId);

        if (request.Cursor is not null)
        {
            var cursor = DecodeCursor(request.Cursor);
            query = query.Where(application =>
                application.AppliedAt < cursor.AppliedAt ||
                (application.AppliedAt == cursor.AppliedAt && application.Id < cursor.Id));
        }

        var applications = await query
            .OrderByDescending(application => application.AppliedAt)
            .ThenByDescending(application => application.Id)
            .Take(request.PageSize + 1)
            .Select(application => new
            {
                application.Id, application.UserId, application.JobId,
                application.Status, application.AppliedAt, application.Note,
                application.ConnectionFeeCoin,
                Job = new JobResponseDto(
                    application.Job.Id, application.Job.SalaryValue, application.Job.SalaryUnit,
                    application.Job.TotalSalaryPayment, application.Job.TotalPlatformFee, application.Job.TotalExpense,
                    application.Job.CompletedAt, application.Job.WorkingAt, application.Job.FullAt,
                    application.Job.RejectedAt, application.Job.AcceptedAt, application.Job.IsPaid,
                    application.Job.Title, application.Job.Description, application.Job.Address,
                    application.Job.Longitude, application.Job.Latitude, application.Job.CancelledAt,
                    application.Job.CreatedAt, application.Job.RequiredNumberOfWorkers, application.Job.ExpiredAt,
                    application.Job.TimeStartAt, application.Job.TimeEndAt, application.Job.Status,
                    application.Job.RequiredDescription, application.Job.AutoReplyMessage, application.Job.PricingModel,
                    application.Job.PostFeeCoin, application.Job.CreatedBy,
                    new JobPosterResponseDto(
                        application.Job.Creator.Id, application.Job.Creator.PublicId,
                        context.Profiles.Where(profile => profile.UserId == application.Job.CreatedBy)
                            .Select(profile => profile.Fullname).FirstOrDefault() ?? string.Empty,
                        context.Profiles.Where(profile => profile.UserId == application.Job.CreatedBy)
                            .Select(profile => profile.Avatar).FirstOrDefault(),
                        application.Job.Creator.IsVerified),
                    new JobCategoryResponseDto(
                        application.Job.JobCategory.Id, application.Job.JobCategory.Name,
                        application.Job.JobCategory.Description, application.Job.JobCategory.Icon),
                    application.Job.Images.OrderBy(image => image.Id)
                        .Select(image => image.Image).FirstOrDefault(),
                    application.Job.SavedByUsers.Any(saved => saved.UserId == currentUserId),
                    application.Job.Feedbacks.Select(feedback => (decimal?)feedback.Star).Average(),
                    application.Job.Feedbacks.Count())
            })
            .ToListAsync(cancellationToken);

        var hasNext = applications.Count > request.PageSize;
        if (hasNext)
            applications.RemoveAt(applications.Count - 1);

        var items = applications.Select(application => new MyApplicationResponseDto(
            application.Id, application.UserId, application.JobId, application.Status,
            application.AppliedAt, application.Note, application.ConnectionFeeCoin, application.Job))
            .ToList();
        var nextCursor = hasNext && applications.Count > 0
            ? EncodeCursor(applications[^1].AppliedAt, applications[^1].Id)
            : null;

        return new MyApplicationListResponseDto(items, nextCursor, hasNext);
    }
    private static string EncodeCursor(DateTimeOffset appliedAt, int id) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes($"{appliedAt.UtcTicks}:{id}"));

    private static (DateTimeOffset AppliedAt, int Id) DecodeCursor(string value)
    {
        var buffer = new byte[value.Length];
        if (!Convert.TryFromBase64String(value, buffer, out var bytesWritten))
            throw new InvalidCursorException();

        var parts = Encoding.UTF8.GetString(buffer, 0, bytesWritten).Split(':');
        if (parts.Length != 2 || !long.TryParse(parts[0], out var ticks) ||
            !int.TryParse(parts[1], out var id) || id <= 0)
            throw new InvalidCursorException();

        try
        {
            return (new DateTimeOffset(ticks, TimeSpan.Zero), id);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new InvalidCursorException();
        }
    }

    public async Task<ApplicationAcceptanceResponseDto> AcceptPerConnectionAsync(
        int jobId,
        int currentUserId,
        AcceptApplicationsRequestDto request,
        CancellationToken cancellationToken)
    {
        if (request.ApplicationIds is null || request.ApplicationIds.Count == 0 ||
            request.ApplicationIds.Distinct().Count() != request.ApplicationIds.Count)
        {
            throw new ApplicationAcceptanceException(
                StatusCodes.Status400BadRequest,
                "Danh sách application cần nhận phải có ít nhất một ID và không được trùng.",
                "invalid_application_ids");
        }

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var job = await context.Jobs
            .SingleOrDefaultAsync(
                candidate => candidate.Id == jobId &&
                    candidate.CreatedBy == currentUserId &&
                    candidate.CancelledAt == null,
                cancellationToken);

        if (job is null)
            throw new JobNotFoundByIdException(jobId);

        if (job.PricingModel != PricingModel.PerConnection)
        {
            throw new ApplicationAcceptanceException(
                StatusCodes.Status409Conflict,
                "Chỉ công việc tính phí theo lượt kết nối mới hỗ trợ nhận ứng viên qua endpoint này.",
                "job_pricing_model_not_supported");
        }

        if (job.Status is not (JobStatus.Accepted or JobStatus.Working))
            throw new JobNotAcceptingApplicationsException(jobId);

        var applications = await context.Applications
            .Where(application => application.JobId == jobId &&
                request.ApplicationIds.Contains(application.Id))
            .OrderBy(application => application.Id)
            .ToListAsync(cancellationToken);

        if (applications.Count != request.ApplicationIds.Count ||
            applications.Any(application => application.Status != ApplicationStatus.Pending))
        {
            throw new ApplicationAcceptanceException(
                StatusCodes.Status409Conflict,
                "Một hoặc nhiều application không tồn tại hoặc không còn ở trạng thái chờ duyệt.",
                "applications_not_pending");
        }

        var acceptedCount = await context.Applications
            .CountAsync(application => application.JobId == jobId &&
                application.Status == ApplicationStatus.Accepted, cancellationToken);
        var remainingSlots = job.RequiredNumberOfWorkers - acceptedCount;

        if (remainingSlots <= 0 || applications.Count > remainingSlots)
        {
            throw new JobApplicationSlotsFullException(jobId);
        }

        var currentConnectionFee = await connectionFeeSettingService
            .GetCurrentAsync(cancellationToken);
        var connectionFeeCoin = currentConnectionFee.FeeCoin;

        foreach (var application in applications)
        {
            application.ConnectionFeeCoin = connectionFeeCoin;
        }

        var totalAmount = applications.Sum(application => application.ConnectionFeeCoin!.Value);
        var walletUpdated = await context.Wallets
            .Where(wallet => wallet.UserId == currentUserId && wallet.Balance >= totalAmount)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(wallet => wallet.Balance,
                    wallet => wallet.Balance - totalAmount),
                cancellationToken);

        if (walletUpdated == 0)
        {
            throw new ApplicationAcceptanceException(
                StatusCodes.Status409Conflict,
                "Số dư xu không đủ để nhận các ứng viên đã chọn.",
                "insufficient_wallet_balance");
        }

        var wallet = await context.Wallets
            .SingleAsync(candidate => candidate.UserId == currentUserId, cancellationToken);
        var balanceAfterEach = wallet.Balance + totalAmount;
        var payments = new List<ApplicationPaymentDetailResponseDto>(applications.Count);
        var createdTransactions = new Dictionary<int, WalletTransactionEntity>();

        foreach (var application in applications)
        {
            var fee = application.ConnectionFeeCoin!.Value;
            application.Status = ApplicationStatus.Accepted;
            balanceAfterEach -= fee;

            var walletTransaction = new WalletTransactionEntity
            {
                CoinAmount = -fee,
                BalanceAfter = balanceAfterEach,
                ObjectType = WalletObjectType.Application,
                ObjectId = application.Id,
                Type = WalletTransactionType.ConnectionFee,
                WalletId = currentUserId,
                CreatedAt = DateTimeOffset.UtcNow
            };
            context.WalletTransactions.Add(walletTransaction);
            createdTransactions.Add(application.Id, walletTransaction);
            payments.Add(new ApplicationPaymentDetailResponseDto(
                application.Id,
                application.UserId,
                0,
                fee,
                balanceAfterEach,
                "CONNECTION_FEE"));
        }

        if (job.Status == JobStatus.Accepted)
        {
            job.Status = JobStatus.Working;
            job.WorkingAt = DateTimeOffset.UtcNow;
        }

        var rejectedApplications = await context.Applications
            .Where(application => application.JobId == jobId &&
                application.Status == ApplicationStatus.Pending &&
                !request.ApplicationIds.Contains(application.Id))
            .ToListAsync(cancellationToken);

        if (acceptedCount + applications.Count >= job.RequiredNumberOfWorkers)
        {
            foreach (var application in rejectedApplications)
                application.Status = ApplicationStatus.Rejected;
        }

        await context.SaveChangesAsync(cancellationToken);
        for (var index = 0; index < payments.Count; index++)
        {
            var payment = payments[index];
            payments[index] = payment with
            {
                TransactionId = createdTransactions[payment.ApplicationId].Id
            };
        }

        await transaction.CommitAsync(cancellationToken);

        return new ApplicationAcceptanceResponseDto(
            jobId,
            payments,
            totalAmount,
            wallet.Balance);
    }

    public async Task<IReadOnlyList<JobApplicantResponseDto>> ListApplicantsAsync(
        int jobId,
        int currentUserId,
        CancellationToken cancellationToken)
    {
        var isOwner = await context.Jobs
            .AnyAsync(job => job.Id == jobId && job.CreatedBy == currentUserId && job.CancelledAt == null,
                cancellationToken);

        if (!isOwner)
            throw new JobNotFoundByIdException(jobId);

        var applications = await context.Applications
            .Where(application => application.JobId == jobId)
            .OrderByDescending(application => application.AppliedAt)
            .ThenByDescending(application => application.Id)
            .Select(application => new JobApplicantResponseDto(
                application.Id,
                application.UserId,
                application.JobId,
                application.Status,
                application.AppliedAt,
                application.Note,
                application.ConnectionFeeCoin,
                application.User.Phone,
                null))
            .ToListAsync(cancellationToken);

        if (applications.Count == 0)
            return applications;

        var applicantIds = applications.Select(application => application.UserId).Distinct().ToArray();
        var profiles = await context.Profiles
            .Where(profile => applicantIds.Contains(profile.UserId))
            .ToDictionaryAsync(profile => profile.UserId, cancellationToken);

        return applications
            .Select(application => application with
            {
                Profile = profiles.TryGetValue(application.UserId, out var profile)
                    ? new ApplicantProfileResponseDto(
                        profile.Fullname,
                        profile.Major,
                        profile.Gender,
                        profile.Avatar,
                        profile.Birthdate,
                        profile.Address,
                        profile.Note)
                    : null
            })
            .ToList();
    }

    public async Task<ApplicationResponseDto> CreateAsync(
        int jobId,
        int currentUserId,
        CreateApplicationRequestDto request,
        CancellationToken cancellationToken)
    {
        var job = await context.Jobs
            .Where(job => job.Id == jobId && job.CancelledAt == null)
            .Select(job => new
            {
                job.Id,
                job.PricingModel,
                job.Status,
                job.RequiredNumberOfWorkers
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (job is null)
            throw new JobNotFoundByIdException(jobId);

        if (job.Status is not (JobStatus.Accepted or JobStatus.Working))
            throw new JobNotAcceptingApplicationsException(jobId);

        var alreadyApplied = await context.Applications
            .AnyAsync(
                application => application.JobId == jobId &&
                    application.UserId == currentUserId,
                cancellationToken);

        if (alreadyApplied)
            throw new JobAlreadyAppliedException(jobId);

        var acceptedApplicationCount = await context.Applications
            .CountAsync(
                application => application.JobId == jobId &&
                    application.Status == ApplicationStatus.Accepted,
                cancellationToken);

        if (acceptedApplicationCount >= job.RequiredNumberOfWorkers)
            throw new JobApplicationSlotsFullException(jobId);

        int? connectionFeeCoin = null;
        var status = ApplicationStatus.Pending;

        if (job.PricingModel == PricingModel.TimeBased)
        {
            var now = DateTimeOffset.UtcNow;
            connectionFeeCoin = await context.ConnectionFeeSettings
                .Where(setting => setting.EffectiveFrom <= now)
                .OrderByDescending(setting => setting.EffectiveFrom)
                .ThenByDescending(setting => setting.Id)
                .Select(setting => (int?)setting.FeeCoin)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new ConnectionFeeNotFoundException();

            status = ApplicationStatus.Accepted;
        }

        var application = new ApplicationEntity
        {
            JobId = jobId,
            UserId = currentUserId,
            Note = string.IsNullOrWhiteSpace(request.Note) ? null : request.Note.Trim(),
            ConnectionFeeCoin = connectionFeeCoin,
            Status = status
        };

        if (job.Status == JobStatus.Accepted &&
            status == ApplicationStatus.Accepted &&
            acceptedApplicationCount == 0)
        {
            var jobToUpdate = await context.Jobs
                .FirstAsync(candidate => candidate.Id == jobId, cancellationToken);

            jobToUpdate.Status = JobStatus.Working;
            jobToUpdate.WorkingAt = DateTimeOffset.UtcNow;
        }

        context.Applications.Add(application);
        await context.SaveChangesAsync(cancellationToken);

        return new ApplicationResponseDto(
            application.Id,
            application.UserId,
            application.JobId,
            application.Status,
            application.AppliedAt,
            application.Note,
            application.ConnectionFeeCoin);
    }
}
