using DotNetSkeleton.Modules.JobModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Shared.Persistence.Seeders;

public sealed class JobCategorySeeder : ISeeder
{
    private static readonly CategoryDefinition[] Categories =
    [
        new("Ăn uống và nhà hàng", "Các công việc theo ca tại nhà hàng, quán ăn và dịch vụ ẩm thực.",
            ["Phục vụ bàn theo ca", "Phụ bếp thời vụ", "Rửa chén theo ca", "Barista thời vụ", "Pha chế tại sự kiện", "Phục vụ tiệc cưới", "Bán hàng tại quầy thức ăn"]),
        new("Bán lẻ và bán hàng", "Các công việc bán hàng, giới thiệu sản phẩm và hỗ trợ cửa hàng ngắn hạn.",
            ["Nhân viên bán hàng thời vụ", "Thu ngân theo ca", "Tư vấn sản phẩm tại cửa hàng", "Phát mẫu dùng thử", "Sắp xếp hàng hóa lên kệ", "Kiểm kê hàng hóa", "Hỗ trợ khai trương cửa hàng"]),
        new("Giao hàng và vận chuyển", "Các công việc giao nhận, chuyển đồ và hỗ trợ vận chuyển theo chuyến.",
            ["Giao đồ ăn theo đơn", "Giao hàng hóa theo chuyến", "Giao thư và hồ sơ", "Tài xế theo ngày", "Phụ xe giao hàng", "Bốc xếp hàng hóa", "Chuyển nhà theo giờ"]),
        new("Sự kiện và hội nghị", "Nhân sự hỗ trợ hội nghị, triển lãm, chương trình và sự kiện ngắn ngày.",
            ["Đón khách và check-in", "Hướng dẫn chỗ ngồi", "Hỗ trợ gian hàng triển lãm", "Lắp đặt sân khấu", "Tháo dỡ thiết bị sự kiện", "Trang trí địa điểm", "Mascot và hoạt náo viên"]),
        new("Vệ sinh và tạp vụ", "Các công việc vệ sinh theo giờ, theo ca hoặc theo địa điểm.",
            ["Vệ sinh nhà ở theo giờ", "Vệ sinh văn phòng", "Vệ sinh sau xây dựng", "Dọn phòng khách sạn", "Tạp vụ sự kiện", "Vệ sinh cửa hàng sau giờ đóng cửa", "Chăm sóc khu vực công cộng"]),
        new("Sửa chữa và chăm sóc nhà cửa", "Các việc sửa chữa nhỏ và bảo trì nhà ở theo lần hoặc theo ngày.",
            ["Phụ thợ điện dân dụng", "Phụ thợ nước", "Vệ sinh máy lạnh", "Sơn sửa nhà cửa", "Lắp ráp đồ nội thất", "Làm vườn theo buổi", "Hỗ trợ chuyển và sắp xếp đồ đạc"]),
        new("Chăm sóc trẻ em và người cao tuổi", "Công việc chăm sóc, trông nom theo buổi hoặc thời gian ngắn.",
            ["Trông trẻ theo giờ", "Đưa đón trẻ đi học", "Hỗ trợ sinh hoạt cho người cao tuổi", "Đồng hành đi khám bệnh", "Trợ giảng trông trẻ tại sự kiện"]),
        new("Thú cưng", "Dịch vụ chăm sóc thú cưng theo buổi, theo ngày hoặc theo chuyến.",
            ["Trông thú cưng tại nhà", "Dắt chó đi dạo", "Tắm và vệ sinh thú cưng", "Đưa thú cưng đi khám", "Cho thú cưng ăn khi chủ vắng nhà"]),
        new("Giáo dục và gia sư", "Công việc dạy kèm và hỗ trợ lớp học theo buổi hoặc theo khóa ngắn.",
            ["Gia sư theo buổi", "Trợ giảng lớp học", "Trợ giảng ngoại ngữ", "Coi thi và chấm bài", "Hướng dẫn kỹ năng cuối tuần", "Hỗ trợ lớp học trực tuyến"]),
        new("Truyền thông và sáng tạo", "Dự án sáng tạo nội dung, hình ảnh và video theo sản phẩm hoặc sự kiện.",
            ["Chụp ảnh sự kiện", "Quay video ngắn", "Dựng video theo dự án", "Thiết kế bài đăng mạng xã hội", "Viết nội dung quảng bá", "Livestream bán hàng", "Hỗ trợ sản xuất chương trình"]),
        new("Marketing và khảo sát", "Các chiến dịch quảng bá, phát tờ rơi và khảo sát ngắn hạn.",
            ["Phát tờ rơi", "Giới thiệu thương hiệu tại điểm bán", "Khảo sát trực tiếp", "Khảo sát qua điện thoại", "Cộng tác viên quảng bá trực tuyến", "Hỗ trợ kích hoạt thương hiệu"]),
        new("Công nghệ thông tin và dữ liệu", "Công việc kỹ thuật số theo nhiệm vụ, dự án ngắn hoặc khối lượng dữ liệu.",
            ["Nhập liệu theo dự án", "Gắn nhãn dữ liệu", "Kiểm thử ứng dụng", "Cài đặt máy tính và thiết bị", "Hỗ trợ kỹ thuật tại chỗ", "Chuyển âm thanh thành văn bản", "Kiểm tra nội dung trực tuyến"]),
        new("Văn phòng và hành chính", "Công việc hỗ trợ văn phòng, hồ sơ và tiếp đón trong thời gian ngắn.",
            ["Nhập hồ sơ giấy", "Số hóa tài liệu", "Sắp xếp và lưu trữ hồ sơ", "Lễ tân thay ca", "Hỗ trợ nhập liệu văn phòng", "Phụ tá hành chính thời vụ", "Phát thư mời và tài liệu"]),
        new("Du lịch và lưu trú", "Công việc theo ca tại khách sạn, homestay, điểm tham quan và tour.",
            ["Hỗ trợ hướng dẫn viên", "Đón khách tại điểm du lịch", "Phụ trách hành lý khách sạn", "Hỗ trợ nhận và trả phòng", "Dọn phòng lưu trú", "Phục vụ tour trong ngày", "Trực quầy homestay"]),
        new("Nông nghiệp và thủy sản", "Công việc mùa vụ tại trang trại, vườn cây và cơ sở sơ chế nông thủy sản.",
            ["Thu hoạch rau củ", "Hái trái cây theo mùa", "Trồng và chăm sóc cây", "Phân loại nông sản", "Đóng gói nông sản", "Thu hoạch thủy sản", "Sơ chế hải sản theo ca"]),
        new("Sản xuất và kho vận", "Công việc thời vụ tại xưởng sản xuất, kho hàng và trung tâm phân phối.",
            ["Lắp ráp sản phẩm", "Đóng gói tại xưởng", "Kiểm tra sản phẩm", "Phân loại hàng trong kho", "Soạn đơn hàng", "Kiểm kê kho theo ca", "Bốc xếp tại trung tâm phân phối"]),
        new("Xây dựng và công trình", "Công việc phụ trợ tại công trình, công trường và dự án sửa chữa ngắn hạn.",
            ["Phụ hồ theo ngày", "Phụ thợ xây", "Sơn công trình", "Vệ sinh công trường", "Vận chuyển vật liệu", "Hỗ trợ lắp giàn giáo", "Phụ khảo sát hiện trường"]),
        new("Y tế và chăm sóc sức khỏe", "Công việc hỗ trợ phòng khám, chương trình sức khỏe và chăm sóc tại nhà.",
            ["Lễ tân phòng khám thay ca", "Hỗ trợ sự kiện hiến máu", "Hỗ trợ đo khám sức khỏe", "Chăm sóc người bệnh tại nhà", "Giao nhận vật tư y tế", "Cộng tác viên khảo sát sức khỏe"]),
        new("Thể thao và giải trí", "Nhân sự hỗ trợ lớp tập, giải đấu và hoạt động giải trí ngắn ngày.",
            ["Hỗ trợ giải chạy", "Trợ lý trọng tài", "Trợ giảng lớp thể thao", "Trực quầy phòng tập", "Nhân viên hồ bơi theo ca", "Hỗ trợ khu vui chơi", "Hướng dẫn trò chơi sự kiện"]),
        new("Thời trang và làm đẹp", "Công việc hỗ trợ cửa hàng thời trang, salon và dịch vụ làm đẹp.",
            ["Tư vấn bán hàng thời trang", "Người mẫu chụp ảnh sản phẩm", "Phụ trang điểm sự kiện", "Phụ việc tại salon", "Hỗ trợ tiệm nail", "Sắp xếp trang phục chụp hình", "Hỗ trợ fitting trang phục"]),
        new("Ô tô và xe máy", "Công việc chăm sóc, vệ sinh và hỗ trợ gara theo ca hoặc theo lượt.",
            ["Rửa xe theo ca", "Đánh bóng xe", "Phụ việc tại gara", "Hỗ trợ thay lốp", "Giao nhận xe", "Hỗ trợ cứu hộ giao thông"]),
        new("Dịch vụ khách hàng và tổng đài", "Công việc tiếp nhận yêu cầu khách hàng theo ca hoặc chiến dịch.",
            ["Trực tổng đài thời vụ", "Tư vấn khách hàng qua điện thoại", "Trả lời tin nhắn trực tuyến", "Gọi xác nhận đơn hàng", "Khảo sát mức độ hài lòng", "Hỗ trợ quầy chăm sóc khách hàng"]),
        new("Thủ công và gia công", "Công việc gia công thủ công, đóng gói quà và hỗ trợ lớp trải nghiệm.",
            ["May gia công theo sản phẩm", "Lắp ráp đồ thủ công", "Làm đồ handmade", "Đóng gói quà tặng", "Phụ lớp học thủ công", "Dán nhãn sản phẩm"]),
        new("Bất động sản và nhà đất", "Công việc hỗ trợ giao dịch, sự kiện mở bán và quản lý địa điểm.",
            ["Hỗ trợ mở cửa xem nhà", "Đón khách tại sự kiện mở bán", "Chụp ảnh phòng và căn hộ", "Kiểm tra hiện trạng căn hộ", "Phát tài liệu dự án", "Hỗ trợ bàn giao mặt bằng"]),
        new("Dịch vụ cá nhân và việc vặt", "Các nhiệm vụ cá nhân có thời lượng ngắn, theo giờ hoặc theo chuyến.",
            ["Mua hộ và giao đồ", "Xếp hàng nhận dịch vụ", "Đi chợ hộ", "Hỗ trợ chuyển đồ cá nhân", "Lắp đặt thiết bị gia đình", "Hỗ trợ công việc theo danh sách"]),
        new("An ninh và bảo vệ", "Công việc bảo vệ và điều phối an toàn theo ca hoặc tại sự kiện.",
            ["Bảo vệ sự kiện", "Trực cổng theo ca", "Điều phối bãi xe", "Hướng dẫn giao thông sự kiện", "Tuần tra địa điểm", "Kiểm soát khách ra vào"]),
        new("Cưới hỏi và tiệc gia đình", "Công việc hỗ trợ lễ cưới, tiệc gia đình và nghi thức trong ngày.",
            ["Phục vụ tiệc cưới", "Đón khách tiệc cưới", "Trang trí bàn tiệc", "Hỗ trợ nghi thức lễ cưới", "Phụ quay chụp lễ cưới", "Thu dọn sau tiệc"]),
        new("Môi trường và cộng đồng", "Công việc chiến dịch ngắn về vệ sinh, cây xanh và hoạt động cộng đồng.",
            ["Thu gom rác theo chiến dịch", "Trồng cây cuối tuần", "Phân loại vật liệu tái chế", "Hỗ trợ ngày hội cộng đồng", "Khảo sát khu dân cư", "Phát tài liệu tuyên truyền"])
    ];

    public int Order => 5;

    public async Task<int> SeedAsync(AppDbContext context)
    {
        var existingCategories = await context.JobCategories.ToListAsync();
        var rootsByName = new Dictionary<string, JobCategoryEntity>(StringComparer.OrdinalIgnoreCase);
        foreach (var category in existingCategories.Where(category => category.ParentId is null))
            rootsByName.TryAdd(category.Name.Trim(), category);

        var addedRootCount = 0;
        foreach (var definition in Categories)
        {
            if (rootsByName.ContainsKey(definition.Name))
                continue;

            var category = new JobCategoryEntity
            {
                Name = definition.Name,
                Description = definition.Description,
                IsActive = true
            };
            context.JobCategories.Add(category);
            rootsByName.Add(definition.Name, category);
            addedRootCount++;
        }

        if (addedRootCount > 0)
            await context.SaveChangesAsync();

        var existingChildren = await context.JobCategories
            .AsNoTracking()
            .Where(category => category.ParentId != null)
            .Select(category => new { category.ParentId, category.Name })
            .ToListAsync();
        var childNamesByParentId = existingChildren
            .GroupBy(category => category.ParentId!.Value)
            .ToDictionary(
                group => group.Key,
                group => new HashSet<string>(
                    group.Select(category => category.Name.Trim()),
                    StringComparer.OrdinalIgnoreCase));

        var newChildren = new List<JobCategoryEntity>();
        foreach (var definition in Categories)
        {
            var parent = rootsByName[definition.Name];
            if (!childNamesByParentId.TryGetValue(parent.Id, out var existingNames))
            {
                existingNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                childNamesByParentId.Add(parent.Id, existingNames);
            }

            foreach (var childName in definition.Children)
            {
                if (!existingNames.Add(childName))
                    continue;

                newChildren.Add(new JobCategoryEntity
                {
                    Name = childName,
                    Description = $"Công việc ngắn hạn thuộc lĩnh vực {definition.Name}: {childName}.",
                    ParentId = parent.Id,
                    IsActive = true
                });
            }
        }

        if (newChildren.Count > 0)
        {
            await context.JobCategories.AddRangeAsync(newChildren);
            await context.SaveChangesAsync();
        }

        return addedRootCount + newChildren.Count;
    }

    private sealed record CategoryDefinition(string Name, string Description, string[] Children);
}
