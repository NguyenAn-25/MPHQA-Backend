using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace DotNetSkeleton.Shared.Validation;

public sealed class VietnameseValidationMetadataProvider : IValidationMetadataProvider
{
    private static readonly IReadOnlyDictionary<string, string> FieldNames = new Dictionary<string, string>
    {
        ["Action"] = "Thao tác",
        ["Address"] = "Địa chỉ",
        ["Avatar"] = "Ảnh đại diện",
        ["Birthdate"] = "Ngày sinh",
        ["CardBackImage"] = "Ảnh mặt sau giấy tờ",
        ["CardFrontImage"] = "Ảnh mặt trước giấy tờ",
        ["CompanyName"] = "Tên công ty",
        ["Content"] = "Nội dung",
        ["CreatedFrom"] = "Ngày bắt đầu",
        ["CreatedTo"] = "Ngày kết thúc",
        ["Description"] = "Mô tả",
        ["Details"] = "Danh sách quyền chi tiết",
        ["Experiences"] = "Danh sách kinh nghiệm",
        ["FullNameOnCard"] = "Họ tên trên giấy tờ",
        ["Fullname"] = "Họ tên",
        ["FunctionId"] = "Chức năng",
        ["Gender"] = "Giới tính",
        ["Id"] = "Mã",
        ["IdNumber"] = "Số giấy tờ",
        ["IsActive"] = "Trạng thái",
        ["IsVerified"] = "Trạng thái xác thực",
        ["JobCategoryId"] = "Lĩnh vực",
        ["JobTitle"] = "Chức danh công việc",
        ["Latitude"] = "Vĩ độ",
        ["Longitude"] = "Kinh độ",
        ["Major"] = "Chuyên ngành",
        ["Name"] = "Tên",
        ["NewPassword"] = "Mật khẩu mới",
        ["Note"] = "Ghi chú",
        ["Page"] = "Trang",
        ["PageSize"] = "Số lượng mỗi trang",
        ["ParentId"] = "Danh mục cha",
        ["Password"] = "Mật khẩu",
        ["PermissionId"] = "Nhóm quyền",
        ["Phone"] = "Số điện thoại",
        ["PortraitImage"] = "Ảnh chân dung",
        ["PublicId"] = "Mã publicId",
        ["RefreshToken"] = "Refresh token",
        ["RejectionReason"] = "Lý do từ chối",
        ["Role"] = "Vai trò",
        ["Search"] = "Từ khóa tìm kiếm",
        ["SendTo"] = "Đối tượng nhận",
        ["SkillIds"] = "Danh sách kỹ năng",
        ["Status"] = "Trạng thái",
        ["Title"] = "Tiêu đề",
        ["UserId"] = "Người dùng"
    };

    public void CreateValidationMetadata(ValidationMetadataProviderContext context)
    {
        var fieldName = context.Attributes
            .OfType<DisplayAttribute>()
            .Select(attribute => attribute.GetName())
            .FirstOrDefault(name => !string.IsNullOrWhiteSpace(name));
        if (string.IsNullOrWhiteSpace(fieldName))
        {
            var propertyName = context.Key.Name;
            fieldName = propertyName is not null && FieldNames.TryGetValue(propertyName, out var translatedName)
                ? translatedName
                : "Thông tin";
        }

        foreach (var attribute in context.ValidationMetadata.ValidatorMetadata.OfType<ValidationAttribute>())
        {
            if (!string.IsNullOrWhiteSpace(attribute.ErrorMessage) ||
                attribute.ErrorMessageResourceName is not null ||
                attribute.ErrorMessageResourceType is not null)
            {
                continue;
            }

            attribute.ErrorMessage = GetErrorMessage(attribute, fieldName);
        }
    }

    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
    }

    private static string GetErrorMessage(ValidationAttribute attribute, string fieldName) => attribute switch
    {
        RequiredAttribute => $"{fieldName} là bắt buộc.",
        RangeAttribute range => $"{fieldName} phải nằm trong khoảng {Format(range.Minimum)} đến {Format(range.Maximum)}.",
        MaxLengthAttribute maxLength => $"{fieldName} không được vượt quá {maxLength.Length} ký tự.",
        MinLengthAttribute minLength => $"{fieldName} phải có ít nhất {minLength.Length} ký tự.",
        StringLengthAttribute stringLength when stringLength.MinimumLength > 0 =>
            $"{fieldName} phải có từ {stringLength.MinimumLength} đến {stringLength.MaximumLength} ký tự.",
        StringLengthAttribute stringLength =>
            $"{fieldName} không được vượt quá {stringLength.MaximumLength} ký tự.",
        RegularExpressionAttribute => $"{fieldName} có định dạng không hợp lệ.",
        EmailAddressAttribute => $"{fieldName} không đúng định dạng email.",
        PhoneAttribute => $"{fieldName} không đúng định dạng số điện thoại.",
        UrlAttribute => $"{fieldName} không đúng định dạng URL.",
        CompareAttribute => $"Giá trị của {fieldName} không khớp.",
        _ => $"{fieldName} không hợp lệ."
    };

    private static string Format(object value) => Convert.ToString(value, CultureInfo.CurrentCulture) ?? string.Empty;
}
