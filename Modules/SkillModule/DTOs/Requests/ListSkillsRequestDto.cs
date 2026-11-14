using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.SkillModule.DTOs.Requests;

public sealed class ListSkillsRequestDto
{
    [Range(1, int.MaxValue)]
    public int Page { get; init; } = 1;

    [Range(1, 100)]
    public int PageSize { get; init; } = 10;

    [MaxLength(100)]
    public string? Search { get; init; }

    public bool? IsActive { get; init; }
}
