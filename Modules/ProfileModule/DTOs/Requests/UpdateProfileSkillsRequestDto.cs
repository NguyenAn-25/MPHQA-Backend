using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;

public sealed class UpdateProfileSkillsRequestDto
{
    [Required]
    [MaxLength(100)]
    public IReadOnlyList<int>? SkillIds { get; init; }
}
