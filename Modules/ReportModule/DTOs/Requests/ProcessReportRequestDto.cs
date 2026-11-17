using System.ComponentModel.DataAnnotations;
using DotNetSkeleton.Modules.ReportModule.Enums;

namespace DotNetSkeleton.Modules.ReportModule.DTOs.Requests;

public sealed class ProcessReportRequestDto : IValidatableObject
{
    [Required(ErrorMessage = "Trạng thái xử lý là bắt buộc.")]
    public ReportStatus? Status { get; init; }

    [MaxLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự.")]
    public string? Note { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Status is { } status && status is not ReportStatus.Resolved and not ReportStatus.Rejected)
        {
            yield return new ValidationResult(
                "Trạng thái xử lý chỉ được là RESOLVED hoặc REJECTED.",
                [nameof(Status)]);
        }
    }
}
