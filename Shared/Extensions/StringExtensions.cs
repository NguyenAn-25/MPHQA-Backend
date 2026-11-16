namespace DotNetSkeleton.Shared.Extensions;

public static class StringExtensions
{
    public static string? NormalizeOptionalText(this string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
