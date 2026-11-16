using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Requests;

/// <summary>
/// Payload for creating a post package.
/// </summary>
public sealed class CreatePostPackageRequestDto
{
    [Required(ErrorMessage = "Tên gói đăng tuyển là bắt buộc.")]
    [StringLength(200, ErrorMessage = "Tên gói đăng tuyển không được vượt quá 200 ký tự.")]
    public string Name { get; init; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Thời hạn gói phải là số nguyên dương.")]
    public int DurationDays { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "Giá coin phải là số nguyên dương.")]
    public int PriceCoin { get; init; }

    [Range(typeof(decimal), "0.01", "9999999999999.99", ErrorMessage = "Giá tiền phải lớn hơn 0.")]
    public decimal PriceMoney { get; init; }
}
