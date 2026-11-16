using System.ComponentModel.DataAnnotations;
using DotNetSkeleton.Modules.AuthModule.Enums;
using DotNetSkeleton.Shared.Pagination;

namespace DotNetSkeleton.Modules.AuthModule.DTOs.Requests;

public sealed class ListUsersRequestDto : OptionalPaginationRequestDto, IValidatableObject
{
    protected override int DefaultPageSize => 20;

    [MaxLength(20)]
    public string? Search { get; init; }

    public UserRole? Role { get; init; }

    public bool? IsActive { get; init; }

    public bool? IsVerified { get; init; }

    [Range(1, int.MaxValue)]
    public int? PermissionId { get; init; }

    public DateTimeOffset? CreatedFrom { get; init; }

    public DateTimeOffset? CreatedTo { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CreatedFrom is not null && CreatedTo is not null && CreatedFrom > CreatedTo)
        {
            yield return new ValidationResult(
                "createdFrom phải nhỏ hơn hoặc bằng createdTo.",
                [nameof(CreatedFrom), nameof(CreatedTo)]);
        }
    }
}
