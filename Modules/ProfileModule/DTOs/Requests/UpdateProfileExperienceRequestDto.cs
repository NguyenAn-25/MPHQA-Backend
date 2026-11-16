using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;

public sealed class UpdateProfileExperienceRequestDto
{
    [Range(1, int.MaxValue)]
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [Required]
    [MaxLength(120)]
    public string JobTitle { get; init; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string CompanyName { get; init; } = string.Empty;

    [MaxLength(4000)]
    public string? Description { get; init; }

    [MaxLength(120)]
    public string? Title { get; init; }

    [Range(1, int.MaxValue)]
    public int? JobCategoryId { get; init; }
}
