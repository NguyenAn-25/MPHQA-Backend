using DotNetSkeleton.Modules.AuthModule.Entities;
using DotNetSkeleton.Modules.AuthModule.Enums;
using DotNetSkeleton.Modules.JobModule.Entities;
using DotNetSkeleton.Modules.ReportModule.Entities;
using DotNetSkeleton.Modules.ReportModule.Enums;
using DotNetSkeleton.Shared.Persistence;
using DotNetSkeleton.Shared.Persistence.Seeders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.ReportModule.Seeders;

public sealed class ReportDevelopmentSampleDataSeeder(
    IWebHostEnvironment environment,
    ILogger<ReportDevelopmentSampleDataSeeder> logger) : ISeeder
{
    private const string TitlePrefix = "[DEV REPORT]";
    private const int SampleUserCount = 60;

    private static readonly ReportTemplate[] JobReportTemplates =
    [
        new("Chưa nhận được tiền công sau ca làm", "Người lao động cho biết đã hoàn thành ca nhưng chưa nhận được khoản thanh toán đã thỏa thuận."),
        new("Mô tả công việc khác thực tế", "Nhiệm vụ được yêu cầu tại địa điểm làm việc khác với mô tả trong tin tuyển dụng."),
        new("Địa điểm làm việc không chính xác", "Địa chỉ trong tin đăng không khớp với địa điểm được gửi sau khi ứng tuyển."),
        new("Yêu cầu đặt cọc trước khi nhận việc", "Người đăng yêu cầu chuyển tiền đặt cọc để giữ ca làm, nội dung này không có trong tin tuyển dụng."),
        new("Tin tuyển dụng đã hết hạn", "Thời gian làm việc đã qua nhưng tin vẫn hiển thị và tiếp tục nhận ứng viên."),
        new("Thông tin mức lương không rõ ràng", "Mức lương thực nhận và cách tính phụ cấp chưa được giải thích thống nhất."),
        new("Thời gian làm việc bị thay đổi", "Khung giờ làm việc được thay đổi sau khi người lao động đã xác nhận nhận ca."),
        new("Số lượng nhân sự tuyển không đúng", "Đơn vị đăng tuyển thông báo đã đủ người nhưng tin vẫn cho phép ứng tuyển."),
        new("Nội dung tin có dấu hiệu lừa đảo", "Tin yêu cầu cung cấp thông tin cá nhân và chuyển khoản trước khi được xác nhận công việc."),
        new("Liên hệ bên ngoài không an toàn", "Người đăng yêu cầu trao đổi qua đường dẫn đáng ngờ và cung cấp thông tin tài khoản."),
        new("Yêu cầu công việc không phù hợp", "Yêu cầu thực tế có nội dung khác đáng kể so với phần mô tả công việc ngắn hạn."),
        new("Thông tin người tuyển dụng không khớp", "Tên đơn vị liên hệ khác với thông tin hiển thị trên tin tuyển dụng."),
        new("Tin đăng bị trùng lặp", "Cùng một công việc được đăng nhiều lần với thông tin và địa điểm giống nhau."),
        new("Công việc đã bị hủy nhưng chưa cập nhật", "Người lao động được thông báo hủy ca nhưng tin tuyển dụng vẫn đang mở."),
        new("Mức lương thực tế thấp hơn tin đăng", "Khoản thanh toán được thông báo sau khi nhận việc thấp hơn mức ghi trong tin."),
        new("Có yêu cầu làm thêm ngoài thỏa thuận", "Người lao động bị yêu cầu ở lại ngoài khung giờ đã xác nhận mà chưa thống nhất tiền công."),
        new("Tin tuyển dụng thiếu thông tin quan trọng", "Tin chưa nêu rõ thời gian, địa chỉ hoặc nhiệm vụ nhưng đã yêu cầu ứng viên xác nhận ca."),
        new("Người đăng không phản hồi sau khi xác nhận", "Ứng viên đã được xác nhận nhận ca nhưng không thể liên hệ người phụ trách để biết hướng dẫn."),
        new("Hình ảnh minh chứng không liên quan", "Hình ảnh đính kèm không thể hiện địa điểm hoặc nội dung công việc trong tin đăng."),
        new("Dấu hiệu thu phí ứng tuyển", "Tin tuyển dụng yêu cầu trả phí để được xét duyệt hoặc ưu tiên nhận ca làm."),
        new("Mô tả ca làm gây hiểu nhầm", "Tin ghi công việc theo ca ngắn hạn nhưng trao đổi sau đó yêu cầu cam kết thời gian dài."),
        new("Không cung cấp thông tin an toàn", "Tin công việc tại kho chưa nêu các yêu cầu an toàn cơ bản cho người làm ca."),
        new("Quyền lợi trong tin không được thực hiện", "Hỗ trợ ăn uống hoặc đi lại được ghi trong tin nhưng không được áp dụng khi làm việc."),
        new("Nội dung tuyển dụng có dấu hiệu phân biệt", "Tin đăng đưa ra điều kiện cá nhân không liên quan trực tiếp đến khả năng thực hiện công việc.")
    ];

    private static readonly ReportTemplate[] UserReportTemplates =
    [
        new("Ứng viên không đến ca đã xác nhận", "Người dùng xác nhận nhận ca nhưng không có mặt và không báo trước cho người phụ trách."),
        new("Thông tin hồ sơ có dấu hiệu không chính xác", "Một số thông tin kinh nghiệm trong hồ sơ không khớp với thông tin người dùng cung cấp khi trao đổi."),
        new("Gửi tin nhắn quảng cáo lặp lại", "Người dùng gửi nhiều nội dung quảng cáo không liên quan sau khi được yêu cầu dừng lại."),
        new("Có hành vi thiếu tôn trọng khi trao đổi", "Các tin nhắn sử dụng lời lẽ xúc phạm và tiếp tục được gửi sau khi người nhận đã đề nghị dừng."),
        new("Yêu cầu chuyển tiền ngoài nền tảng", "Người dùng đề nghị chuyển tiền trước để được giới thiệu hoặc giữ một công việc ngắn hạn."),
        new("Tài khoản mạo danh người khác", "Ảnh và thông tin hồ sơ có dấu hiệu sử dụng danh tính của người khác."),
        new("Gửi đường dẫn đáng ngờ qua trò chuyện", "Người dùng gửi đường dẫn yêu cầu đăng nhập hoặc cung cấp mã xác minh ngoài ứng dụng."),
        new("Có dấu hiệu thu thập giấy tờ trái mục đích", "Người dùng yêu cầu gửi ảnh giấy tờ cá nhân qua kênh không được xác nhận."),
        new("Hủy ca nhiều lần không báo trước", "Người dùng đã xác nhận nhiều ca làm nhưng liên tục hủy sát giờ mà không thông báo."),
        new("Hồ sơ sử dụng ảnh không phù hợp", "Ảnh đại diện hoặc ảnh trong hồ sơ có nội dung không phù hợp với mục đích tìm việc."),
        new("Đưa ra lời mời việc làm không rõ ràng", "Người dùng mời tham gia công việc nhưng không giải thích nhiệm vụ, địa chỉ hoặc cách thanh toán."),
        new("Gửi nội dung quấy rối", "Người nhận báo cáo đã bị gửi nhiều tin nhắn ngoài nội dung trao đổi công việc."),
        new("Dùng nhiều tài khoản để liên hệ", "Người dùng tiếp tục liên hệ bằng tài khoản khác sau khi bị chặn ở tài khoản ban đầu."),
        new("Không tuân thủ hướng dẫn tại nơi làm", "Người dùng không tuân thủ hướng dẫn của người phụ trách và gây ảnh hưởng đến ca làm."),
        new("Đăng thông tin liên hệ sai lệch", "Số điện thoại hoặc thông tin liên hệ trong hồ sơ không thể kết nối sau nhiều lần thử."),
        new("Có hành vi gây mất an toàn tại nơi làm", "Người báo cáo phản ánh hành vi không tuân thủ hướng dẫn an toàn trong thời gian làm ca."),
        new("Yêu cầu trả phí để được giới thiệu", "Người dùng yêu cầu ứng viên thanh toán khoản phí không được nêu trong tin công việc."),
        new("Gửi nội dung trùng lặp hàng loạt", "Người dùng gửi cùng một nội dung qua nhiều cuộc trò chuyện trong thời gian ngắn."),
        new("Không bàn giao công việc đã nhận", "Người dùng rời ca trước khi bàn giao các nhiệm vụ đã nhận mà không thông báo."),
        new("Có dấu hiệu sử dụng tài khoản tự động", "Tài khoản gửi nhiều yêu cầu giống nhau trong thời gian ngắn và không phản hồi câu hỏi xác nhận."),
        new("Hẹn gặp ngoài địa điểm công việc", "Người dùng đề nghị gặp tại địa điểm khác với nơi làm việc mà không giải thích lý do."),
        new("Cung cấp thông tin thanh toán gây nhầm lẫn", "Người dùng nhiều lần thay đổi thông tin nhận thanh toán sau khi công việc đã hoàn thành."),
        new("Lời mời làm việc không phù hợp", "Nội dung liên hệ có yêu cầu cá nhân không liên quan đến công việc đã trao đổi."),
        new("Có dấu hiệu sử dụng hồ sơ của người khác", "Người xuất hiện tại buổi nhận việc không trùng với người đã xác nhận qua hồ sơ.")
    ];

    private static readonly string[] SamplePhones = Enumerable.Range(0, SampleUserCount)
        .Select(index => $"+1202555{100 + index:0000}")
        .ToArray();

    public int Order => 9;

    public async Task<int> SeedAsync(AppDbContext context)
    {
        if (!environment.IsDevelopment())
            return 0;

        var users = await context.Users
            .Where(user => SamplePhones.Contains(user.Phone) && user.Role == UserRole.User)
            .OrderBy(user => user.Id)
            .ToListAsync();
        if (users.Count < 2)
        {
            logger.LogInformation("Report sample data skipped because fewer than two user fixtures exist.");
            return 0;
        }

        var jobs = await context.Jobs
            .AsNoTracking()
            .OrderBy(job => job.Id)
            .Take(24)
            .Select(job => new JobSample(job.Id, job.CreatedAt))
            .ToListAsync();

        var existingTitles = await context.Reports
            .Where(report => report.Title.StartsWith(TitlePrefix))
            .Select(report => report.Title)
            .ToHashSetAsync();

        var reports = new List<ReportEntity>();
        if (jobs.Count > 0)
        {
            for (var index = 0; index < JobReportTemplates.Length; index++)
            {
                var sender = users[index % users.Count];
                var job = jobs[index % jobs.Count];
                var title = $"{TitlePrefix} Công việc {index + 1:00}: {JobReportTemplates[index].Title}";
                if (!existingTitles.Add(title))
                    continue;

                reports.Add(CreateReport(
                    ReportObjectType.Job,
                    job.Id,
                    sender,
                    JobReportTemplates[index],
                    index,
                    title,
                    job.CreatedAt));
            }
        }
        else
        {
            logger.LogInformation("Job-target report fixtures skipped because no jobs exist yet.");
        }

        for (var index = 0; index < UserReportTemplates.Length; index++)
        {
            var senderIndex = index % users.Count;
            var targetIndex = (senderIndex + 1 + index / users.Count) % users.Count;
            if (targetIndex == senderIndex)
                targetIndex = (targetIndex + 1) % users.Count;

            var sender = users[senderIndex];
            var target = users[targetIndex];
            var title = $"{TitlePrefix} Người dùng {index + 1:00}: {UserReportTemplates[index].Title}";
            if (!existingTitles.Add(title))
                continue;

            reports.Add(CreateReport(
                ReportObjectType.User,
                target.Id,
                sender,
                UserReportTemplates[index],
                index,
                title,
                target.CreatedAt));
        }

        if (reports.Count == 0)
            return 0;

        await context.Reports.AddRangeAsync(reports);
        await context.SaveChangesAsync();

        logger.LogInformation("Development report sample data: added {AddedCount} report record(s).", reports.Count);
        return reports.Count;
    }

    private static ReportEntity CreateReport(
        ReportObjectType objectType,
        int objectId,
        UserEntity sender,
        ReportTemplate template,
        int index,
        string title,
        DateTimeOffset targetCreatedAt)
    {
        var status = GetStatus(index);
        var createdAt = DateTimeOffset.UtcNow.AddDays(-1 - index % 44);
        var earliestCreatedAt = sender.CreatedAt > targetCreatedAt ? sender.CreatedAt : targetCreatedAt;
        if (createdAt <= earliestCreatedAt)
            createdAt = earliestCreatedAt.AddHours(1);

        DateTimeOffset? processedAt = status == ReportStatus.Pending
            ? null
            : createdAt.AddHours(2 + index % 36);

        return new ReportEntity
        {
            ObjectType = objectType,
            ObjectId = objectId,
            SenderId = sender.Id,
            Title = title,
            Reason = template.Reason,
            Image = index % 2 == 0
                ? $"https://example.test/development/reports/{objectType.ToString().ToLowerInvariant()}/{index + 1:000}.jpg"
                : null,
            Status = status,
            CreatedAt = createdAt,
            ProcessedAt = processedAt,
            ProcessedNote = status switch
            {
                ReportStatus.Resolved => "Đã kiểm tra thông tin liên quan và xử lý báo cáo.",
                ReportStatus.Rejected => "Thông tin cung cấp chưa đủ căn cứ để xác nhận vi phạm.",
                ReportStatus.Cancelled => "Người gửi đã rút báo cáo.",
                _ => null
            }
        };
    }

    private static ReportStatus GetStatus(int index) => (index % 12) switch
    {
        < 6 => ReportStatus.Pending,
        < 9 => ReportStatus.Resolved,
        < 11 => ReportStatus.Rejected,
        _ => ReportStatus.Cancelled
    };

    private sealed record ReportTemplate(string Title, string Reason);

    private sealed record JobSample(int Id, DateTimeOffset CreatedAt);
}
