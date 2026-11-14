using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Requests;

public sealed class UpdateJobCategoryStatusRequestDto
{
    [Required(ErrorMessage = "Trạng thái lĩnh vực là bắt buộc!")]
    public bool? IsActive { get; init; }
}
