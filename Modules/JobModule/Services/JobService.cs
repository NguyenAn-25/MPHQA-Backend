using System.Text;
using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.JobModule.Entities;
using DotNetSkeleton.Modules.JobModule.Exceptions;
using DotNetSkeleton.Modules.JobModule.Enums;
using DotNetSkeleton.Modules.WalletModule.Entities;
using DotNetSkeleton.Modules.WalletModule.Enums;
using DotNetSkeleton.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.JobModule.Services;

public sealed class JobService(AppDbContext context) : Interfaces.IJobService
{
    public async Task CancelAsync(
        int jobId,
        int currentUserId,
        CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var job = await context.Jobs
            .SingleOrDefaultAsync(
                candidate => candidate.Id == jobId && candidate.CreatedBy == currentUserId,
                cancellationToken);

        if (job is null)
            throw new JobNotFoundByIdException(jobId);

        if (job.Status != JobStatus.Working)
        {
            throw new JobUpdateException(
                StatusCodes.Status409Conflict,
                "Chỉ có thể xác nhận công việc không diễn ra khi job đang ở trạng thái WORKING.",
                "job_not_working");
        }

        var applications = await context.Applications
            .Where(application => application.JobId == jobId)
            .ToListAsync(cancellationToken);

        var now = DateTimeOffset.UtcNow;
        job.Status = JobStatus.Cancelled;
        job.CancelledAt = now;

        foreach (var application in applications)
            application.Status = ApplicationStatus.Cancelled;

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task CompleteAsync(
        int jobId,
        CompleteJobRequestDto request,
        int currentUserId,
        CancellationToken cancellationToken)
    {
        if (request.FinishedApplicationIds is null ||
            request.FinishedApplicationIds.Distinct().Count() != request.FinishedApplicationIds.Count)
        {
            throw new JobUpdateException(
                StatusCodes.Status400BadRequest,
                "Danh sách application đã làm không được chứa ID trùng.",
                "invalid_application_ids");
        }

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var job = await context.Jobs
            .SingleOrDefaultAsync(
                candidate => candidate.Id == jobId && candidate.CreatedBy == currentUserId,
                cancellationToken);

        if (job is null)
            throw new JobNotFoundByIdException(jobId);

        if (job.Status != JobStatus.Working)
        {
            throw new JobUpdateException(
                StatusCodes.Status409Conflict,
                "Chỉ có thể hoàn thành công việc khi job đang ở trạng thái WORKING.",
                "job_not_working");
        }

        var applications = await context.Applications
            .Where(application => application.JobId == jobId)
            .ToListAsync(cancellationToken);

        var finishedApplicationIds = request.FinishedApplicationIds.ToHashSet();
        var selectedApplications = applications
            .Where(application => finishedApplicationIds.Contains(application.Id))
            .ToList();

        if (selectedApplications.Count != finishedApplicationIds.Count ||
            selectedApplications.Any(application => application.Status != ApplicationStatus.Accepted))
        {
            throw new JobUpdateException(
                StatusCodes.Status409Conflict,
                "Một hoặc nhiều application đã chọn không thuộc job này hoặc không ở trạng thái ACCEPTED.",
                "applications_not_accepted");
        }

        foreach (var application in applications)
        {
            application.Status = finishedApplicationIds.Contains(application.Id)
                ? ApplicationStatus.Finished
                : ApplicationStatus.NoShow;
        }

        var now = DateTimeOffset.UtcNow;
        job.Status = JobStatus.Completed;
        job.CompletedAt = now;

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task<JobDetailResponseDto> CreatePerConnectionAsync(
        CreatePerConnectionJobRequestDto request,
        int currentUserId,
        CancellationToken cancellationToken)
    {
        var user = await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(candidate => candidate.Id == currentUserId, cancellationToken);

        if (user is null || !user.IsVerified)
            throw new JobCreationException(
                StatusCodes.Status403Forbidden,
                "Tài khoản phải được xác thực trước khi đăng công việc.",
                "user_not_verified");

        var categoryExists = await context.JobCategories
            .AnyAsync(
                category => category.Id == request.JobCategoryId && category.IsActive,
                cancellationToken);

        if (!categoryExists)
            throw new JobCreationException(
                StatusCodes.Status404NotFound,
                "Không tìm thấy danh mục công việc đang hoạt động.",
                "job_category_not_found");

        var createdAt = DateTimeOffset.UtcNow;
        var connectionFeeCoin = await context.ConnectionFeeSettings
            .AsNoTracking()
            .Where(setting => setting.EffectiveFrom <= createdAt)
            .OrderByDescending(setting => setting.EffectiveFrom)
            .ThenByDescending(setting => setting.Id)
            .Select(setting => (int?)setting.FeeCoin)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new JobCreationException(
                StatusCodes.Status503ServiceUnavailable,
                "Chưa có mức phí kết nối khả dụng để đăng công việc.",
                "connection_fee_not_available");

        var job = new JobEntity
        {
            SalaryValue = request.SalaryValue,
            SalaryUnit = request.SalaryUnit.Trim(),
            TotalSalaryPayment = 0,
            TotalPlatformFee = 0,
            TotalExpense = 0,
            IsPaid = false,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Address = request.Address.Trim(),
            Longitude = request.Longitude,
            Latitude = request.Latitude,
            CreatedAt = createdAt,
            RequiredNumberOfWorkers = request.RequiredNumberOfWorkers,
            ExpiredAt = null,
            TimeStartAt = request.TimeStartAt.ToUniversalTime(),
            // Tạm đặt thời gian kết thúc công việc mặc định là 2099-12-31T23:59:59+07:00
            TimeEndAt = new DateTimeOffset(
                2099,
                12,
                31,
                23,
                59,
                59,
                TimeSpan.FromHours(7)).ToUniversalTime(),
            Status = JobStatus.Pending,
            RequiredDescription = request.RequiredDescription?.Trim(),
            AutoReplyMessage = request.AutoReplyMessage?.Trim(),
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = connectionFeeCoin,
            CreatedBy = currentUserId,
            JobCategoryId = request.JobCategoryId,
            PostPackageId = null
        };

        context.Jobs.Add(job);
        foreach (var image in request.Images)
        {
            job.Images.Add(new JobImageEntity
            {
                Image = image.Trim()
            });
        }

        await context.SaveChangesAsync(cancellationToken);
        return await GetForCustomerAsync(job.Id, currentUserId, cancellationToken);
    }

    public async Task<TimeBasedPaymentDetailResponseDto> GetTimeBasedPaymentAsync(
        int jobId,
        int currentUserId,
        CancellationToken cancellationToken)
    {
        var job = await context.Jobs
            .AsNoTracking()
            .Where(candidate =>
                candidate.Id == jobId &&
                candidate.CreatedBy == currentUserId &&
                candidate.PricingModel == PricingModel.TimeBased)
            .Select(candidate => new
            {
                candidate.Id,
                candidate.Title,
                candidate.PricingModel,
                candidate.PostFeeCoin,
                PostPackage = candidate.PostPackage == null
                    ? null
                    : new
                    {
                        candidate.PostPackage.Id,
                        candidate.PostPackage.Name,
                        candidate.PostPackage.DurationDays
                    }
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (job is null || job.PostPackage is null)
            throw new JobNotFoundByIdException(jobId);

        var payment = await context.WalletTransactions
            .AsNoTracking()
            .Where(transaction =>
                transaction.WalletId == currentUserId &&
                transaction.ObjectType == WalletObjectType.Job &&
                transaction.ObjectId == jobId &&
                transaction.Type == WalletTransactionType.PostFee)
            .OrderByDescending(transaction => transaction.Id)
            .Select(transaction => new TimeBasedPaymentDetailResponseDto(
                job.Id,
                job.Title,
                job.PricingModel,
                job.PostPackage.Id,
                job.PostPackage.Name,
                job.PostPackage.DurationDays,
                job.PostFeeCoin,
                transaction.Id,
                "POST_FEE",
                -transaction.CoinAmount,
                transaction.BalanceAfter,
                transaction.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        if (payment is null)
            throw new JobCreationException(
                StatusCodes.Status404NotFound,
                "Không tìm thấy giao dịch thanh toán phí đăng bài cho công việc.",
                "job_payment_not_found");

        return payment;
    }

    public async Task<JobDetailResponseDto> UpdateAsync(
        int jobId,
        UpdateJobRequestDto request,
        int currentUserId,
        CancellationToken cancellationToken)
    {
        var job = await context.Jobs
            .Include(candidate => candidate.Images)
            .SingleOrDefaultAsync(
                candidate => candidate.Id == jobId && candidate.CreatedBy == currentUserId,
                cancellationToken);

        if (job is null)
            throw new JobNotFoundByIdException(jobId);

        if (job.Status != JobStatus.Pending)
            throw new JobUpdateException(
                StatusCodes.Status409Conflict,
                "Chỉ có thể cập nhật công việc đang ở trạng thái chờ duyệt.",
                "job_not_pending");

        if (request.JobCategoryId is { } jobCategoryId)
        {
            var categoryExists = await context.JobCategories
                .AnyAsync(
                    category => category.Id == jobCategoryId && category.IsActive,
                    cancellationToken);

            if (!categoryExists)
                throw new JobUpdateException(
                    StatusCodes.Status404NotFound,
                    "Không tìm thấy danh mục công việc đang hoạt động.",
                    "job_category_not_found");

            job.JobCategoryId = jobCategoryId;
        }

        if (request.SalaryUnit is not null)
            job.SalaryUnit = request.SalaryUnit.Trim();

        if (request.SalaryValue is { } salaryValue)
            job.SalaryValue = salaryValue;

        if (request.Title is not null)
            job.Title = request.Title.Trim();

        if (request.Description is not null)
            job.Description = request.Description.Trim();

        if (request.Address is not null)
            job.Address = request.Address.Trim();

        if (request.Longitude is not null)
            job.Longitude = request.Longitude;

        if (request.Latitude is not null)
            job.Latitude = request.Latitude;

        if (request.RequiredNumberOfWorkers is { } requiredNumberOfWorkers)
            job.RequiredNumberOfWorkers = requiredNumberOfWorkers;

        if (request.TimeStartAt is { } timeStartAt)
            job.TimeStartAt = timeStartAt.ToUniversalTime();

        if (request.RequiredDescription is not null)
            job.RequiredDescription = request.RequiredDescription.Trim();

        if (request.AutoReplyMessage is not null)
            job.AutoReplyMessage = request.AutoReplyMessage.Trim();

        if (request.Images is not null)
        {
            context.JobImages.RemoveRange(job.Images);
            foreach (var image in request.Images)
            {
                job.Images.Add(new JobImageEntity
                {
                    Image = image.Trim(),
                    JobId = job.Id
                });
            }
        }

        await context.SaveChangesAsync(cancellationToken);
        return await GetForCustomerAsync(job.Id, currentUserId, cancellationToken);
    }

    public async Task<TimeBasedJobCreationResponseDto> CreateTimeBasedAsync(
        CreateTimeBasedJobRequestDto request,
        int currentUserId,
        CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var user = await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(
                candidate => candidate.Id == currentUserId,
                cancellationToken);

        if (user is null || !user.IsVerified)
            throw new JobCreationException(
                StatusCodes.Status403Forbidden,
                "Tài khoản phải được xác thực trước khi đăng công việc.",
                "user_not_verified");

        var categoryExists = await context.JobCategories
            .AnyAsync(
                category => category.Id == request.JobCategoryId && category.IsActive,
                cancellationToken);

        if (!categoryExists)
            throw new JobCreationException(
                StatusCodes.Status404NotFound,
                "Không tìm thấy danh mục công việc đang hoạt động.",
                "job_category_not_found");

        var postPackage = await context.PostPackages
            .AsNoTracking()
            .SingleOrDefaultAsync(
                package => package.Id == request.PostPackageId,
                cancellationToken)
            ?? throw new JobCreationException(
                StatusCodes.Status404NotFound,
                "Không tìm thấy gói đăng tin.",
                "post_package_not_found");

        var createdAt = DateTimeOffset.UtcNow;
        var job = new JobEntity
        {
            SalaryValue = request.SalaryValue,
            SalaryUnit = request.SalaryUnit.Trim(),
            // Tạm đặt các giá trị tổng lương, chi phí hoa hồng là 0 (Đã đổi nghiệp vụ sang thu tiền qua phí kết nối/ phí đăng bài)
            TotalSalaryPayment = 0,
            TotalPlatformFee = 0,
            TotalExpense = 0,
            IsPaid = true,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Address = request.Address.Trim(),
            Longitude = request.Longitude,
            Latitude = request.Latitude,
            CreatedAt = createdAt,
            RequiredNumberOfWorkers = request.RequiredNumberOfWorkers,
            ExpiredAt = createdAt.AddDays(postPackage.DurationDays),
            TimeStartAt = request.TimeStartAt.ToUniversalTime(),
            // Tạm đặt thời gian kết thúc công việc mặc định là 2099-12-31T23:59:59+07:00
            TimeEndAt = new DateTimeOffset(
                2099,
                12,
                31,
                23,
                59,
                59,
                TimeSpan.FromHours(7)).ToUniversalTime(),
            Status = JobStatus.Pending,
            RequiredDescription = request.RequiredDescription?.Trim(),
            AutoReplyMessage = request.AutoReplyMessage?.Trim(),
            PricingModel = PricingModel.TimeBased,
            PostFeeCoin = postPackage.PriceCoin,
            CreatedBy = currentUserId,
            JobCategoryId = request.JobCategoryId,
            PostPackageId = postPackage.Id
        };

        context.Jobs.Add(job);
        foreach (var image in request.Images)
        {
            job.Images.Add(new JobImageEntity
            {
                Image = image.Trim()
            });
        }

        await context.SaveChangesAsync(cancellationToken);

        var walletUpdated = await context.Wallets
            .Where(wallet =>
                wallet.UserId == currentUserId &&
                wallet.Balance >= postPackage.PriceCoin)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    wallet => wallet.Balance,
                    wallet => wallet.Balance - postPackage.PriceCoin),
                cancellationToken);

        if (walletUpdated == 0)
            throw new JobCreationException(
                StatusCodes.Status409Conflict,
                "Số dư xu không đủ để thanh toán phí đăng tin.",
                "insufficient_wallet_balance");

        var wallet = await context.Wallets
            .SingleAsync(wallet => wallet.UserId == currentUserId, cancellationToken);

        var walletTransaction = new WalletTransactionEntity
        {
            CoinAmount = -postPackage.PriceCoin,
            BalanceAfter = wallet.Balance,
            ObjectType = WalletObjectType.Job,
            ObjectId = job.Id,
            Type = WalletTransactionType.PostFee,
            WalletId = currentUserId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        context.WalletTransactions.Add(walletTransaction);
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        var jobDetails = await GetForCustomerAsync(job.Id, currentUserId, cancellationToken);
        return new TimeBasedJobCreationResponseDto(
            jobDetails,
            new JobPaymentResponseDto(
                walletTransaction.Id,
                postPackage.PriceCoin,
                wallet.Balance,
                "POST_FEE"));
    }

    public async Task UnsaveForCustomerAsync(
        int id,
        int currentUserId,
        CancellationToken cancellationToken)
    {
        var savedJob = await context.SavedJobs
            .SingleOrDefaultAsync(
                savedJob => savedJob.JobId == id && savedJob.UserId == currentUserId,
                cancellationToken);

        if (savedJob is null)
            throw new JobNotSavedException(id);

        context.SavedJobs.Remove(savedJob);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveForCustomerAsync(
        int id,
        int currentUserId,
        CancellationToken cancellationToken)
    {
        var jobExists = await context.Jobs
            .AnyAsync(
                job => job.Id == id &&
                    job.CancelledAt == null &&
                    job.Status != JobStatus.Rejected,
                cancellationToken);

        if (!jobExists)
            throw new JobNotFoundByIdException(id);

        var alreadySaved = await context.SavedJobs
            .AnyAsync(
                savedJob => savedJob.JobId == id && savedJob.UserId == currentUserId,
                cancellationToken);

        if (alreadySaved)
            throw new JobAlreadySavedException(id);

        context.SavedJobs.Add(new SavedJobEntity
        {
            JobId = id,
            UserId = currentUserId
        });

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<JobDetailResponseDto> GetForCustomerAsync(
        int id,
        int currentUserId,
        CancellationToken cancellationToken)
    {
        var job = await context.Jobs
            .AsNoTracking()
            .Where(job => job.Id == id && job.CancelledAt == null && job.Status != JobStatus.Rejected)
            .Select(job => new JobDetailResponseDto(
                job.Id, job.SalaryValue, job.SalaryUnit,
                job.TotalSalaryPayment, job.TotalPlatformFee, job.TotalExpense,
                job.CompletedAt, job.WorkingAt, job.FullAt,
                job.RejectedAt, job.AcceptedAt, job.IsPaid,
                job.Title, job.Description, job.Address,
                job.Longitude, job.Latitude, job.CancelledAt,
                job.CreatedAt, job.RequiredNumberOfWorkers, job.ExpiredAt,
                job.TimeStartAt, job.TimeEndAt, job.Status,
                job.RequiredDescription, job.AutoReplyMessage, job.PricingModel,
                job.PostFeeCoin, job.CreatedBy,
                new JobPosterResponseDto(
                    job.Creator.Id,
                    job.Creator.PublicId,
                    context.Profiles
                        .Where(profile => profile.UserId == job.CreatedBy)
                        .Select(profile => profile.Fullname)
                        .FirstOrDefault() ?? string.Empty,
                    context.Profiles
                        .Where(profile => profile.UserId == job.CreatedBy)
                        .Select(profile => profile.Avatar)
                        .FirstOrDefault(),
                    job.Creator.IsVerified),
                new JobCategoryResponseDto(
                    job.JobCategory.Id,
                    job.JobCategory.Name,
                    job.JobCategory.Description,
                    job.JobCategory.Icon),
                job.Images
                    .OrderBy(image => image.Id)
                    .Select(image => image.Image)
                    .ToList(),
                job.SavedByUsers.Any(savedJob => savedJob.UserId == currentUserId),
                job.Feedbacks
                    .Select(feedback => (decimal?)feedback.Star)
                    .Average(),
                job.Feedbacks.Count()))
            .SingleOrDefaultAsync(cancellationToken);

        if (job is null)
            throw new JobNotFoundByIdException(id);

        return job;
    }

    // Lấy danh sách job ở phía user theo infinite scroll cursor based
    public async Task<JobListResponseDto> ListForCustomerAsync(
        ListJobsRequestDto request,
        int currentUserId,
        CancellationToken cancellationToken)
    {
        var query = context.Jobs
            .AsNoTracking()
            .Where(job =>
                job.CancelledAt == null &&
                job.Status != JobStatus.Pending &&
                job.Status != JobStatus.Rejected);

        if (request.CategoryId is { } categoryId)
            query = query.Where(job => job.JobCategoryId == categoryId);

        if (!string.IsNullOrWhiteSpace(request.Location))
            query = query.Where(job => EF.Functions.ILike(job.Address, $"%{request.Location.Trim()}%"));

        if (request.IsVerified is { } isVerified)
            query = query.Where(job => job.Creator.IsVerified == isVerified);

        if (request.SalaryMin is { } salaryMin)
            query = query.Where(job => job.SalaryValue >= salaryMin);

        if (request.SalaryMax is { } salaryMax)
            query = query.Where(job => job.SalaryValue <= salaryMax);

        if (request.RatingMin is { } ratingMin)
            query = query.Where(job =>
                job.Feedbacks.Select(feedback => (decimal?)feedback.Star).Average() >= ratingMin);

        if (request.Cursor is not null)
        {
            var cursor = DecodeCursor(request.Cursor);
            query = query.Where(job =>
                job.CreatedAt < cursor.CreatedAt ||
                (job.CreatedAt == cursor.CreatedAt && job.Id < cursor.Id));
        }

        var jobs = await query
            .OrderByDescending(job => job.CreatedAt)
            .ThenByDescending(job => job.Id)
            .Take(request.PageSize + 1)
            .Select(job => new
            {
                job.Id,
                job.SalaryValue,
                job.SalaryUnit,
                job.TotalSalaryPayment,
                job.TotalPlatformFee,
                job.TotalExpense,
                job.CompletedAt,
                job.WorkingAt,
                job.FullAt,
                job.RejectedAt,
                job.AcceptedAt,
                job.IsPaid,
                job.Title,
                job.Description,
                job.Address,
                job.Longitude,
                job.Latitude,
                job.CancelledAt,
                job.CreatedAt,
                job.RequiredNumberOfWorkers,
                job.ExpiredAt,
                job.TimeStartAt,
                job.TimeEndAt,
                job.Status,
                job.RequiredDescription,
                job.AutoReplyMessage,
                job.PricingModel,
                job.PostFeeCoin,
                job.CreatedBy,
                Poster = new JobPosterResponseDto(
                    job.Creator.Id,
                    job.Creator.PublicId,
                    context.Profiles
                        .Where(profile => profile.UserId == job.CreatedBy)
                        .Select(profile => profile.Fullname)
                        .FirstOrDefault() ?? string.Empty,
                    context.Profiles
                        .Where(profile => profile.UserId == job.CreatedBy)
                        .Select(profile => profile.Avatar)
                        .FirstOrDefault(),
                    job.Creator.IsVerified),
                Category = new JobCategoryResponseDto(
                    job.JobCategory.Id,
                    job.JobCategory.Name,
                    job.JobCategory.Description,
                    job.JobCategory.Icon),
                FirstImage = job.Images
                    .OrderBy(image => image.Id)
                    .Select(image => image.Image)
                    .FirstOrDefault(),
                IsSaved = job.SavedByUsers.Any(savedJob => savedJob.UserId == currentUserId),
                AverageRating = job.Feedbacks
                    .Select(feedback => (decimal?)feedback.Star)
                    .Average(),
                TotalRatings = job.Feedbacks.Count()
            })
            .ToListAsync(cancellationToken);

        var hasNext = jobs.Count > request.PageSize;
        if (hasNext)
            jobs.RemoveAt(jobs.Count - 1);

        var items = jobs
            .Select(job => new JobResponseDto(
                job.Id, job.SalaryValue, job.SalaryUnit,
                job.TotalSalaryPayment, job.TotalPlatformFee, job.TotalExpense,
                job.CompletedAt, job.WorkingAt, job.FullAt,
                job.RejectedAt, job.AcceptedAt, job.IsPaid,
                job.Title, job.Description, job.Address,
                job.Longitude, job.Latitude, job.CancelledAt,
                job.CreatedAt, job.RequiredNumberOfWorkers, job.ExpiredAt,
                job.TimeStartAt, job.TimeEndAt, job.Status,
                job.RequiredDescription, job.AutoReplyMessage, job.PricingModel,
                job.PostFeeCoin, job.CreatedBy, job.Poster,
                job.Category, job.FirstImage, job.IsSaved,
                job.AverageRating, job.TotalRatings))
            .ToList();

        var nextCursor = hasNext && jobs.Count > 0
            ? EncodeCursor(jobs[^1].CreatedAt, jobs[^1].Id)
            : null;

        return new JobListResponseDto(items, nextCursor, hasNext);
    }

    public async Task<MyPostedJobListResponseDto> ListMyPostedAsync(
        ListJobsRequestDto request,
        int currentUserId,
        CancellationToken cancellationToken)
    {
        var query = context.Jobs
            .AsNoTracking()
            .Where(job => job.CreatedBy == currentUserId);

        if (request.Status is { } status)
            query = query.Where(job => job.Status == status);

        if (request.Cursor is not null)
        {
            var cursor = DecodeCursor(request.Cursor);
            query = query.Where(job =>
                job.CreatedAt < cursor.CreatedAt ||
                (job.CreatedAt == cursor.CreatedAt && job.Id < cursor.Id));
        }

        var jobs = await query
            .OrderByDescending(job => job.CreatedAt)
            .ThenByDescending(job => job.Id)
            .Take(request.PageSize + 1)
            .Select(job => new
            {
                job.Id,
                job.SalaryValue,
                job.SalaryUnit,
                job.TotalSalaryPayment,
                job.TotalPlatformFee,
                job.TotalExpense,
                job.CompletedAt,
                job.WorkingAt,
                job.FullAt,
                job.RejectedAt,
                job.AcceptedAt,
                job.IsPaid,
                job.Title,
                job.Description,
                job.Address,
                job.Longitude,
                job.Latitude,
                job.CancelledAt,
                job.CreatedAt,
                job.RequiredNumberOfWorkers,
                job.ExpiredAt,
                job.TimeStartAt,
                job.TimeEndAt,
                job.Status,
                job.RequiredDescription,
                job.AutoReplyMessage,
                job.PricingModel,
                job.PostFeeCoin,
                job.CreatedBy,
                Poster = new JobPosterResponseDto(
                    job.Creator.Id,
                    job.Creator.PublicId,
                    context.Profiles
                        .Where(profile => profile.UserId == job.CreatedBy)
                        .Select(profile => profile.Fullname)
                        .FirstOrDefault() ?? string.Empty,
                    context.Profiles
                        .Where(profile => profile.UserId == job.CreatedBy)
                        .Select(profile => profile.Avatar)
                        .FirstOrDefault(),
                    job.Creator.IsVerified),
                Category = new JobCategoryResponseDto(
                    job.JobCategory.Id,
                    job.JobCategory.Name,
                    job.JobCategory.Description,
                    job.JobCategory.Icon),
                FirstImage = job.Images
                    .OrderBy(image => image.Id)
                    .Select(image => image.Image)
                    .FirstOrDefault(),
                ApplicantCount = job.Applications.Count()
            })
            .ToListAsync(cancellationToken);

        var hasNext = jobs.Count > request.PageSize;
        if (hasNext)
            jobs.RemoveAt(jobs.Count - 1);

        var items = jobs
            .Select(job => new MyPostedJobResponseDto(
                job.Id, job.SalaryValue, job.SalaryUnit,
                job.TotalSalaryPayment, job.TotalPlatformFee, job.TotalExpense,
                job.CompletedAt, job.WorkingAt, job.FullAt,
                job.RejectedAt, job.AcceptedAt, job.IsPaid,
                job.Title, job.Description, job.Address,
                job.Longitude, job.Latitude, job.CancelledAt,
                job.CreatedAt, job.RequiredNumberOfWorkers, job.ExpiredAt,
                job.TimeStartAt, job.TimeEndAt, job.Status,
                job.RequiredDescription, job.AutoReplyMessage, job.PricingModel,
                job.PostFeeCoin, job.CreatedBy, job.Poster,
                job.Category, job.FirstImage, job.ApplicantCount))
            .ToList();

        var nextCursor = hasNext && jobs.Count > 0
            ? EncodeCursor(jobs[^1].CreatedAt, jobs[^1].Id)
            : null;

        return new MyPostedJobListResponseDto(items, nextCursor, hasNext);
    }

    public async Task<SavedJobListResponseDto> ListSavedForCustomerAsync(
        int currentUserId,
        CancellationToken cancellationToken)
    {
        var query = context.SavedJobs
            .AsNoTracking()
            .Where(savedJob =>
                savedJob.UserId == currentUserId &&
                savedJob.Job.CancelledAt == null &&
                savedJob.Job.Status != JobStatus.Rejected)
            .Select(savedJob => savedJob.Job);

        var jobs = await query
            .OrderByDescending(job => job.CreatedAt)
            .ThenByDescending(job => job.Id)
            .Select(job => new
            {
                job.Id,
                job.SalaryValue,
                job.SalaryUnit,
                job.TotalSalaryPayment,
                job.TotalPlatformFee,
                job.TotalExpense,
                job.CompletedAt,
                job.WorkingAt,
                job.FullAt,
                job.RejectedAt,
                job.AcceptedAt,
                job.IsPaid,
                job.Title,
                job.Description,
                job.Address,
                job.Longitude,
                job.Latitude,
                job.CancelledAt,
                job.CreatedAt,
                job.RequiredNumberOfWorkers,
                job.ExpiredAt,
                job.TimeStartAt,
                job.TimeEndAt,
                job.Status,
                job.RequiredDescription,
                job.AutoReplyMessage,
                job.PricingModel,
                job.PostFeeCoin,
                job.CreatedBy,
                Poster = new JobPosterResponseDto(
                    job.Creator.Id,
                    job.Creator.PublicId,
                    context.Profiles
                        .Where(profile => profile.UserId == job.CreatedBy)
                        .Select(profile => profile.Fullname)
                        .FirstOrDefault() ?? string.Empty,
                    context.Profiles
                        .Where(profile => profile.UserId == job.CreatedBy)
                        .Select(profile => profile.Avatar)
                        .FirstOrDefault(),
                    job.Creator.IsVerified),
                Category = new JobCategoryResponseDto(
                    job.JobCategory.Id,
                    job.JobCategory.Name,
                    job.JobCategory.Description,
                    job.JobCategory.Icon),
                FirstImage = job.Images
                    .OrderBy(image => image.Id)
                    .Select(image => image.Image)
                    .FirstOrDefault(),
                IsSaved = true,
                AverageRating = job.Feedbacks
                    .Select(feedback => (decimal?)feedback.Star)
                    .Average(),
                TotalRatings = job.Feedbacks.Count(),
                ApplicationStatus = job.Applications
                    .Where(application => application.UserId == currentUserId)
                    .Select(application => (ApplicationStatus?)application.Status)
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        var items = jobs
            .Select(job => new SavedJobResponseDto(
                job.Id, job.SalaryValue, job.SalaryUnit,
                job.TotalSalaryPayment, job.TotalPlatformFee, job.TotalExpense,
                job.CompletedAt, job.WorkingAt, job.FullAt,
                job.RejectedAt, job.AcceptedAt, job.IsPaid,
                job.Title, job.Description, job.Address,
                job.Longitude, job.Latitude, job.CancelledAt,
                job.CreatedAt, job.RequiredNumberOfWorkers, job.ExpiredAt,
                job.TimeStartAt, job.TimeEndAt, job.Status,
                job.RequiredDescription, job.AutoReplyMessage, job.PricingModel,
                job.PostFeeCoin, job.CreatedBy, job.Poster,
                job.Category, job.FirstImage, job.IsSaved,
                job.AverageRating, job.TotalRatings, job.ApplicationStatus))
            .ToList();

        return new SavedJobListResponseDto(items);
    }

    private static string EncodeCursor(DateTimeOffset createdAt, int id) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes($"{createdAt.UtcTicks}:{id}"));

    private static (DateTimeOffset CreatedAt, int Id) DecodeCursor(string value)
    {
        var decodedBuffer = new byte[value.Length];
        var isBase64 = Convert.TryFromBase64String(value, decodedBuffer, out var bytesWritten);

        if (!isBase64)
            throw new InvalidCursorException();

        var decoded = Encoding.UTF8.GetString(decodedBuffer, 0, bytesWritten);
        var parts = decoded.Split(':');

        if (parts.Length != 2)
            throw new InvalidCursorException();

        var isTicksValid = long.TryParse(parts[0], out var ticks);
        var isIdValid = int.TryParse(parts[1], out var id);

        if (!isTicksValid || !isIdValid || id <= 0)
            throw new InvalidCursorException();

        return (new DateTimeOffset(ticks, TimeSpan.Zero), id);
    }
}
