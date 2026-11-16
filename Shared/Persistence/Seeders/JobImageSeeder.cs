using DotNetSkeleton.Modules.JobModule.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Shared.Persistence.Seeders;

public sealed class JobImageSeeder(
    IConfiguration configuration,
    IWebHostEnvironment environment) : ISeeder
{
    private static readonly string[] JobTitles =
    [
        "Nhân viên hỗ trợ tiệc cưới",
        "Cộng tác viên tạo tài khoản ngân hàng",
        "Tạo tài khoản ngân hàng, mở thẻ online VIB",
        "Hướng dẫn tạo và thu acc",
        "Tuyển nhân viên làm bánh tại cơ sở bánh Crepe",
        "Sáng tạo video ngắn (từ 30s)",
        "Tuyển dụng nhân viên phục vụ",
        "Tuyển cộng tác viên rep inbox khách thuê phòng",
        "Việc làm bán hàng, chăm sóc khách hàng và gói hàng cho khách",
        "Tuyển cộng tác viên sale phòng trọ",
        "Cộng tác viên chụp ảnh sản phẩm & nhập liệu",
        "Thiết kế banner",
        "Rep tin nhắn với người nước ngoài",
        "Tư vấn viên công ty CoreAgent",
        "Kargo coffee tuyển dụng",
        "Viết content ngắn, comment tương tác bài viết trên tik tok",
        "Tuyển nữ livestream",
        "Việc làm tư vấn bán hàng",
        "Phụ bán đồ uống healthy tại HCM",
        "Tuyển bốc xếp phụ xe"
    ];

    public int Order => 6;

    public async Task<int> SeedAsync(AppDbContext context)
    {
        if (!environment.ShouldSeedDevelopmentData(configuration))
            return 0;

        var jobs = await context.Jobs
            .Where(job => JobTitles.Contains(job.Title))
            .ToDictionaryAsync(job => job.Title, StringComparer.Ordinal);
        var existingImages = await context.JobImages
            .Where(image => jobs.Values.Select(job => job.Id).Contains(image.JobId))
            .ToListAsync();
        var added = 0;

        for (var jobIndex = 0; jobIndex < JobTitles.Length; jobIndex++)
        {
            var title = JobTitles[jobIndex];
            if (!jobs.TryGetValue(title, out var job))
                continue;

            var jobImages = existingImages
                .Where(image => image.JobId == job.Id)
                .ToHashSet();

            for (var imageIndex = 1; imageIndex <= 2; imageIndex++)
            {
                var image = $"https://picsum.photos/seed/job-{jobIndex + 1}-{imageIndex}/1200/800";
                if (jobImages.Any(existing => existing.Image == image))
                    continue;

                var jobImage = new JobImageEntity
                {
                    JobId = job.Id,
                    Image = image
                };
                context.JobImages.Add(jobImage);
                jobImages.Add(jobImage);
                added++;
            }
        }

        if (added > 0)
            await context.SaveChangesAsync();

        return added;
    }
}
