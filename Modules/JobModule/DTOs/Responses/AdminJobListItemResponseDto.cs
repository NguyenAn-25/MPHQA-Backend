using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

public sealed record AdminJobListItemResponseDto(
    int Id,
    string Title,
    decimal SalaryValue,
    string SalaryUnit,
    PricingModel PricingModel,
    JobStatus Status,
    int ApplicantCount,
    DateTimeOffset CreatedAt,
    AdminJobCategoryResponseDto Category,
    JobAuthorResponseDto Author);
