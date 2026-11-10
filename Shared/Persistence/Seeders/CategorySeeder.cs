using DotNetSkeleton.Modules.JobModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Shared.Persistence.Seeders;

public sealed class CategorySeeder : ISeeder
{
    private static readonly (string Name, string Description, string Icon, string[] Children)[] Categories =
    [
        ("Marketing & tăng trưởng", "Các dịch vụ tiếp thị, nội dung và phát triển thương hiệu.", "megaphone", ["Viết content", "Copywriting", "Forum seeding", "Email marketing", "Tư vấn marketing", "Tiếp thị liên kết", "SEO", "Quản lý fanpage", "Quảng cáo Facebook"]),
        ("Media & sản xuất", "Các dịch vụ quay phim, chụp ảnh, thu âm và hậu kỳ nội dung.", "camera", ["Quay video sự kiện", "Chụp ảnh cá nhân", "Chụp ảnh sự kiện", "Chụp ảnh sản phẩm", "Chụp ảnh thời trang", "Quay video quảng cáo", "Lồng tiếng", "Review sản phẩm", "Review dịch vụ", "Quay video ngắn", "Thu âm podcast", "Chỉnh sửa ảnh", "TVC", "Chỉnh sửa video"]),
        ("Thiết kế & sáng tạo", "Các dịch vụ thiết kế đồ họa, sản phẩm, không gian và nội dung sáng tạo.", "palette", ["Motion graphics", "Vẽ tay", "Thiết kế ngoại thất", "UI/UX", "CAD/CAM", "Thiết kế nội thất", "Thiết kế logo", "Thiết kế brochure", "Digital painting", "Thiết kế kiến trúc", "Animation", "Banner quảng cáo", "Nhãn hiệu và bao bì", "Thiết kế infographic"]),
        ("Kỹ thuật & Sửa chữa", "Các dịch vụ xây dựng, lắp đặt, bảo trì và sửa chữa thiết bị.", "wrench", ["Phụ xây", "Sửa khóa", "Thợ cơ khí", "Thợ sơn", "Sửa ô tô", "Sửa xe máy", "Sửa điện thoại", "Thợ mộc", "Thợ xây dựng", "Sửa điện nước", "Sửa điện lạnh", "Sửa máy tính"]),
        ("Công nghệ & IT", "Các dịch vụ phát triển phần mềm, dữ liệu, hạ tầng và công nghệ thông tin.", "laptop-code", ["Quản trị hệ thống", "DevOps", "Data Labeling", "Machine Learning", "Phân tích dữ liệu", "AI", "QA Tester", "Hệ thống mạng", "Bảo mật / an ninh mạng", "Ứng dụng di động", "Lập trình Web", "Lập trình phần mềm"]),
        ("Văn phòng, tư vấn & đào tạo", "Các dịch vụ văn phòng, ngôn ngữ, tư vấn chuyên môn và đào tạo.", "briefcase", ["Tiếng Nhật", "Tiếng Đức", "Tiếng Ý", "Tư vấn thuế", "Gia sư", "Kế toán", "Nhập dữ liệu", "Quản lý CRM", "Quản lý dự án", "Tư vấn tài chính", "Tiếng Anh", "Hỗ trợ pháp lý", "Tiếng Hàn", "Tiếng Pháp", "Tiếng Thái", "Tiếng Tây Ban Nha", "Đào tạo kỹ năng mềm", "Kiểm toán", "Trợ lý ảo", "Chứng khoán", "Tư vấn pháp lý", "Tiếng Trung"]),
        ("Dịch vụ ăn uống", "Các dịch vụ chế biến, phục vụ và vận hành nhà hàng, quán ăn.", "utensils", ["Phục vụ nhà hàng", "Phục vụ tiệc lẻ", "Tạp vụ quán ăn", "Tạp vụ nhà hàng", "Phục vụ quán cafe", "Lễ tân nhà hàng", "Pha chế", "Đầu bếp", "Phụ bếp", "Nấu ăn tại nhà", "Nấu ăn văn phòng"]),
        ("Bán hàng", "Các công việc bán hàng, tư vấn sản phẩm và chăm sóc khách hàng.", "store", ["Telesales", "Bán hàng trực tiếp", "Nhân viên trưng bày", "Thu ngân", "Trực quầy / Tư vấn", "Livestream", "Khảo sát thị trường", "Cộng tác viên bán hàng", "Kiểm kê hàng hoá", "Sampling", "Chăm sóc khách hàng", "Bán hàng online"]),
        ("Sự kiện & quảng bá", "Các dịch vụ nhân sự, biểu diễn và quảng bá tại sự kiện.", "calendar-star", ["Phát tờ rơi", "Mẫu Ảnh", "Hỗ trợ sự kiện", "PG/PB", "Lễ tân sự kiện", "Dẫn chương trình", "Chạy roadshow", "Setup sân khấu", "Mascot", "Hoá trang nhân vật"]),
        ("Kho vận & vận hành", "Các công việc kho bãi, giao nhận, vận chuyển và vận hành.", "boxes", ["Sơ chế thực phẩm", "Logistic", "Nhân viên kho", "Soạn hàng", "Đóng gói", "Phân loại hàng hoá", "Dán tem", "Bốc xếp", "Tài xế", "Bảo vệ", "Giữ xe"]),
        ("Dịch vụ cá nhân", "Các dịch vụ hỗ trợ đời sống, chăm sóc cá nhân, gia đình và thú cưng.", "user-heart", ["Giúp việc nhà", "Chăm thú cưng", "Massage", "Việc nhỏ", "Trợ lý cá nhân", "Thể thao cùng nhau", "Tâm sự", "Nail", "Makeup", "Spa", "Hướng dẫn viên", "Xem tarot", "Thần số học", "Bản đồ sao", "Điều Dưỡng", "Chăm sóc cây cảnh", "Handmade", "Chăm sóc người già", "Trông trẻ", "Coaching", "Cắt tóc", "Game", "Giặt ủi", "Thuê bạn đồng hành", "Nhận nuôi thú cưng"]),
        ("Khách sạn & lưu trú", "Các dịch vụ phục vụ khách sạn, cơ sở lưu trú và phòng nghỉ.", "hotel", ["Tất cả Khách sạn & lưu trú", "Phục vụ khách sạn", "Nhân viên buồng phòng", "Lễ tân khách sạn", "Bellman", "Tạp vụ khách sạn"])
    ];

    public int Order => 2;

    public async Task<int> SeedAsync(AppDbContext context)
    {
        var existing = await context.JobCategories
            .AsTracking()
            .ToListAsync();
        var rootsByName = existing
            .Where(category => category.ParentId is null)
            .ToDictionary(category => category.Name, StringComparer.Ordinal);
        var added = 0;

        foreach (var definition in Categories)
        {
            if (rootsByName.TryGetValue(definition.Name, out var existingRoot))
            {
                FillMissingMetadata(existingRoot, definition.Description, definition.Icon);
                continue;
            }

            var root = new JobCategoryEntity
            {
                Name = definition.Name,
                IsActive = true,
                Description = definition.Description,
                Icon = definition.Icon
            };
            context.JobCategories.Add(root);
            rootsByName.Add(definition.Name, root);
            added++;
        }

        // Save roots first so newly created parent IDs are available for child rows.
        await context.SaveChangesAsync();

        var childrenByParentAndName = existing
            .Where(category => category.ParentId is not null)
            .ToDictionary(category => (category.ParentId!.Value, category.Name));

        foreach (var definition in Categories)
        {
            var parent = rootsByName[definition.Name];
            foreach (var childName in definition.Children)
            {
                if (childrenByParentAndName.TryGetValue((parent.Id, childName), out var existingChild))
                {
                    FillMissingMetadata(
                        existingChild,
                        $"Dịch vụ {childName} thuộc nhóm {definition.Name}.",
                        definition.Icon);
                    continue;
                }

                context.JobCategories.Add(new JobCategoryEntity
                {
                    Name = childName,
                    IsActive = true,
                    Description = $"Dịch vụ {childName} thuộc nhóm {definition.Name}.",
                    Icon = definition.Icon,
                    ParentId = parent.Id
                });
                added++;
            }
        }

        if (added > 0 || existing.Any(category =>
                string.IsNullOrWhiteSpace(category.Description) ||
                string.IsNullOrWhiteSpace(category.Icon)))
            await context.SaveChangesAsync();

        return added;
    }

    private static void FillMissingMetadata(
        JobCategoryEntity category,
        string description,
        string icon)
    {
        if (string.IsNullOrWhiteSpace(category.Description))
            category.Description = description;

        if (string.IsNullOrWhiteSpace(category.Icon))
            category.Icon = icon;
    }
}
