using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;

public sealed class ListVerificationRequestsRequestDto : IValidatableObject
{
    [Range(1, int.MaxValue)]
    public int Page { get; init; } = 1;

    [Range(1, 100)]
    public int PageSize { get; init; } = 10;

    [RegularExpression("^(PENDING|ACCEPTED|REJECTED|CANCELLED)$")]
    public string? Status { get; init; }

    public DateTimeOffset? SubmittedFrom { get; init; }

    public DateTimeOffset? SubmittedTo { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SubmittedFrom is not null && SubmittedTo is not null && SubmittedFrom > SubmittedTo)
        {
            yield return new ValidationResult(
                "submittedFrom phải nhỏ hơn hoặc bằng submittedTo.",
                [nameof(SubmittedFrom), nameof(SubmittedTo)]);
        }
    }
}
