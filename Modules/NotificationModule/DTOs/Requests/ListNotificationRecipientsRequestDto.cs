using System.ComponentModel.DataAnnotations;
using DotNetSkeleton.Shared.Pagination;

namespace DotNetSkeleton.Modules.NotificationModule.DTOs.Requests;

public sealed class ListNotificationRecipientsRequestDto : IValidatableObject
{
    [Range(1, int.MaxValue, ErrorMessage = "page phải lớn hơn hoặc bằng 1.")]
    public int? Page { get; init; }

    [Range(1, 100, ErrorMessage = "pageSize phải nằm trong khoảng từ 1 đến 100.")]
    public int? PageSize { get; init; }

    public PaginationOptions GetPaginationOptions() => new(
        PageSize.HasValue,
        Page ?? 1,
        PageSize ?? 1);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Page.HasValue && !PageSize.HasValue)
        {
            yield return new ValidationResult(
                "pageSize là bắt buộc khi truyền page.",
                [nameof(PageSize)]);
        }
    }
}
