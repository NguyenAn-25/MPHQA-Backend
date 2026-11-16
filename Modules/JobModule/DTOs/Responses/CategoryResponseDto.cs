namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

/// <summary>
/// Represents an active parent job category and its active child categories.
/// </summary>
public sealed record CategoryResponseDto(
    int Id,
    string Name,
    string? Description,
    string? Icon,
    IReadOnlyList<CategoryChildResponseDto> Children);

/// <summary>
/// Represents an active child job category.
/// </summary>
public sealed record CategoryChildResponseDto(
    int Id,
    string Name,
    string? Description,
    string? Icon);
