using DotNetSkeleton.Modules.AuthModule.Entities;
using DotNetSkeleton.Modules.AuthModule.Enums;
using DotNetSkeleton.Modules.NotificationModule.Entities;
using DotNetSkeleton.Modules.NotificationModule.Enums;
using DotNetSkeleton.Shared.Persistence;
using DotNetSkeleton.Shared.Persistence.Seeders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.NotificationModule.Seeders;

public sealed class NotificationDevelopmentSampleDataSeeder(
    IConfiguration configuration,
    IWebHostEnvironment environment,
    ILogger<NotificationDevelopmentSampleDataSeeder> logger) : ISeeder
{
    private static readonly NotificationFixture[] Fixtures =
    [
        new("Việc làm thời vụ cuối tuần đã mở đăng ký", "Bạn có thể xem các ca làm cuối tuần mới tại khu vực của mình.", NotificationSendTo.All, null, 30),
        new("Hồ sơ xác thực đã được duyệt", "Hồ sơ định danh của bạn đã được xác thực. Hãy cập nhật kỹ năng để nhà tuyển dụng dễ tìm thấy bạn.", NotificationSendTo.VerifiedUsers, null, 29),
        new("Nhắc xác nhận ca phục vụ tiệc cưới", "Nhà hàng cần xác nhận danh sách nhân sự trước 18:00 hôm nay.", NotificationSendTo.SpecificUser, 0, 28),
        new("Tuyển nhân viên hỗ trợ hội chợ cuối tuần", "Ca làm kéo dài 6 tiếng, có hướng dẫn tại chỗ và thanh toán sau khi hoàn thành.", NotificationSendTo.All, null, 27),
        new("Bổ sung ảnh chân dung rõ nét", "Ảnh chân dung giúp nhà tuyển dụng xác nhận hồ sơ của bạn nhanh hơn.", NotificationSendTo.VerifiedUsers, null, 26),
        new("Bạn được mời nhận ca đóng gói hàng", "Kho hàng đang cần thêm nhân sự cho ca tối ngày mai. Vui lòng xem chi tiết trong danh sách việc làm.", NotificationSendTo.SpecificUser, 5, 25),
        new("Mở thêm ca kiểm kê hàng hóa", "Các cửa hàng trong khu vực đang tuyển người kiểm kê theo ca ngắn hạn.", NotificationSendTo.All, null, 24),
        new("Cập nhật thông tin thanh toán", "Vui lòng kiểm tra thông tin nhận thanh toán trước khi bắt đầu công việc mới.", NotificationSendTo.VerifiedUsers, null, 23),
        new("Lịch phỏng vấn cộng tác viên sự kiện", "Đơn vị tổ chức sẽ liên hệ các ứng viên phù hợp trong hôm nay.", NotificationSendTo.SpecificUser, 11, 22),
        new("Cơ hội làm thêm tại quán cà phê", "Một số quán đang cần nhân viên phục vụ theo ca sáng và ca tối.", NotificationSendTo.All, null, 21),
        new("Hướng dẫn chuẩn bị cho ca làm đầu tiên", "Hãy đến trước giờ nhận ca 10 phút và mang theo giấy tờ tùy thân.", NotificationSendTo.VerifiedUsers, null, 20),
        new("Xác nhận lịch giao hàng ngày mai", "Vui lòng phản hồi với điều phối viên để chốt khung giờ giao hàng.", NotificationSendTo.SpecificUser, 17, 19),
        new("Việc làm hỗ trợ sự kiện tại trung tâm hội nghị", "Đang tuyển nhân sự đón khách, hướng dẫn chỗ ngồi và hỗ trợ đăng ký.", NotificationSendTo.All, null, 18),
        new("Hồ sơ của bạn đã sẵn sàng ứng tuyển", "Thông tin định danh đã được xác nhận. Bạn có thể ứng tuyển các công việc yêu cầu hồ sơ xác thực.", NotificationSendTo.VerifiedUsers, null, 17),
        new("Nhắc gửi bảng chấm công", "Vui lòng gửi xác nhận giờ làm để đơn vị tuyển dụng hoàn tất đối soát.", NotificationSendTo.SpecificUser, 23, 16),
        new("Tuyển nhân viên bán hàng hội chợ", "Ca 4 tiếng, phù hợp với người muốn làm thêm trong ngày nghỉ.", NotificationSendTo.All, null, 15),
        new("Mẹo tăng khả năng được chọn", "Hãy mô tả kinh nghiệm theo từng công việc và giữ số điện thoại luôn liên lạc được.", NotificationSendTo.VerifiedUsers, null, 14),
        new("Cập nhật địa điểm nhận đồng phục", "Điểm nhận đồng phục đã chuyển sang sảnh phụ của trung tâm sự kiện.", NotificationSendTo.SpecificUser, 29, 13),
        new("Các ca làm ngắn hạn trong tuần này", "Danh sách việc làm mới gồm phục vụ, đóng gói, kiểm kê và hỗ trợ sự kiện.", NotificationSendTo.All, null, 12),
        new("Thông tin về thời gian thanh toán", "Thời gian thanh toán được ghi trong từng tin tuyển dụng. Hãy xác nhận với người phụ trách trước khi nhận ca.", NotificationSendTo.VerifiedUsers, null, 11),
        new("Bạn đã được xác nhận cho ca thu ngân", "Đơn vị tuyển dụng đã xác nhận lịch làm. Vui lòng kiểm tra giờ bắt đầu và địa chỉ.", NotificationSendTo.SpecificUser, 35, 10),
        new("Tuyển cộng tác viên phát mẫu thử", "Công việc trong ngày, có hướng dẫn sản phẩm trước khi bắt đầu.", NotificationSendTo.All, null, 9),
        new("Cập nhật chính sách an toàn khi làm việc", "Nếu địa điểm hoặc nội dung công việc khác với mô tả, hãy liên hệ bộ phận hỗ trợ trước khi tiếp tục.", NotificationSendTo.VerifiedUsers, null, 8),
        new("Nhắc có mặt đúng giờ tại điểm tập trung", "Nhóm trưởng sẽ điểm danh trước khi di chuyển đến địa điểm làm việc.", NotificationSendTo.SpecificUser, 41, 7),
        new("Tuyển nhân viên phụ bếp theo ca", "Nhà hàng cần người sơ chế và chuẩn bị nguyên liệu cho ca tối.", NotificationSendTo.All, null, 6),
        new("Kiểm tra lại kỹ năng trong hồ sơ", "Hãy chọn các kỹ năng phù hợp với công việc bạn muốn nhận.", NotificationSendTo.VerifiedUsers, null, 5),
        new("Thay đổi giờ bắt đầu ca hỗ trợ kho", "Ca ngày mai bắt đầu lúc 08:30 thay vì 09:00. Vui lòng xác nhận đã nhận thông tin.", NotificationSendTo.SpecificUser, 47, 4),
        new("Nhiều công việc mới quanh khu vực của bạn", "Mở danh sách việc làm để xem các ca ngắn hạn vừa được đăng.", NotificationSendTo.All, null, 3),
        new("Thông báo bảo trì hệ thống", "Một số chức năng có thể tạm thời gián đoạn trong khung giờ bảo trì tối nay.", NotificationSendTo.VerifiedUsers, null, 2),
        new("Cảm ơn bạn đã hoàn thành công việc", "Đánh giá sau mỗi ca làm giúp cải thiện chất lượng kết nối giữa người làm và nhà tuyển dụng.", NotificationSendTo.SpecificUser, 53, 1)
    ];

    private static readonly string[] SamplePhones = Enumerable.Range(0, 60)
        .Select(index => $"+1202555{100 + index:0000}")
        .ToArray();

    public int Order => 8;

    public async Task<int> SeedAsync(AppDbContext context)
    {
        if (!environment.ShouldSeedDevelopmentData(configuration))
            return 0;

        var adminId = await context.Users
            .Where(user => user.Role == UserRole.Admin)
            .OrderBy(user => user.Id)
            .Select(user => (int?)user.Id)
            .FirstOrDefaultAsync();
        if (adminId is null)
        {
            logger.LogInformation("Notification sample data skipped because no admin account exists.");
            return 0;
        }

        var users = await context.Users
            .Where(user => user.Role == UserRole.User)
            .OrderBy(user => user.Id)
            .ToListAsync();
        if (users.Count == 0)
            return 0;

        var sampleUsers = users
            .Where(user => SamplePhones.Contains(user.Phone))
            .OrderBy(user => user.Id)
            .ToArray();
        if (sampleUsers.Length == 0)
            return 0;

        var titles = Fixtures.Select(fixture => fixture.Title).ToArray();
        var existingNotifications = await context.Notifications
            .Include(notification => notification.Details)
            .Where(notification => titles.Contains(notification.Title))
            .ToDictionaryAsync(notification => notification.Title);
        var allUserIds = users.Select(user => user.Id).ToArray();
        var verifiedUserIds = users
            .Where(user => user.IdentityVerifiedAt is not null)
            .Select(user => user.Id)
            .ToArray();
        var now = DateTimeOffset.UtcNow;
        var addedRecords = 0;

        for (var fixtureIndex = 0; fixtureIndex < Fixtures.Length; fixtureIndex++)
        {
            var fixture = Fixtures[fixtureIndex];
            var createdAt = now.AddDays(-fixture.DaysAgo);
            var notificationExists = existingNotifications.TryGetValue(fixture.Title, out var notification);
            var recipientIds = GetRecipientIds(fixture, sampleUsers, allUserIds, verifiedUserIds);
            if (recipientIds.Length == 0)
                continue;

            if (!notificationExists)
            {
                notification = new NotificationEntity
                {
                    Title = fixture.Title,
                    Content = fixture.Content,
                    SendTo = fixture.SendTo,
                    CreatedBy = adminId.Value,
                    CreatedAt = createdAt
                };

                foreach (var (userId, recipientIndex) in recipientIds.Select((userId, index) => (userId, index)))
                {
                    notification.Details.Add(new NotificationDetailEntity
                    {
                        UserId = userId,
                        ReadAt = GetReadAt(createdAt, fixtureIndex, recipientIndex)
                    });
                }

                context.Notifications.Add(notification);
                existingNotifications.Add(fixture.Title, notification);
                addedRecords += 1 + notification.Details.Count;
                continue;
            }

            var knownRecipientIds = notification!.Details.Select(detail => detail.UserId).ToHashSet();
            var missingDetails = recipientIds
                .Where(userId => knownRecipientIds.Add(userId))
                .Select((userId, recipientIndex) => new NotificationDetailEntity
                {
                    NotificationId = notification.Id,
                    UserId = userId,
                    ReadAt = GetReadAt(notification.CreatedAt, fixtureIndex, recipientIndex)
                })
                .ToArray();
            if (missingDetails.Length == 0)
                continue;

            await context.NotificationDetails.AddRangeAsync(missingDetails);
            addedRecords += missingDetails.Length;
        }

        if (addedRecords > 0)
            await context.SaveChangesAsync();

        logger.LogInformation(
            "Development notification sample data: added {AddedCount} notification and recipient record(s).",
            addedRecords);
        return addedRecords;
    }

    private static int[] GetRecipientIds(
        NotificationFixture fixture,
        IReadOnlyList<UserEntity> sampleUsers,
        int[] allUserIds,
        int[] verifiedUserIds) =>
        fixture.SendTo switch
        {
            NotificationSendTo.All => allUserIds,
            NotificationSendTo.VerifiedUsers => verifiedUserIds,
            NotificationSendTo.SpecificUser when fixture.SampleUserIndex is { } index =>
                [sampleUsers[index % sampleUsers.Count].Id],
            _ => []
        };

    private static DateTimeOffset? GetReadAt(DateTimeOffset createdAt, int fixtureIndex, int recipientIndex) =>
        (fixtureIndex + recipientIndex) % 4 == 0
            ? null
            : createdAt.AddHours(1 + (fixtureIndex + recipientIndex) % 12);

    private sealed record NotificationFixture(
        string Title,
        string Content,
        NotificationSendTo SendTo,
        int? SampleUserIndex,
        int DaysAgo);
}
