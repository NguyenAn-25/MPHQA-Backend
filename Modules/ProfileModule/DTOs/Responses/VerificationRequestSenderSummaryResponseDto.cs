namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

public sealed record VerificationRequestSenderSummaryResponseDto(
    int Id,
    Guid PublicId,
    string Phone);
