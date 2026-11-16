using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;

public sealed class UpdateProfileRequestDto
{
    [Required(ErrorMessage = "Họ tên là bắt buộc.")]
    [MaxLength(100)]
    public string Fullname { get; init; } = string.Empty;

    [MaxLength(2000)]
    public string? Note { get; init; }

    [MaxLength(200)]
    public string? Major { get; init; }

    [MaxLength(30)]
    public string? Gender { get; init; }

    [MaxLength(2048)]
    public string? Avatar { get; init; }

    public DateOnly? Birthdate { get; init; }

    [MaxLength(500)]
    public string? Address { get; init; }

    [Range(typeof(decimal), "-180", "180")]
    public decimal? Longitude { get; init; }

    [Range(typeof(decimal), "-90", "90")]
    public decimal? Latitude { get; init; }

}
