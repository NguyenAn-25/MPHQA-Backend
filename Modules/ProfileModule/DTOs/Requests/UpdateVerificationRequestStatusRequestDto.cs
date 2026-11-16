using System.ComponentModel.DataAnnotations;
using DotNetSkeleton.Modules.ProfileModule.Enums;

namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;

public sealed class UpdateVerificationRequestStatusRequestDto : IValidatableObject
{
    [Required]
    public VerificationRequestStatus? Status { get; init; }

    [MaxLength(1000)]
    public string? RejectionReason { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Status is { } status && status is not VerificationRequestStatus.Accepted and not VerificationRequestStatus.Rejected)
        {
            yield return new ValidationResult(
                "Trạng thái chỉ được là ACCEPTED hoặc REJECTED.",
                [nameof(Status)]);
        }

        if (Status == VerificationRequestStatus.Rejected && string.IsNullOrWhiteSpace(RejectionReason))
        {
            yield return new ValidationResult(
                "Lý do từ chối là bắt buộc khi REJECTED.",
                [nameof(RejectionReason)]);
        }
    }
}
