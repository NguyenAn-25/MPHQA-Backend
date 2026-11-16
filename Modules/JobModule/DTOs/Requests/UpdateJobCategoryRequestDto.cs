using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Requests;

public sealed class UpdateJobCategoryRequestDto
{
    [Required(ErrorMessage = "Tên lĩnh vực là bắt buộc!")]
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public string? Icon { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "Danh mục cha phải là số nguyên dương bắt đầu từ 1!")]
    public int? ParentId { get; init; }
}
