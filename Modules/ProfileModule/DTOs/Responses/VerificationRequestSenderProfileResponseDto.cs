namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

public sealed record VerificationRequestSenderProfileResponseDto(
    int Id,
    string Fullname,
    string? Gender,
    DateOnly? Birthdate,
    string? Avatar);
