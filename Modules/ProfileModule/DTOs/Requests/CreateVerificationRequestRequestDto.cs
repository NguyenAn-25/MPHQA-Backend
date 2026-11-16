using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;

public sealed class CreateVerificationRequestRequestDto
{
    [Required]
    [MaxLength(150)]
    public string FullNameOnCard { get; init; } = string.Empty;

    [Required]
    [MaxLength(64)]
    public string IdNumber { get; init; } = string.Empty;

    [Required]
    [MaxLength(2048)]
    public string CardFrontImage { get; init; } = string.Empty;

    [Required]
    [MaxLength(2048)]
    public string CardBackImage { get; init; } = string.Empty;

    [Required]
    [MaxLength(2048)]
    public string PortraitImage { get; init; } = string.Empty;
}
