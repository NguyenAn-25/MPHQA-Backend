using System.ComponentModel.DataAnnotations;
using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Requests;

public sealed class AdminUpdateJobModerationStatusRequestDto : IValidatableObject
{
    [Required(ErrorMessage = "Trạng thái công việc là bắt buộc.")]
    public JobStatus? Status { get; init; }

    [MaxLength(1000, ErrorMessage = "rejectionReason không được vượt quá 1000 ký tự.")]
    public string? RejectionReason { get; init; }

    [MaxLength(1000, ErrorMessage = "cancelReason không được vượt quá 1000 ký tự.")]
    public string? CancelReason { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Status is { } status && status is not JobStatus.Accepted and not JobStatus.Rejected and not JobStatus.Cancelled)
        {
            yield return new ValidationResult(
                "Trạng thái xử lý chỉ được là ACCEPTED, REJECTED hoặc CANCELLED.",
                [nameof(Status)]);
        }

        if (Status == JobStatus.Cancelled && string.IsNullOrWhiteSpace(CancelReason))
        {
            yield return new ValidationResult(
                "cancelReason là bắt buộc khi chuyển trạng thái sang CANCELLED.",
                [nameof(CancelReason)]);
        }

        if (Status != JobStatus.Cancelled && !string.IsNullOrWhiteSpace(CancelReason))
        {
            yield return new ValidationResult(
                "cancelReason chỉ được truyền khi trạng thái là CANCELLED.",
                [nameof(CancelReason)]);
        }

        if (Status != JobStatus.Rejected && !string.IsNullOrWhiteSpace(RejectionReason))
        {
            yield return new ValidationResult(
                "rejectionReason chỉ được truyền khi trạng thái là REJECTED.",
                [nameof(RejectionReason)]);
        }
    }
}
