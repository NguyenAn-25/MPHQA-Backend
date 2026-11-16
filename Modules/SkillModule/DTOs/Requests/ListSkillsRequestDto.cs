using System.ComponentModel.DataAnnotations;
using DotNetSkeleton.Shared.Pagination;

namespace DotNetSkeleton.Modules.SkillModule.DTOs.Requests;

public sealed class ListSkillsRequestDto : OptionalPaginationRequestDto
{
    [MaxLength(100)]
    public string? Search { get; init; }

    public bool? IsActive { get; init; }
}
