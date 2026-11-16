using System.ComponentModel.DataAnnotations;
using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Requests;

/// <summary>
/// Filters and cursor options for a job list.
/// </summary>
public sealed record ListJobsRequestDto
{
    /// <summary>Opaque cursor returned by the previous request.</summary>
    public string? Cursor { get; init; }

    /// <summary>Maximum number of jobs to return.</summary>
    [Range(1, 100)]
    public int PageSize { get; init; } = 20;

    /// <summary>Filters jobs by their current status.</summary>
    public JobStatus? Status { get; init; }

    /// <summary>Job category identifier.</summary>
    [Range(1, int.MaxValue)]
    public int? CategoryId { get; init; }

    /// <summary>Case-insensitive text matched against the job address.</summary>
    [MaxLength(200)]
    public string? Location { get; init; }

    /// <summary>Filters jobs by the poster's account verification state.</summary>
    public bool? IsVerified { get; init; }

    /// <summary>Minimum average feedback rating.</summary>
    [Range(0, 5)]
    public decimal? RatingMin { get; init; }

    /// <summary>Maximum salary value.</summary>
    [Range(0, double.MaxValue)]
    public decimal? SalaryMax { get; init; }

    /// <summary>Minimum salary value.</summary>
    [Range(0, double.MaxValue)]
    public decimal? SalaryMin { get; init; }
}
