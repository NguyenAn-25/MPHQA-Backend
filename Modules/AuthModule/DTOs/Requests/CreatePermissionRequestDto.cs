using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.AuthModule.DTOs.Requests;

public sealed class CreatePermissionRequestDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; init; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; init; }

    [Required]
    public IReadOnlyList<PermissionGrantRequestDto>? Details { get; init; }
}
