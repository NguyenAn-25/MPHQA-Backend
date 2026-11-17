using System.ComponentModel.DataAnnotations;
using DotNetSkeleton.Modules.ReportModule.Enums;
using DotNetSkeleton.Shared.Pagination;

namespace DotNetSkeleton.Modules.ReportModule.DTOs.Requests;

public sealed class ListReportsRequestDto : IValidatableObject
{
    [Range(1, int.MaxValue, ErrorMessage = "page phải lớn hơn hoặc bằng 1.")]
    public int? Page { get; init; }

    [Range(1, 100, ErrorMessage = "pageSize phải nằm trong khoảng từ 1 đến 100.")]
    public int? PageSize { get; init; }

    public ReportObjectType? ObjectType { get; init; }

    public ReportStatus? Status { get; init; }

    public DateTimeOffset? CreatedFrom { get; init; }

    public DateTimeOffset? CreatedTo { get; init; }

    public PaginationOptions GetPaginationOptions() => new(
        Page.HasValue || PageSize.HasValue,
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

        if (CreatedFrom is not null && CreatedTo is not null && CreatedFrom > CreatedTo)
        {
            yield return new ValidationResult(
                "createdFrom phải nhỏ hơn hoặc bằng createdTo.",
                [nameof(CreatedFrom), nameof(CreatedTo)]);
        }
    }
}
