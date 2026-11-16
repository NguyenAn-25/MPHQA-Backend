using System.ComponentModel.DataAnnotations;
using DotNetSkeleton.Shared.Pagination;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Requests;

public sealed class ListJobCategoriesRequestDto : OptionalPaginationRequestDto
{
    [MaxLength(100)]
    public string? Search { get; init; }

    public bool? IsActive { get; init; }

    [Range(1, int.MaxValue)]
    public int? ParentId { get; init; }
}
