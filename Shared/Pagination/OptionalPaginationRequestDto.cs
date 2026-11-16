using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Shared.Pagination;

public abstract class OptionalPaginationRequestDto
{
    [Range(1, int.MaxValue)]
    public int? Page { get; init; }

    [Range(1, 100)]
    public int? PageSize { get; init; }

    protected virtual int DefaultPageSize => 10;

    public PaginationOptions GetPaginationOptions() => new(
        Page.HasValue || PageSize.HasValue,
        Page ?? 1,
        PageSize ?? DefaultPageSize);
}

public readonly record struct PaginationOptions(bool IsEnabled, int Page, int PageSize);
