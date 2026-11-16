using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Requests;

/// <summary>
/// Payload for posting a new job paid by time.
/// </summary>
public sealed record CreateTimeBasedJobRequestDto
{
    /// <summary>Payment offered to each worker for one salary unit.</summary>
    [Range(typeof(decimal), "0.01", "999999999999.99")]
    public decimal SalaryValue { get; init; }

    /// <summary>Salary unit selected by the user, such as hour, day, or month.</summary>
    [Required, MaxLength(50)]
    public required string SalaryUnit { get; init; }

    /// <summary>Job title.</summary>
    [Required, MaxLength(200)]
    public required string Title { get; init; }

    /// <summary>Detailed job description.</summary>
    [Required]
    public required string Description { get; init; }

    /// <summary>Location where the job is performed.</summary>
    [Required, MaxLength(500)]
    public required string Address { get; init; }

    /// <summary>Optional longitude of the job location.</summary>
    [Range(typeof(decimal), "-180", "180")]
    public decimal? Longitude { get; init; }

    /// <summary>Optional latitude of the job location.</summary>
    [Range(typeof(decimal), "-90", "90")]
    public decimal? Latitude { get; init; }

    /// <summary>Number of workers required for this job.</summary>
    [Range(1, int.MaxValue)]
    public int RequiredNumberOfWorkers { get; init; }

    /// <summary>Job start time.</summary>
    public DateTimeOffset TimeStartAt { get; init; }

    /// <summary>Required qualifications or instructions for applicants.</summary>
    public string? RequiredDescription { get; init; }

    /// <summary>Automatic reply sent to applicants.</summary>
    public string? AutoReplyMessage { get; init; }

    /// <summary>Existing job category identifier.</summary>
    [Range(1, int.MaxValue)]
    public int JobCategoryId { get; init; }

    /// <summary>Post package used to promote the job.</summary>
    [Range(1, int.MaxValue)]
    public int PostPackageId { get; init; }

    /// <summary>Image URLs for the job, from one to four images.</summary>
    [Required, MinLength(1), MaxLength(4)]
    public required IReadOnlyList<string> Images { get; init; }
}
