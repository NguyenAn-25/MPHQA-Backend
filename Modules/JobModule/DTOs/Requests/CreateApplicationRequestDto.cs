using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Requests;

/// <summary>
/// Data submitted when a customer applies for a job.
/// </summary>
public sealed record CreateApplicationRequestDto
{
    /// <summary>
    /// Optional note sent with the application.
    /// </summary>
    [MaxLength(2000)]
    public string? Note { get; init; }
}
