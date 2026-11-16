using DotNetSkeleton.Modules.AuthModule.Enums;
using DotNetSkeleton.Modules.JobModule.Entities;
using DotNetSkeleton.Modules.JobModule.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Shared.Persistence.Seeders;

public sealed class JobSeeder(
    IConfiguration configuration,
    IWebHostEnvironment environment) : ISeeder
{
    private const int JobCreatorCount = 20;
    public const string FirstJobTitle = "Nhân viên hỗ trợ tiệc cưới";

    public int Order => 5;

    private static readonly JobSeedDefinition[] JobDefinitions =
    [
        new()
        {
            CategoryName = "Phục vụ tiệc lẻ",
            SalaryValue = 45000,
            SalaryUnit = "giờ",
            TotalSalaryPayment = 2250000,
            TotalPlatformFee = 225000.0m,
            TotalExpense = 2475000.0m,
            CompletedAt = null,
            WorkingAt = null,
            FullAt = null,
            RejectedAt = null,
            AcceptedAt = At("2026-11-09T12:00:00+07:00"),
            IsPaid = false,
            Title = FirstJobTitle,
            Description = "Hỗ trợ đón khách, sắp xếp bàn ghế, bưng bê và dọn dẹp trong tiệc cưới tại nhà hàng.",
            Address = "Nhà hàng Sen Hồng, 25 Nguyễn Hữu Cảnh, Bình Thạnh, TP.HCM",
            Longitude = 106.7219m,
            Latitude = 10.7951m,
            CancelledAt = null,
            CreatedAt = At("2026-11-09T09:00:00+07:00"),
            RequiredNumberOfWorkers = 10,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-13T16:00:00+07:00"),
            TimeEndAt = At("2026-11-13T21:00:00+07:00"),
            Status = JobStatus.Accepted,
            RequiredDescription = "Nam/nữ từ 18 tuổi, nhanh nhẹn, ngoại hình gọn gàng, mặc đồng phục (công ty hỗ trợ).",
            AutoReplyMessage = "Cảm ơn bạn đã ứng tuyển! Vui lòng có mặt trước giờ bắt đầu 30 phút.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 10,
        },
        new()
        {
            CategoryName = "Khảo sát thị trường",
            SalaryValue = 50000,
            SalaryUnit = "",
            TotalSalaryPayment = 1000000,
            TotalPlatformFee = 100000.0m,
            TotalExpense = 1100000.0m,
            CompletedAt = null,
            WorkingAt = At("2026-11-10T08:00:00+07:00"),
            FullAt = At("2026-11-08T21:00:00+07:00"),
            RejectedAt = null,
            AcceptedAt = At("2026-11-08T11:00:00+07:00"),
            IsPaid = false,
            Title = "Cộng tác viên tạo tài khoản ngân hàng",
            Description = "Hỗ trợ khách hàng đăng ký tài khoản ngân hàng số theo hướng dẫn, nhận thù lao theo số tài khoản thành công.",
            Address = "Tòa nhà Bitexco, 2 Hải Triều, Quận 1, TP.HCM",
            Longitude = 106.7044m,
            Latitude = 10.7716m,
            CancelledAt = null,
            CreatedAt = At("2026-11-08T08:00:00+07:00"),
            RequiredNumberOfWorkers = 20,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-10T08:00:00+07:00"),
            TimeEndAt = At("2026-11-10T16:00:00+07:00"),
            Status = JobStatus.Working,
            RequiredDescription = "Có CCCD, smartphone, biết sử dụng ứng dụng ngân hàng cơ bản.",
            AutoReplyMessage = "Chào bạn, mình sẽ gửi hướng dẫn chi tiết qua tin nhắn sớm nhất.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 15,
        },
        new()
        {
            CategoryName = "Khảo sát thị trường",
            SalaryValue = 120000,
            SalaryUnit = "",
            TotalSalaryPayment = 1800000,
            TotalPlatformFee = 180000.0m,
            TotalExpense = 1980000.0m,
            CompletedAt = null,
            WorkingAt = null,
            FullAt = null,
            RejectedAt = null,
            AcceptedAt = At("2026-11-07T10:00:00+07:00"),
            IsPaid = false,
            Title = "Tạo tài khoản ngân hàng, mở thẻ online VIB",
            Description = "Thực hiện mở tài khoản và thẻ tín dụng online VIB theo quy trình, nhận thưởng cho mỗi hồ sơ được duyệt.",
            Address = "Đường Nguyễn Văn Linh, Quận 7, TP.HCM",
            Longitude = 106.7218m,
            Latitude = 10.7295m,
            CancelledAt = null,
            CreatedAt = At("2026-11-07T07:00:00+07:00"),
            RequiredNumberOfWorkers = 15,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-11T09:00:00+07:00"),
            TimeEndAt = At("2026-11-11T15:00:00+07:00"),
            Status = JobStatus.Accepted,
            RequiredDescription = "Từ 18 tuổi, có CCCD gắn chip, số điện thoại chính chủ.",
            AutoReplyMessage = "Cảm ơn bạn đã quan tâm! Mình sẽ liên hệ hướng dẫn các bước tiếp theo.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 12,
        },
        new()
        {
            CategoryName = "Nhập dữ liệu",
            SalaryValue = 30000,
            SalaryUnit = "giờ",
            TotalSalaryPayment = 960000,
            TotalPlatformFee = 96000.0m,
            TotalExpense = 1056000.0m,
            CompletedAt = null,
            WorkingAt = null,
            FullAt = null,
            RejectedAt = null,
            AcceptedAt = null,
            IsPaid = false,
            Title = "Hướng dẫn tạo và thu acc",
            Description = "Hướng dẫn người dùng tạo tài khoản trên nền tảng và thu thập thông tin tài khoản theo mẫu.",
            Address = "Đường Cộng Hòa, Tân Bình, TP.HCM",
            Longitude = 106.6527m,
            Latitude = 10.801m,
            CancelledAt = null,
            CreatedAt = At("2026-11-09T06:00:00+07:00"),
            RequiredNumberOfWorkers = 8,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-12T13:00:00+07:00"),
            TimeEndAt = At("2026-11-12T17:00:00+07:00"),
            Status = JobStatus.Pending,
            RequiredDescription = "Cẩn thận, trung thực, biết dùng Excel/Google Sheet cơ bản.",
            AutoReplyMessage = "Đã nhận hồ sơ của bạn, vui lòng chờ phản hồi trong 24 giờ.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 8,
        },
        new()
        {
            CategoryName = "Phụ bếp",
            SalaryValue = 28000,
            SalaryUnit = "giờ",
            TotalSalaryPayment = 504000,
            TotalPlatformFee = 50400.0m,
            TotalExpense = 554400.0m,
            CompletedAt = At("2026-11-08T20:00:00+07:00"),
            WorkingAt = At("2026-11-08T14:00:00+07:00"),
            FullAt = At("2026-11-06T03:00:00+07:00"),
            RejectedAt = null,
            AcceptedAt = At("2026-11-05T17:00:00+07:00"),
            IsPaid = true,
            Title = "Tuyển nhân viên làm bánh tại cơ sở bánh Crepe",
            Description = "Phụ trách làm bánh crepe, chuẩn bị nguyên liệu và giữ vệ sinh khu vực bếp.",
            Address = "Đường Nguyễn Thị Minh Khai, Quận 3, TP.HCM",
            Longitude = 106.6934m,
            Latitude = 10.7829m,
            CancelledAt = null,
            CreatedAt = At("2026-11-05T14:00:00+07:00"),
            RequiredNumberOfWorkers = 3,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-08T14:00:00+07:00"),
            TimeEndAt = At("2026-11-08T20:00:00+07:00"),
            Status = JobStatus.Completed,
            RequiredDescription = "Ưu tiên có kinh nghiệm làm bánh; chịu khó, sạch sẽ, có giấy khám sức khỏe là một lợi thế.",
            AutoReplyMessage = "Chào bạn, quán sẽ liên hệ sắp xếp lịch thử việc nhé!",
            PricingModel = PricingModel.TimeBased,
            PostFeeCoin = 0,
        },
        new()
        {
            CategoryName = "Quay video ngắn",
            SalaryValue = 150000,
            SalaryUnit = "",
            TotalSalaryPayment = 750000,
            TotalPlatformFee = 75000.0m,
            TotalExpense = 825000.0m,
            CompletedAt = null,
            WorkingAt = null,
            FullAt = null,
            RejectedAt = null,
            AcceptedAt = At("2026-11-07T12:00:00+07:00"),
            IsPaid = false,
            Title = "Sáng tạo video ngắn (từ 30s)",
            Description = "Quay và dựng video ngắn từ 30 giây giới thiệu sản phẩm để đăng TikTok, Reels.",
            Address = "Làm việc tại nhà (nhận brief qua Zalo), TP.HCM",
            Longitude = 106.7009m,
            Latitude = 10.7769m,
            CancelledAt = null,
            CreatedAt = At("2026-11-07T09:00:00+07:00"),
            RequiredNumberOfWorkers = 5,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-15T09:00:00+07:00"),
            TimeEndAt = At("2026-11-16T09:00:00+07:00"),
            Status = JobStatus.Accepted,
            RequiredDescription = "Biết dùng CapCut hoặc phần mềm dựng tương đương, có portfolio là lợi thế.",
            AutoReplyMessage = "Cảm ơn bạn! Vui lòng gửi thêm 1-2 video mẫu bạn đã thực hiện.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 10,
        },
        new()
        {
            CategoryName = "Phục vụ nhà hàng",
            SalaryValue = 25000,
            SalaryUnit = "giờ",
            TotalSalaryPayment = 600000,
            TotalPlatformFee = 60000.0m,
            TotalExpense = 660000.0m,
            CompletedAt = null,
            WorkingAt = null,
            FullAt = At("2026-11-09T21:00:00+07:00"),
            RejectedAt = null,
            AcceptedAt = At("2026-11-09T11:00:00+07:00"),
            IsPaid = false,
            Title = "Tuyển dụng nhân viên phục vụ",
            Description = "Phục vụ bàn, order món, dọn dẹp và hỗ trợ thu ngân tại quán ăn.",
            Address = "Đường Phan Xích Long, Phú Nhuận, TP.HCM",
            Longitude = 106.6823m,
            Latitude = 10.7993m,
            CancelledAt = null,
            CreatedAt = At("2026-11-09T08:00:00+07:00"),
            RequiredNumberOfWorkers = 4,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-11T17:00:00+07:00"),
            TimeEndAt = At("2026-11-11T23:00:00+07:00"),
            Status = JobStatus.Accepted,
            RequiredDescription = "Từ 18 tuổi, giao tiếp tốt, có thể làm ca tối và cuối tuần.",
            AutoReplyMessage = "Chào bạn, quán đã nhận thông tin và sẽ liên hệ phỏng vấn.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 8,
        },
        new()
        {
            CategoryName = "Quản lý fanpage",
            SalaryValue = 20000,
            SalaryUnit = "giờ",
            TotalSalaryPayment = 480000,
            TotalPlatformFee = 48000.0m,
            TotalExpense = 528000.0m,
            CompletedAt = null,
            WorkingAt = At("2026-11-10T18:00:00+07:00"),
            FullAt = At("2026-11-08T20:00:00+07:00"),
            RejectedAt = null,
            AcceptedAt = At("2026-11-08T10:00:00+07:00"),
            IsPaid = false,
            Title = "Tuyển cộng tác viên rep inbox khách thuê phòng",
            Description = "Trả lời tin nhắn khách hỏi thuê phòng trên fanpage và Zalo, chốt lịch xem phòng.",
            Address = "Làm việc online, khu vực Gò Vấp, TP.HCM",
            Longitude = 106.6652m,
            Latitude = 10.8386m,
            CancelledAt = null,
            CreatedAt = At("2026-11-08T07:00:00+07:00"),
            RequiredNumberOfWorkers = 6,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-10T18:00:00+07:00"),
            TimeEndAt = At("2026-11-10T22:00:00+07:00"),
            Status = JobStatus.Working,
            RequiredDescription = "Gõ phím nhanh, nói chuyện lịch sự, online ít nhất 4 giờ/ngày.",
            AutoReplyMessage = "Cảm ơn bạn đã ứng tuyển, mình sẽ gửi kịch bản trả lời mẫu.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 6,
        },
        new()
        {
            CategoryName = "Bán hàng online",
            SalaryValue = 200000,
            SalaryUnit = "ngày",
            TotalSalaryPayment = 600000,
            TotalPlatformFee = 60000.0m,
            TotalExpense = 660000.0m,
            CompletedAt = null,
            WorkingAt = null,
            FullAt = null,
            RejectedAt = null,
            AcceptedAt = At("2026-11-07T09:00:00+07:00"),
            IsPaid = false,
            Title = "Việc làm bán hàng, chăm sóc khách hàng và gói hàng cho khách",
            Description = "Tư vấn khách, chốt đơn, kiểm tra và đóng gói hàng tại cửa hàng.",
            Address = "Đường Lê Văn Việt, TP. Thủ Đức, TP.HCM",
            Longitude = 106.796m,
            Latitude = 10.847m,
            CancelledAt = null,
            CreatedAt = At("2026-11-07T06:00:00+07:00"),
            RequiredNumberOfWorkers = 3,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-12T08:00:00+07:00"),
            TimeEndAt = At("2026-11-12T16:00:00+07:00"),
            Status = JobStatus.Accepted,
            RequiredDescription = "Nhanh nhẹn, trung thực, có thể đứng làm việc lâu.",
            AutoReplyMessage = "Chào bạn, cửa hàng sẽ liên hệ trong thời gian sớm nhất.",
            PricingModel = PricingModel.TimeBased,
            PostFeeCoin = 0,
        },
        new()
        {
            CategoryName = "Trực quầy / Tư vấn",
            SalaryValue = 300000,
            SalaryUnit = "",
            TotalSalaryPayment = 3000000,
            TotalPlatformFee = 300000.0m,
            TotalExpense = 3300000.0m,
            CompletedAt = null,
            WorkingAt = null,
            FullAt = null,
            RejectedAt = null,
            AcceptedAt = null,
            IsPaid = false,
            Title = "Tuyển cộng tác viên sale phòng trọ",
            Description = "Giới thiệu và dẫn khách xem phòng trọ, nhận hoa hồng cho mỗi hợp đồng thuê thành công.",
            Address = "Đường Quang Trung, Gò Vấp, TP.HCM",
            Longitude = 106.6683m,
            Latitude = 10.8365m,
            CancelledAt = null,
            CreatedAt = At("2026-11-09T05:00:00+07:00"),
            RequiredNumberOfWorkers = 10,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-17T08:00:00+07:00"),
            TimeEndAt = At("2026-11-17T18:00:00+07:00"),
            Status = JobStatus.Pending,
            RequiredDescription = "Thông thuộc khu vực, có xe máy, giao tiếp tốt.",
            AutoReplyMessage = "Cảm ơn bạn! Mình sẽ gửi danh sách phòng đang trống để bạn nắm trước.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 10,
        },
        new()
        {
            CategoryName = "Chụp ảnh sản phẩm",
            SalaryValue = 35000,
            SalaryUnit = "giờ",
            TotalSalaryPayment = 350000,
            TotalPlatformFee = 35000.0m,
            TotalExpense = 385000.0m,
            CompletedAt = null,
            WorkingAt = null,
            FullAt = null,
            RejectedAt = null,
            AcceptedAt = At("2026-11-08T12:00:00+07:00"),
            IsPaid = false,
            Title = "Cộng tác viên chụp ảnh sản phẩm & nhập liệu",
            Description = "Chụp ảnh sản phẩm thời trang theo yêu cầu và nhập thông tin sản phẩm lên file quản lý.",
            Address = "Đường Võ Văn Tần, Quận 3, TP.HCM",
            Longitude = 106.6879m,
            Latitude = 10.7766m,
            CancelledAt = null,
            CreatedAt = At("2026-11-08T09:00:00+07:00"),
            RequiredNumberOfWorkers = 2,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-14T09:00:00+07:00"),
            TimeEndAt = At("2026-11-14T14:00:00+07:00"),
            Status = JobStatus.Accepted,
            RequiredDescription = "Có smartphone chụp đẹp hoặc máy ảnh, biết chỉnh ảnh cơ bản, nhập liệu chính xác.",
            AutoReplyMessage = "Đã nhận thông tin, shop sẽ gửi hướng dẫn chụp ảnh qua tin nhắn.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 8,
        },
        new()
        {
            CategoryName = "Banner quảng cáo",
            SalaryValue = 100000,
            SalaryUnit = "",
            TotalSalaryPayment = 300000,
            TotalPlatformFee = 30000.0m,
            TotalExpense = 330000.0m,
            CompletedAt = null,
            WorkingAt = null,
            FullAt = null,
            RejectedAt = null,
            AcceptedAt = At("2026-11-07T11:00:00+07:00"),
            IsPaid = false,
            Title = "Thiết kế banner",
            Description = "Thiết kế banner quảng cáo cho fanpage và sàn thương mại điện tử theo brief có sẵn.",
            Address = "Làm việc online, TP.HCM",
            Longitude = 106.7009m,
            Latitude = 10.7769m,
            CancelledAt = null,
            CreatedAt = At("2026-11-07T08:00:00+07:00"),
            RequiredNumberOfWorkers = 3,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-13T09:00:00+07:00"),
            TimeEndAt = At("2026-11-13T17:00:00+07:00"),
            Status = JobStatus.Accepted,
            RequiredDescription = "Thành thạo Photoshop hoặc Canva, gửi kèm portfolio.",
            AutoReplyMessage = "Cảm ơn bạn! Vui lòng gửi portfolio để shop xem trước.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 8,
        },
        new()
        {
            CategoryName = "Tiếng Anh",
            SalaryValue = 40000,
            SalaryUnit = "giờ",
            TotalSalaryPayment = 800000,
            TotalPlatformFee = 80000.0m,
            TotalExpense = 880000.0m,
            CompletedAt = null,
            WorkingAt = At("2026-11-10T19:00:00+07:00"),
            FullAt = At("2026-11-09T20:00:00+07:00"),
            RejectedAt = null,
            AcceptedAt = At("2026-11-09T10:00:00+07:00"),
            IsPaid = false,
            Title = "Rep tin nhắn với người nước ngoài",
            Description = "Trả lời tin nhắn tiếng Anh với khách quốc tế về dịch vụ lưu trú và tour.",
            Address = "Đường Bùi Viện, Quận 1, TP.HCM",
            Longitude = 106.6937m,
            Latitude = 10.7672m,
            CancelledAt = null,
            CreatedAt = At("2026-11-09T07:00:00+07:00"),
            RequiredNumberOfWorkers = 4,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-10T19:00:00+07:00"),
            TimeEndAt = At("2026-11-11T00:00:00+07:00"),
            Status = JobStatus.Working,
            RequiredDescription = "Tiếng Anh giao tiếp tốt (tương đương IELTS 5.5+), phản hồi nhanh.",
            AutoReplyMessage = "Hi, thank you for applying! We'll send you the message templates soon.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 10,
        },
        new()
        {
            CategoryName = "Tư vấn tài chính",
            SalaryValue = 180000,
            SalaryUnit = "ngày",
            TotalSalaryPayment = 900000,
            TotalPlatformFee = 90000.0m,
            TotalExpense = 990000.0m,
            CompletedAt = At("2026-11-05T16:00:00+07:00"),
            WorkingAt = At("2026-11-05T08:00:00+07:00"),
            FullAt = At("2026-11-02T21:00:00+07:00"),
            RejectedAt = null,
            AcceptedAt = At("2026-11-02T11:00:00+07:00"),
            IsPaid = true,
            Title = "Tư vấn viên công ty CoreAgent",
            Description = "Tư vấn giải pháp phần mềm cho khách hàng doanh nghiệp qua điện thoại và Zalo.",
            Address = "Tòa nhà E.Town, 364 Cộng Hòa, Tân Bình, TP.HCM",
            Longitude = 106.653m,
            Latitude = 10.8012m,
            CancelledAt = null,
            CreatedAt = At("2026-11-02T08:00:00+07:00"),
            RequiredNumberOfWorkers = 5,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-05T08:00:00+07:00"),
            TimeEndAt = At("2026-11-05T16:00:00+07:00"),
            Status = JobStatus.Completed,
            RequiredDescription = "Giọng nói rõ ràng, kỹ năng thuyết phục, biết dùng máy tính.",
            AutoReplyMessage = "Chào bạn, CoreAgent đã nhận hồ sơ và sẽ liên hệ lịch phỏng vấn.",
            PricingModel = PricingModel.TimeBased,
            PostFeeCoin = 0,
        },
        new()
        {
            CategoryName = "Pha chế",
            SalaryValue = 27000,
            SalaryUnit = "giờ",
            TotalSalaryPayment = 486000,
            TotalPlatformFee = 48600.0m,
            TotalExpense = 534600.0m,
            CompletedAt = null,
            WorkingAt = null,
            FullAt = null,
            RejectedAt = null,
            AcceptedAt = At("2026-11-07T08:00:00+07:00"),
            IsPaid = false,
            Title = "Kargo coffee tuyển dụng",
            Description = "Pha chế, phục vụ khách và vệ sinh quán cà phê theo ca.",
            Address = "Đường Trần Não, TP. Thủ Đức, TP.HCM",
            Longitude = 106.731m,
            Latitude = 10.8033m,
            CancelledAt = null,
            CreatedAt = At("2026-11-07T05:00:00+07:00"),
            RequiredNumberOfWorkers = 3,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-11T07:00:00+07:00"),
            TimeEndAt = At("2026-11-11T13:00:00+07:00"),
            Status = JobStatus.Accepted,
            RequiredDescription = "Từ 18 tuổi, sẵn sàng học việc, có thể xoay ca sáng/tối.",
            AutoReplyMessage = "Cảm ơn bạn! Quán sẽ mời bạn đến thử việc trong tuần này.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 8,
        },
        new()
        {
            CategoryName = "Viết content",
            SalaryValue = 3000,
            SalaryUnit = "",
            TotalSalaryPayment = 1200000,
            TotalPlatformFee = 120000.0m,
            TotalExpense = 1320000.0m,
            CompletedAt = null,
            WorkingAt = At("2026-11-10T09:00:00+07:00"),
            FullAt = At("2026-11-09T22:00:00+07:00"),
            RejectedAt = null,
            AcceptedAt = At("2026-11-09T12:00:00+07:00"),
            IsPaid = false,
            Title = "Viết content ngắn, comment tương tác bài viết trên tik tok",
            Description = "Viết caption ngắn và bình luận tương tác tự nhiên cho các bài đăng TikTok của thương hiệu.",
            Address = "Làm việc online, TP.HCM",
            Longitude = 106.7009m,
            Latitude = 10.7769m,
            CancelledAt = null,
            CreatedAt = At("2026-11-09T09:00:00+07:00"),
            RequiredNumberOfWorkers = 8,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-10T09:00:00+07:00"),
            TimeEndAt = At("2026-11-10T15:00:00+07:00"),
            Status = JobStatus.Working,
            RequiredDescription = "Hiểu xu hướng TikTok, viết lách sáng tạo, có tài khoản TikTok sử dụng thường xuyên.",
            AutoReplyMessage = "Cảm ơn bạn đã ứng tuyển, mình sẽ gửi bộ nội dung mẫu.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 6,
        },
        new()
        {
            CategoryName = "Livestream",
            SalaryValue = 50000,
            SalaryUnit = "giờ",
            TotalSalaryPayment = 400000,
            TotalPlatformFee = 40000.0m,
            TotalExpense = 440000.0m,
            CompletedAt = null,
            WorkingAt = null,
            FullAt = null,
            RejectedAt = At("2026-11-08T13:00:00+07:00"),
            AcceptedAt = null,
            IsPaid = false,
            Title = "Tuyển nữ livestream",
            Description = "Livestream bán hàng mỹ phẩm và phụ kiện, tương tác và chốt đơn cùng khán giả.",
            Address = "Đường Hoàng Hoa Thám, Tân Bình, TP.HCM",
            Longitude = 106.66m,
            Latitude = 10.801m,
            CancelledAt = null,
            CreatedAt = At("2026-11-08T08:00:00+07:00"),
            RequiredNumberOfWorkers = 2,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-16T19:00:00+07:00"),
            TimeEndAt = At("2026-11-16T23:00:00+07:00"),
            Status = JobStatus.Rejected,
            RequiredDescription = "Nữ 18-30 tuổi, giọng nói truyền cảm, tự tin trước ống kính.",
            AutoReplyMessage = "Chào bạn, vui lòng gửi video giới thiệu bản thân khoảng 30 giây.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 12,
        },
        new()
        {
            CategoryName = "Bán hàng trực tiếp",
            SalaryValue = 230000,
            SalaryUnit = "ngày",
            TotalSalaryPayment = 920000,
            TotalPlatformFee = 92000.0m,
            TotalExpense = 1012000.0m,
            CompletedAt = null,
            WorkingAt = null,
            FullAt = null,
            RejectedAt = null,
            AcceptedAt = At("2026-10-26T10:00:00+07:00"),
            IsPaid = false,
            Title = "Việc làm tư vấn bán hàng",
            Description = "Tư vấn và giới thiệu sản phẩm gia dụng cho khách tại gian hàng trung tâm thương mại.",
            Address = "Vincom Center Landmark 81, Bình Thạnh, TP.HCM",
            Longitude = 106.7218m,
            Latitude = 10.7951m,
            CancelledAt = At("2026-10-27T07:00:00+07:00"),
            CreatedAt = At("2026-10-26T07:00:00+07:00"),
            RequiredNumberOfWorkers = 4,
            ExpiredAt = null,
            TimeStartAt = At("2026-10-31T09:00:00+07:00"),
            TimeEndAt = At("2026-10-31T17:00:00+07:00"),
            Status = JobStatus.Cancelled,
            RequiredDescription = "Ngoại hình dễ nhìn, giao tiếp tốt, ưu tiên có kinh nghiệm bán hàng.",
            AutoReplyMessage = "Cảm ơn bạn! Gian hàng sẽ liên hệ phỏng vấn trực tiếp.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 10,
        },
        new()
        {
            CategoryName = "Pha chế",
            SalaryValue = 30000,
            SalaryUnit = "giờ",
            TotalSalaryPayment = 360000,
            TotalPlatformFee = 36000.0m,
            TotalExpense = 396000.0m,
            CompletedAt = null,
            WorkingAt = null,
            FullAt = null,
            RejectedAt = null,
            AcceptedAt = At("2026-11-09T09:00:00+07:00"),
            IsPaid = false,
            Title = "Phụ bán đồ uống healthy tại HCM",
            Description = "Phụ bán nước ép, sinh tố tại xe đẩy, pha chế đơn giản và thu tiền.",
            Address = "Công viên 23/9, Quận 1, TP.HCM",
            Longitude = 106.693m,
            Latitude = 10.77m,
            CancelledAt = null,
            CreatedAt = At("2026-11-09T06:00:00+07:00"),
            RequiredNumberOfWorkers = 2,
            ExpiredAt = At("2026-11-12T06:00:00+07:00"),
            TimeStartAt = At("2026-11-12T06:00:00+07:00"),
            TimeEndAt = At("2026-11-12T12:00:00+07:00"),
            Status = JobStatus.Cancelled,
            RequiredDescription = "Chăm chỉ, vui vẻ, có thể làm việc ngoài trời.",
            AutoReplyMessage = "Chào bạn, mình sẽ gửi địa điểm và giờ làm cụ thể.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 6,
        },
        new()
        {
            CategoryName = "Bốc xếp",
            SalaryValue = 35000,
            SalaryUnit = "giờ",
            TotalSalaryPayment = 1680000,
            TotalPlatformFee = 168000.0m,
            TotalExpense = 1848000.0m,
            CompletedAt = null,
            WorkingAt = null,
            FullAt = null,
            RejectedAt = null,
            AcceptedAt = At("2026-11-08T08:00:00+07:00"),
            IsPaid = false,
            Title = "Tuyển bốc xếp phụ xe",
            Description = "Bốc xếp hàng hóa lên xuống xe tải và hỗ trợ tài xế giao hàng nội thành.",
            Address = "Kho hàng KCN Tân Bình, Tân Phú, TP.HCM",
            Longitude = 106.628m,
            Latitude = 10.7957m,
            CancelledAt = null,
            CreatedAt = At("2026-11-08T05:00:00+07:00"),
            RequiredNumberOfWorkers = 6,
            ExpiredAt = null,
            TimeStartAt = At("2026-11-11T06:00:00+07:00"),
            TimeEndAt = At("2026-11-11T14:00:00+07:00"),
            Status = JobStatus.Accepted,
            RequiredDescription = "Nam, sức khỏe tốt, có thể bê vác nặng, đúng giờ.",
            AutoReplyMessage = "Cảm ơn bạn! Vui lòng có mặt đúng giờ tại kho.",
            PricingModel = PricingModel.PerConnection,
            PostFeeCoin = 10,
        }
    ];

    public async Task<int> SeedAsync(AppDbContext context)
    {
        if (!environment.ShouldSeedDevelopmentData(configuration))
            return 0;

        var jobCreators = await context.Users
            .Where(user => user.Role == UserRole.User)
            .OrderBy(user => user.Id)
            .Take(JobCreatorCount)
            .ToListAsync();
        if (jobCreators.Count < JobCreatorCount)
        {
            throw new InvalidOperationException(
                $"JobSeeder requires at least {JobCreatorCount} customer accounts.");
        }

        var added = 0;
        var creatorIndex = 0;
        var categoryNames = JobDefinitions
            .Select(definition => definition.CategoryName)
            .Distinct()
            .ToArray();
        var categories = await context.JobCategories
            .Where(category => category.ParentId != null &&
                               categoryNames.Contains(category.Name))
            .ToListAsync();
        var categoriesByName = categories
            .GroupBy(category => category.Name, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.Single(), StringComparer.Ordinal);
        var missingCategory = categoryNames.FirstOrDefault(
            name => !categoriesByName.ContainsKey(name));
        if (missingCategory is not null)
        {
            throw new InvalidOperationException(
                $"Job category '{missingCategory}' is missing. Run CategorySeeder before JobSeeder.");
        }

        var titles = JobDefinitions.Select(definition => definition.Title).ToArray();
        var existingTitles = await context.Jobs
            .Where(job => titles.Contains(job.Title))
            .Select(job => job.Title)
            .ToHashSetAsync();

        foreach (var definition in JobDefinitions)
        {
            if (!existingTitles.Add(definition.Title))
                continue;

            context.Jobs.Add(new JobEntity
            {
                SalaryValue = definition.SalaryValue,
                SalaryUnit = definition.SalaryUnit,
                TotalSalaryPayment = definition.TotalSalaryPayment,
                TotalPlatformFee = definition.TotalPlatformFee,
                TotalExpense = definition.TotalExpense,
                CompletedAt = definition.CompletedAt,
                WorkingAt = definition.WorkingAt,
                FullAt = definition.FullAt,
                RejectedAt = definition.RejectedAt,
                AcceptedAt = definition.AcceptedAt,
                IsPaid = definition.IsPaid,
                Title = definition.Title,
                Description = definition.Description,
                Address = definition.Address,
                Longitude = definition.Longitude,
                Latitude = definition.Latitude,
                CancelledAt = definition.CancelledAt,
                CreatedAt = definition.CreatedAt,
                RequiredNumberOfWorkers = definition.RequiredNumberOfWorkers,
                ExpiredAt = definition.ExpiredAt,
                TimeStartAt = definition.TimeStartAt,
                TimeEndAt = definition.TimeEndAt,
                Status = definition.Status,
                RequiredDescription = definition.RequiredDescription,
                AutoReplyMessage = definition.AutoReplyMessage,
                PricingModel = definition.PricingModel,
                PostFeeCoin = definition.PostFeeCoin,
                CreatedBy = jobCreators[creatorIndex++ % jobCreators.Count].Id,
                JobCategoryId = categoriesByName[definition.CategoryName].Id
            });
            added++;
        }

        if (added > 0)
            await context.SaveChangesAsync();

        return added;
    }

    private static DateTimeOffset At(string value) =>
        DateTimeOffset.Parse(value).ToUniversalTime();

    private sealed class JobSeedDefinition
    {
        public string CategoryName { get; init; } = string.Empty;
        public decimal SalaryValue { get; init; }
        public string SalaryUnit { get; init; } = string.Empty;
        public decimal TotalSalaryPayment { get; init; }
        public decimal TotalPlatformFee { get; init; }
        public decimal TotalExpense { get; init; }
        public DateTimeOffset? CompletedAt { get; init; }
        public DateTimeOffset? WorkingAt { get; init; }
        public DateTimeOffset? FullAt { get; init; }
        public DateTimeOffset? RejectedAt { get; init; }
        public DateTimeOffset? AcceptedAt { get; init; }
        public bool IsPaid { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public decimal? Longitude { get; init; }
        public decimal? Latitude { get; init; }
        public DateTimeOffset? CancelledAt { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public int RequiredNumberOfWorkers { get; init; }
        public DateTimeOffset? ExpiredAt { get; init; }
        public DateTimeOffset TimeStartAt { get; init; }
        public DateTimeOffset TimeEndAt { get; init; }
        public JobStatus Status { get; init; } = JobStatus.Pending;
        public string? RequiredDescription { get; init; }
        public string? AutoReplyMessage { get; init; }
        public PricingModel PricingModel { get; init; } = PricingModel.TimeBased;
        public int PostFeeCoin { get; init; }
    }
}
