namespace DotNetSkeleton.Modules.JobModule.DTOs.Requests;

public sealed record AcceptApplicationsRequestDto(IReadOnlyList<int> ApplicationIds);
