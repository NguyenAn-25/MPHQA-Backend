using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

public sealed record JobListItemResponseDto(
    int Id,
    string Title,
    decimal SalaryValue,
    string SalaryUnit,
    JobPricingModel PricingModel,
    JobStatus Status,
    int ApplicantCount,
    DateTimeOffset CreatedAt,
    JobCategoryResponseDto Category,
    JobAuthorResponseDto Author);
