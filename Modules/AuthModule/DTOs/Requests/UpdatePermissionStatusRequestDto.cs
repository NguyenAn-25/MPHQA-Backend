using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.AuthModule.DTOs.Requests;

public sealed class UpdatePermissionStatusRequestDto
{
    [Required]
    public bool? IsActive { get; init; }
}
