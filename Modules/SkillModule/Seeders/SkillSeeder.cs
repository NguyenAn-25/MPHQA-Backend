using DotNetSkeleton.Modules.SkillModule.Entities;
using DotNetSkeleton.Shared.Persistence;
using DotNetSkeleton.Shared.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.SkillModule.Seeders;

public sealed class SkillSeeder : ISeeder
{
    private static readonly string[] SkillNames =
    [
        "Giao tiếp với khách hàng",
        "Chăm sóc khách hàng",
        "Làm việc nhóm",
        "Quản lý thời gian",
        "Đúng giờ",
        "Tinh thần trách nhiệm",
        "Chủ động trong công việc",
        "Khả năng học nhanh",
        "Làm việc dưới áp lực",
        "Giải quyết vấn đề",
        "Chú ý đến chi tiết",
        "Giao tiếp qua điện thoại",
        "Giao tiếp qua tin nhắn",
        "Sắp xếp công việc",
        "Xử lý nhiều việc cùng lúc",
        "Tuân thủ quy trình",
        "Tuân thủ an toàn lao động",
        "Linh hoạt theo ca",
        "Phục vụ bàn",
        "Chuẩn bị bàn ăn",
        "Phụ bếp",
        "Sơ chế thực phẩm",
        "Vệ sinh khu vực bếp",
        "Pha chế đồ uống",
        "Pha cà phê máy",
        "Pha cà phê thủ công",
        "Phục vụ tiệc buffet",
        "Phục vụ tiệc cưới",
        "Đóng gói thức ăn",
        "Đảm bảo vệ sinh an toàn thực phẩm",
        "Sử dụng máy POS",
        "Thu ngân",
        "Tư vấn bán hàng",
        "Giới thiệu sản phẩm",
        "Trưng bày hàng hóa",
        "Sắp xếp hàng lên kệ",
        "Kiểm kê hàng hóa",
        "Phát mẫu dùng thử",
        "Xử lý đơn hàng bán lẻ",
        "Đóng gói quà tặng",
        "Đón khách và check-in sự kiện",
        "Hướng dẫn khách tham dự",
        "Điều phối hàng chờ",
        "Hỗ trợ gian hàng triển lãm",
        "Lắp đặt thiết bị sự kiện",
        "Tháo dỡ sân khấu",
        "Trang trí địa điểm",
        "Hỗ trợ âm thanh sân khấu",
        "Hỗ trợ ánh sáng sân khấu",
        "Hoạt náo sự kiện",
        "Dẫn chương trình",
        "Bán vé sự kiện",
        "Chụp ảnh sự kiện",
        "Quay video sự kiện",
        "Giao hàng bằng xe máy",
        "Giao hàng bằng ô tô",
        "Giao đồ ăn",
        "Giao thư và hồ sơ",
        "Sử dụng ứng dụng giao hàng",
        "Tìm đường và định vị",
        "Lập lộ trình giao hàng",
        "Đóng gói bưu kiện",
        "Quét mã và phân loại bưu kiện",
        "Soạn đơn hàng trong kho",
        "Bốc xếp hàng hóa",
        "Kiểm kê kho",
        "Vận hành xe nâng có chứng chỉ",
        "Vệ sinh nhà ở",
        "Vệ sinh văn phòng",
        "Dọn phòng khách sạn",
        "Vệ sinh sau xây dựng",
        "Giặt và gấp quần áo",
        "Vệ sinh kính",
        "Chăm sóc cây xanh",
        "Làm vườn",
        "Lắp ráp đồ nội thất",
        "Sửa chữa điện dân dụng cơ bản",
        "Sửa chữa nước dân dụng cơ bản",
        "Trông trẻ",
        "Hỗ trợ trẻ em học tập",
        "Đưa đón trẻ",
        "Hỗ trợ người cao tuổi",
        "Sơ cứu cơ bản",
        "Chăm sóc thú cưng",
        "Dắt chó đi dạo",
        "Tắm và vệ sinh thú cưng",
        "Gia sư tiểu học",
        "Gia sư trung học",
        "Gia sư tiếng Anh",
        "Trợ giảng lớp học",
        "Quản lý lớp học",
        "Coi thi",
        "Chấm bài",
        "Nhập liệu",
        "Đánh máy văn bản",
        "Số hóa tài liệu",
        "Lưu trữ hồ sơ",
        "Sử dụng Microsoft Word",
        "Sử dụng Microsoft Excel",
        "Sử dụng Google Sheets",
        "Sử dụng email công việc",
        "Lễ tân văn phòng",
        "Tổng đài chăm sóc khách hàng",
        "Tư vấn khách hàng qua điện thoại",
        "Trả lời tin nhắn khách hàng",
        "Gọi xác nhận đơn hàng",
        "Khảo sát khách hàng",
        "Nhập liệu bằng bảng tính",
        "Gắn nhãn dữ liệu",
        "Kiểm tra chất lượng dữ liệu",
        "Chuyển âm thanh thành văn bản",
        "Viết nội dung mạng xã hội",
        "Biên tập nội dung",
        "Thiết kế bằng Canva",
        "Chỉnh sửa ảnh cơ bản",
        "Dựng video ngắn",
        "Livestream bán hàng",
        "Quản lý bình luận trực tuyến",
        "Đăng bài lên mạng xã hội",
        "Hỗ trợ quảng cáo tại điểm bán",
        "Phát tờ rơi",
        "Phỏng vấn khảo sát trực tiếp",
        "Khảo sát qua điện thoại",
        "Cài đặt máy tính",
        "Cài đặt phần mềm",
        "Hỗ trợ kỹ thuật cơ bản",
        "Kiểm thử ứng dụng cơ bản",
        "Kiểm tra lỗi website",
        "Sử dụng công cụ quản lý công việc",
        "Thu hoạch nông sản",
        "Phân loại nông sản",
        "Đóng gói nông sản",
        "Trồng và chăm sóc cây",
        "Sơ chế thủy sản",
        "Lắp ráp sản phẩm",
        "Đóng gói sản phẩm tại xưởng",
        "Kiểm tra sản phẩm đầu ra",
        "Dán nhãn sản phẩm",
        "May gia công cơ bản",
        "Làm đồ thủ công",
        "Phụ hồ",
        "Sơn tường cơ bản",
        "Sử dụng dụng cụ cầm tay",
        "Vệ sinh công trường",
        "Vận chuyển vật liệu xây dựng",
        "Điều phối bãi xe",
        "Bảo vệ sự kiện",
        "Kiểm soát khách ra vào",
        "Rửa xe",
        "Chăm sóc xe cơ bản",
        "Rửa xe máy",
        "Rửa ô tô",
        "Tiếng Anh giao tiếp",
        "Tiếng Trung giao tiếp",
        "Tiếng Hàn giao tiếp",
        "Tiếng Nhật giao tiếp",
        "Tiếng Việt chuẩn hóa văn bản"
    ];

    public int Order => 6;

    public async Task<int> SeedAsync(AppDbContext context)
    {
        var existingNames = await context.Skills
            .AsNoTracking()
            .Select(skill => skill.Name)
            .ToListAsync();
        var knownNames = new HashSet<string>(
            existingNames.Select(name => name.Trim()),
            StringComparer.OrdinalIgnoreCase);

        var newSkills = SkillNames
            .Where(name => knownNames.Add(name))
            .Select(name => new SkillEntity
            {
                Name = name,
                IsActive = true
            })
            .ToList();
        if (newSkills.Count == 0)
            return 0;

        await context.Skills.AddRangeAsync(newSkills);
        await context.SaveChangesAsync();
        return newSkills.Count;
    }
}
