using DotNetSkeleton.Modules.AuthModule.Entities;
using DotNetSkeleton.Modules.AuthModule.Enums;
using DotNetSkeleton.Modules.JobModule.Entities;
using DotNetSkeleton.Modules.JobModule.Enums;
using DotNetSkeleton.Shared.Persistence;
using DotNetSkeleton.Shared.Persistence.Seeders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.JobModule.Seeders;

public sealed class JobDevelopmentSampleDataSeeder(
    IWebHostEnvironment environment,
    ILogger<JobDevelopmentSampleDataSeeder> logger) : ISeeder
{
    private const int SampleJobCount = 120;
    private const string TitlePrefix = "[DEV JOB]";
    private const int SampleUserCount = 60;

    private static readonly (string Name, JobStatus Status)[] StatusTestFixtures =
    [
        ("PENDING TO ACCEPTED", JobStatus.Pending),
        ("PENDING TO REJECTED", JobStatus.Pending),
        ("PENDING TO CANCELLED", JobStatus.Pending),
        ("ACCEPTED TO CANCELLED", JobStatus.Accepted),
        ("FULL TO CANCELLED", JobStatus.Full),
        ("WORKING TO CANCELLED", JobStatus.Working),
        ("REJECTED CONFLICT", JobStatus.Rejected),
        ("COMPLETED CONFLICT", JobStatus.Completed),
        ("CANCELLED IDEMPOTENT", JobStatus.Cancelled),
        ("ACCEPTED IDEMPOTENT", JobStatus.Accepted),
        ("FULL CONFLICT", JobStatus.Full),
        ("WORKING CONFLICT", JobStatus.Working)
    ];

    private static readonly string[] JobTitles =
    [
        "Nhân viên phục vụ tiệc cưới cuối tuần",
        "Cộng tác viên phát tờ rơi ca sáng",
        "Đóng gói đơn hàng ca tối",
        "Kiểm kê hàng hóa siêu thị",
        "Phụ bếp theo ca tại nhà hàng",
        "Trực quầy thông tin sự kiện",
        "Hỗ trợ khách check-in hội thảo",
        "Giao hàng nội thành trong ngày",
        "Dọn dẹp văn phòng sau sự kiện",
        "Bán vé tại lễ hội âm nhạc",
        "Bốc xếp hàng hóa tại kho",
        "Chăm sóc thú cưng cuối tuần",
        "Hỗ trợ chụp ảnh sự kiện",
        "Nhập liệu sản phẩm bán hàng",
        "Nhân viên thu ngân thời vụ",
        "Điều phối chỗ ngồi tại hội trường",
        "Vệ sinh căn hộ theo giờ",
        "Phát mẫu thử tại siêu thị",
        "Hỗ trợ kho trong đợt cuối tháng",
        "Phục vụ quán cà phê ca sáng",
        "Trang trí gian hàng hội chợ",
        "Giữ xe tại sự kiện thể thao",
        "Trợ giảng lớp kỹ năng ngắn hạn",
        "Hỗ trợ livestream bán hàng",
        "Nhân viên lễ tân thay ca",
        "Phân loại nông sản theo mùa",
        "Chăm sóc cây xanh khu văn phòng",
        "Đóng gói quà tặng doanh nghiệp",
        "Dịch phụ đề video ngắn",
        "Chuyển âm thanh thành văn bản",
        "Hỗ trợ khách hàng qua khung chat",
        "Rửa ly và dọn bàn quán ăn",
        "Lắp ráp kệ trưng bày sản phẩm",
        "Hỗ trợ chuyển nhà trong ngày",
        "Điều phối nhân sự tại khu vực sự kiện",
        "Tiếp đón khách tại phòng khám",
        "Dán nhãn sản phẩm trong kho",
        "Thợ phụ lắp đặt sân khấu",
        "Thu ngân cửa hàng tiện lợi ca tối",
        "Trông coi gian hàng cuối tuần",
        "Phát nước giải khát tại giải chạy",
        "Kiểm tra vé vào cổng sự kiện",
        "Phụ giúp bếp ăn công nghiệp",
        "Cộng tác viên cập nhật dữ liệu",
        "Chăm sóc khách hàng qua điện thoại",
        "Lau kính khu vực văn phòng",
        "Trực tổng đài đặt lịch hẹn",
        "Hỗ trợ khảo sát khách hàng tại chỗ"
    ];

    private static readonly string[] WorkDetails =
    [
        "Thực hiện công việc theo ca đã đăng ký, phối hợp với người phụ trách tại địa điểm và bàn giao dụng cụ sau khi hoàn thành.",
        "Có hướng dẫn ban đầu tại chỗ. Ưu tiên người đúng giờ, giao tiếp lịch sự và có thể làm việc theo nhóm.",
        "Công việc ngắn hạn phù hợp làm thêm cuối tuần. Lịch làm cụ thể được xác nhận trước ngày bắt đầu.",
        "Nhân sự được chia theo nhóm nhỏ, có trưởng ca hỗ trợ và nghỉ giữa ca theo sắp xếp của đơn vị.",
        "Vui lòng chuẩn bị trang phục gọn gàng, có mặt trước giờ nhận ca và báo sớm nếu cần đổi lịch.",
        "Nhiệm vụ tập trung vào hỗ trợ vận hành trong khung giờ cao điểm, không yêu cầu cam kết dài hạn.",
        "Đơn vị cung cấp vật dụng cần thiết. Người làm cần tuân thủ hướng dẫn an toàn và quy trình tại chỗ.",
        "Thanh toán sau khi xác nhận thời lượng làm việc và hoàn thành các đầu việc đã thống nhất."
    ];

    private static readonly string[] Areas =
    [
        "Quận 1, Thành phố Hồ Chí Minh",
        "Quận Bình Thạnh, Thành phố Hồ Chí Minh",
        "Quận Gò Vấp, Thành phố Hồ Chí Minh",
        "Thành phố Thủ Đức, Thành phố Hồ Chí Minh",
        "Quận Hải Châu, Thành phố Đà Nẵng",
        "Quận Thanh Khê, Thành phố Đà Nẵng",
        "Quận Hoàn Kiếm, Thành phố Hà Nội",
        "Quận Cầu Giấy, Thành phố Hà Nội",
        "Thành phố Biên Hòa, Đồng Nai",
        "Thành phố Thuận An, Bình Dương",
        "Thành phố Nha Trang, Khánh Hòa",
        "Thành phố Cần Thơ"
    ];

    private static readonly string[] PackageNames =
    [
        "Gói đăng cơ bản 7 ngày",
        "Gói đăng tiêu chuẩn 14 ngày",
        "Gói đăng nổi bật 30 ngày",
        "Gói tuyển nhanh 3 ngày",
        "Gói tuyển số lượng lớn 30 ngày"
    ];

    private static readonly string[] SamplePhones = Enumerable.Range(0, SampleUserCount)
        .Select(index => $"+1202555{100 + index:0000}")
        .ToArray();

    public int Order => 8;

    public async Task<int> SeedAsync(AppDbContext context)
    {
        if (!environment.IsDevelopment())
            return 0;

        var users = await context.Users
            .Where(user => SamplePhones.Contains(user.Phone) && user.Role == UserRole.User && user.IsActive)
            .OrderBy(user => user.Id)
            .ToListAsync();
        var categories = await context.JobCategories
            .AsNoTracking()
            .Where(category => category.IsActive && category.ParentId != null)
            .OrderBy(category => category.Id)
            .Select(category => new CategorySample(category.Id))
            .ToListAsync();
        var skills = await context.Skills
            .AsNoTracking()
            .Where(skill => skill.IsActive)
            .OrderBy(skill => skill.Id)
            .Select(skill => new SkillSample(skill.Id))
            .ToListAsync();

        if (users.Count < 2 || categories.Count == 0)
        {
            logger.LogInformation(
                "Job sample data skipped because it requires at least two active user fixtures and one active child category.");
            return 0;
        }

        var packagesAdded = await EnsurePackagesAsync(context);
        var packages = await context.PostPackages
            .AsNoTracking()
            .OrderBy(package => package.Id)
            .ToListAsync();
        if (packages.Count == 0)
        {
            logger.LogInformation("Job sample data skipped because no post packages are available.");
            return packagesAdded;
        }

        var existingTitles = await context.Jobs
            .Where(job => job.Title.StartsWith(TitlePrefix))
            .Select(job => job.Title)
            .ToHashSetAsync();
        var jobs = new List<JobEntity>();
        var relatedRecordsAdded = packagesAdded;
        var now = DateTimeOffset.UtcNow;

        for (var index = 0; index < SampleJobCount; index++)
        {
            var isStatusTestFixture = index < StatusTestFixtures.Length;
            var title = isStatusTestFixture
                ? $"{TitlePrefix} API STATUS TEST {StatusTestFixtures[index].Name}"
                : $"{TitlePrefix} {index + 1:000} - {JobTitles[index % JobTitles.Length]}";
            if (!existingTitles.Add(title))
                continue;

            var creator = users[index % users.Count];
            var category = categories[(index * 7) % categories.Count];
            var package = packages[(index * 3) % packages.Count];
            var status = isStatusTestFixture
                ? StatusTestFixtures[index].Status
                : GetStatus(index - StatusTestFixtures.Length);
            var createdAt = GetCreatedAt(now, status, index, creator.CreatedAt);
            var requiredWorkers = 1 + index % 8;
            var pricingModel = index % 3 == 0
                ? JobPricingModel.PerConnection
                : JobPricingModel.TimeBased;
            var salaryValue = pricingModel == JobPricingModel.TimeBased
                ? 35_000m + index % 6 * 5_000m
                : 250_000m + index % 8 * 50_000m;
            var totalSalary = salaryValue * requiredWorkers * (pricingModel == JobPricingModel.TimeBased ? 6 : 1);
            var platformFee = decimal.Round(totalSalary * 0.05m, 2);
            var timeStartAt = status switch
            {
                JobStatus.Working => now.AddHours(-3),
                JobStatus.Completed => createdAt.AddDays(1 + index % 8),
                JobStatus.Pending or JobStatus.Accepted or JobStatus.Full => now.AddDays(1 + index % 30),
                _ => createdAt.AddDays(1 + index % 12).AddHours(7 + index % 10)
            };
            var timeEndAt = status == JobStatus.Working
                ? now.AddHours(5)
                : timeStartAt.AddHours(4 + index % 5);
            var acceptedAt = status is JobStatus.Accepted or JobStatus.Full or JobStatus.Working or JobStatus.Completed ||
                             status == JobStatus.Cancelled && index % 2 == 0
                ? createdAt.AddHours(1 + index % 4)
                : (DateTimeOffset?)null;
            var rejectedAt = status == JobStatus.Rejected
                ? createdAt.AddHours(2 + index % 4)
                : (DateTimeOffset?)null;
            var cancelledAt = status == JobStatus.Cancelled
                ? createdAt.AddHours(2 + index % 12)
                : (DateTimeOffset?)null;

            var job = new JobEntity
            {
                Title = title,
                Description = $"{WorkDetails[index % WorkDetails.Length]} Ca làm phù hợp với công việc ngắn hạn tại {Areas[index % Areas.Length]}.",
                RequiredDescription = index % 3 == 0
                    ? "Có thể làm đúng giờ, giao tiếp lịch sự và hoàn thành nhiệm vụ trong ca đã đăng ký."
                    : null,
                AutoReplyMessage = index % 4 == 0
                    ? "Cảm ơn bạn đã quan tâm. Đơn vị sẽ xác nhận lịch làm trong thời gian sớm nhất."
                    : null,
                SalaryValue = salaryValue,
                SalaryUnit = pricingModel == JobPricingModel.TimeBased ? "giờ" : "ca",
                TotalSalaryPayment = totalSalary,
                TotalPlatformFee = platformFee,
                TotalExpense = totalSalary + platformFee,
                Address = Areas[index % Areas.Length],
                Longitude = 106.6m + index % 30 * 0.001m,
                Latitude = 10.7m + index % 30 * 0.001m,
                RequiredNumberOfWorkers = requiredWorkers,
                PricingModel = pricingModel,
                PostFeeCoin = package.PriceCoin,
                IsPaid = status == JobStatus.Completed && index % 2 == 0,
                Status = status,
                CreatedAt = createdAt,
                TimeStartAt = timeStartAt,
                TimeEndAt = timeEndAt,
                AcceptedAt = acceptedAt,
                RejectedAt = rejectedAt,
                RejectionReason = status == JobStatus.Rejected
                    ? "Thông tin tuyển dụng cần được bổ sung trước khi đăng công khai."
                    : null,
                CancelledAt = cancelledAt,
                CancelReason = status == JobStatus.Cancelled
                    ? "Đơn vị đã dừng tuyển cho ca làm này."
                    : null,
                CompletedAt = status == JobStatus.Completed ? timeEndAt.AddMinutes(30) : null,
                WorkingAt = status is JobStatus.Working or JobStatus.Completed ? timeStartAt : null,
                FullAt = status == JobStatus.Full ? acceptedAt?.AddDays(1) : null,
                ExpiredAt = status == JobStatus.Pending ? createdAt.AddDays(14) : null,
                CreatedBy = creator.Id,
                JobCategoryId = category.Id,
                PostPackageId = index % 9 == 0 ? null : package.Id
            };

            AddImages(job, index);
            AddRequiredSkills(job, skills, index);
            AddApplications(job, users, creator, status, requiredWorkers, createdAt, pricingModel, index);

            relatedRecordsAdded += 1 + job.Images.Count + job.RequiredSkills.Count + job.Applications.Count;
            jobs.Add(job);
        }

        if (jobs.Count > 0)
        {
            await context.Jobs.AddRangeAsync(jobs);
            await context.SaveChangesAsync();
        }

        logger.LogInformation(
            "Development job sample data: added {JobCount} job(s) and {RelatedCount} related record(s).",
            jobs.Count,
            relatedRecordsAdded - packagesAdded);
        return relatedRecordsAdded;
    }

    private static async Task<int> EnsurePackagesAsync(AppDbContext context)
    {
        var existingNames = await context.PostPackages
            .Select(package => package.Name)
            .ToHashSetAsync();
        var packages = new List<PostPackageEntity>();
        (string Name, int Days, int Coins, decimal Price)[] definitions =
        [
            (PackageNames[0], 7, 30, 15_000m),
            (PackageNames[1], 14, 55, 28_000m),
            (PackageNames[2], 30, 120, 60_000m),
            (PackageNames[3], 3, 20, 10_000m),
            (PackageNames[4], 30, 180, 90_000m)
        ];

        foreach (var definition in definitions)
        {
            if (!existingNames.Add(definition.Name))
                continue;

            packages.Add(new PostPackageEntity
            {
                Name = definition.Name,
                DurationDays = definition.Days,
                PriceCoin = definition.Coins,
                PriceMoney = definition.Price
            });
        }

        if (packages.Count == 0)
            return 0;

        await context.PostPackages.AddRangeAsync(packages);
        await context.SaveChangesAsync();
        return packages.Count;
    }

    private static JobStatus GetStatus(int index) => (index % 12) switch
    {
        < 3 => JobStatus.Pending,
        < 6 => JobStatus.Accepted,
        < 8 => JobStatus.Rejected,
        8 => JobStatus.Full,
        9 => JobStatus.Working,
        10 => JobStatus.Completed,
        _ => JobStatus.Cancelled
    };

    private static DateTimeOffset GetCreatedAt(
        DateTimeOffset now,
        JobStatus status,
        int index,
        DateTimeOffset creatorCreatedAt)
    {
        var daysAgo = status switch
        {
            JobStatus.Completed => 35 + index % 20,
            JobStatus.Working => 12 + index % 12,
            JobStatus.Pending => 1 + index % 12,
            _ => 1 + index % 44
        };
        var createdAt = now.AddDays(-daysAgo);
        return createdAt > creatorCreatedAt ? createdAt : creatorCreatedAt.AddHours(1);
    }

    private static void AddImages(JobEntity job, int index)
    {
        var imageCount = 1 + index % 3;
        for (var imageIndex = 0; imageIndex < imageCount; imageIndex++)
        {
            job.Images.Add(new JobImageEntity
            {
                Image = $"https://example.test/development/jobs/{index + 1:000}/workplace-{imageIndex + 1}.jpg"
            });
        }
    }

    private static void AddRequiredSkills(JobEntity job, IReadOnlyList<SkillSample> skills, int index)
    {
        var skillCount = Math.Min(3 + index % 4, skills.Count);
        var usedSkillIds = new HashSet<int>();
        for (var offset = 0; usedSkillIds.Count < skillCount; offset++)
        {
            var skill = skills[(index * 7 + offset) % skills.Count];
            if (!usedSkillIds.Add(skill.Id))
                continue;

            job.RequiredSkills.Add(new JobRequiredSkillEntity { SkillId = skill.Id });
        }
    }

    private static void AddApplications(
        JobEntity job,
        IReadOnlyList<UserEntity> users,
        UserEntity creator,
        JobStatus status,
        int requiredWorkers,
        DateTimeOffset createdAt,
        JobPricingModel pricingModel,
        int index)
    {
        var requestedCount = status switch
        {
            JobStatus.Pending => index % 8,
            JobStatus.Accepted => 2 + index % 9,
            JobStatus.Rejected => index % 4,
            JobStatus.Full => requiredWorkers + 2,
            JobStatus.Working => requiredWorkers + 1,
            JobStatus.Completed => requiredWorkers + 3,
            JobStatus.Cancelled => index % 5,
            _ => 0
        };
        var targetCount = Math.Min(requestedCount, users.Count - 1);
        var selectedUserIds = new HashSet<int>();
        for (var offset = 0; selectedUserIds.Count < targetCount; offset++)
        {
            var applicant = users[(index * 5 + offset + 1) % users.Count];
            if (applicant.Id == creator.Id || !selectedUserIds.Add(applicant.Id))
                continue;

            job.Applications.Add(new ApplicationEntity
            {
                UserId = applicant.Id,
                Status = (status is JobStatus.Full or JobStatus.Working or JobStatus.Completed) &&
                         selectedUserIds.Count <= requiredWorkers
                    ? "ACCEPTED"
                    : "PENDING",
                AppliedAt = createdAt.AddMinutes(30 + offset * 15),
                Note = offset % 4 == 0 ? "Có thể nhận ca theo lịch đã đăng." : null,
                ConnectionFeeCoin = pricingModel == JobPricingModel.PerConnection ? 1 + offset % 5 : null
            });
        }
    }

    private sealed record CategorySample(int Id);

    private sealed record SkillSample(int Id);
}
