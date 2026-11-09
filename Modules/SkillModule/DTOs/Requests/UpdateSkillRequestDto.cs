using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.SkillModule.DTOs.Requests;

public sealed class UpdateSkillRequestDto
{
    [Required(ErrorMessage = "Tên kỹ năng là bắt buộc!")]
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public string? Icon { get; init; }
}
