namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

public sealed record VerificationRequestProfileComparisonResponseDto(
    string Fullname,
    DateOnly? Birthdate,
    string? Address,
    string? Avatar,
    VerificationRequestIdentityComparisonResponseDto? ExistingIdentity);
