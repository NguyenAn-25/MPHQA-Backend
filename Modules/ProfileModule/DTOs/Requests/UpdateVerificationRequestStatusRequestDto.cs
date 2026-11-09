using System.ComponentModel.DataAnnotations;
using DotNetSkeleton.Modules.ProfileModule.Entities;

namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;

public sealed class UpdateVerificationRequestStatusRequestDto : IValidatableObject
{
    [Required]
    [RegularExpression("^(ACCEPTED|REJECTED)$", ErrorMessage = "Trạng thái chỉ được là ACCEPTED hoặc REJECTED.")]
    public string Status { get; init; } = string.Empty;

    [MaxLength(1000)]
    public string? RejectionReason { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Status == VerificationRequestStatuses.Rejected && string.IsNullOrWhiteSpace(RejectionReason))
        {
            yield return new ValidationResult(
                "Lý do từ chối là bắt buộc khi REJECTED.",
                [nameof(RejectionReason)]);
        }
    }
}
