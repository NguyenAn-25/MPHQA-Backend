using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;

public sealed class UpdateProfileExperiencesRequestDto
{
    [Required]
    [MaxLength(50)]
    public IReadOnlyList<UpdateProfileExperienceRequestDto>? Experiences { get; init; }
}
