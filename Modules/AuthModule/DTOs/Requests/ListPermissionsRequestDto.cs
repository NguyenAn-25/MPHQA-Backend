using System.ComponentModel.DataAnnotations;
using DotNetSkeleton.Shared.Pagination;

namespace DotNetSkeleton.Modules.AuthModule.DTOs.Requests;

public sealed class ListPermissionsRequestDto : OptionalPaginationRequestDto
{
    protected override int DefaultPageSize => 20;

    [MaxLength(100)]
    public string? Search { get; init; }

    public bool? IsActive { get; init; }
}
