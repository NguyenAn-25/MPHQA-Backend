namespace DotNetSkeleton.Modules.SkillModule.DTOs.Responses;

public sealed record PagedSkillsResponseDto(
    IReadOnlyList<SkillSummaryResponseDto> Items,
    int Page,
    int PageSize,
    int TotalCount);
