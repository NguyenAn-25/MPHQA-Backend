using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Requests;

public sealed class ListJobCategoriesRequestDto
{
    [Range(1, int.MaxValue)]
    public int Page { get; init; } = 1;

    [Range(1, 100)]
    public int PageSize { get; init; } = 10;

    [MaxLength(100)]
    public string? Search { get; init; }

    public bool? IsActive { get; init; }

    [Range(1, int.MaxValue)]
    public int? ParentId { get; init; }
}
