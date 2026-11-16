using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.SkillModule.DTOs.Requests;

public sealed class UpdateSkillStatusRequestDto
{
    [Required(ErrorMessage = "Trạng thái kỹ năng là bắt buộc!")]
    public bool? IsActive { get; init; }
}
