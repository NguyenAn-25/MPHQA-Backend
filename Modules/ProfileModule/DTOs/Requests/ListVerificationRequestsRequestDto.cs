using System.ComponentModel.DataAnnotations;
using DotNetSkeleton.Modules.ProfileModule.Enums;
using DotNetSkeleton.Shared.Pagination;

namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;

public sealed class ListVerificationRequestsRequestDto : OptionalPaginationRequestDto, IValidatableObject
{
    public VerificationRequestStatus? Status { get; init; }

    public DateTimeOffset? CreatedFrom { get; init; }

    public DateTimeOffset? CreatedTo { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CreatedFrom is not null && CreatedTo is not null && CreatedFrom > CreatedTo)
        {
            yield return new ValidationResult(
                "createdFrom phải nhỏ hơn hoặc bằng createdTo.",
                [nameof(CreatedFrom), nameof(CreatedTo)]);
        }
    }
}
