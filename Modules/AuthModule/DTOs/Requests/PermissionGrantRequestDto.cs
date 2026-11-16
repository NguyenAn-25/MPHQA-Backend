using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.AuthModule.DTOs.Requests;

public sealed class PermissionGrantRequestDto
{
    [Range(1, int.MaxValue)]
    public int FunctionId { get; init; }

    [Required]
    [MaxLength(64)]
    public string Action { get; init; } = string.Empty;
}
