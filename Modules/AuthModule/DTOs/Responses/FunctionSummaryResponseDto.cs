namespace DotNetSkeleton.Modules.AuthModule.DTOs.Responses;

public sealed record FunctionSummaryResponseDto(
    int Id,
    string Code,
    string NameVi,
    string NameEn,
    IReadOnlyList<string> Actions);
