namespace DotNetSkeleton.Modules.JobModule.DTOs.Requests;

public sealed record CompleteJobRequestDto(IReadOnlyList<int> FinishedApplicationIds);
