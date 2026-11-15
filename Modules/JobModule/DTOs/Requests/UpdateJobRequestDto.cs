using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Requests;

/// <summary>
/// Payload for updating a pending job owned by the current user.
/// </summary>
public sealed record UpdateJobRequestDto
{
    /// <summary>Payment offered to each worker.</summary>
    [Range(typeof(decimal), "0.01", "999999999999.99")]
    public decimal? SalaryValue { get; init; }

    /// <summary>Optional salary unit.</summary>
    [MaxLength(50)]
    public string? SalaryUnit { get; init; }

    /// <summary>Job title.</summary>
    [MaxLength(200)]
    public string? Title { get; init; }

    /// <summary>Detailed job description.</summary>
    public string? Description { get; init; }

    /// <summary>Location where the job is performed.</summary>
    [MaxLength(500)]
    public string? Address { get; init; }

    /// <summary>Optional longitude of the job location.</summary>
    [Range(typeof(decimal), "-180", "180")]
    public decimal? Longitude { get; init; }

    /// <summary>Optional latitude of the job location.</summary>
    [Range(typeof(decimal), "-90", "90")]
    public decimal? Latitude { get; init; }

    /// <summary>Number of workers required for this job.</summary>
    [Range(1, int.MaxValue)]
    public int? RequiredNumberOfWorkers { get; init; }

    /// <summary>Job start time.</summary>
    public DateTimeOffset? TimeStartAt { get; init; }

    /// <summary>Required qualifications or instructions for applicants.</summary>
    public string? RequiredDescription { get; init; }

    /// <summary>Automatic reply sent to applicants.</summary>
    public string? AutoReplyMessage { get; init; }

    /// <summary>Existing job category identifier.</summary>
    [Range(1, int.MaxValue)]
    public int? JobCategoryId { get; init; }

    /// <summary>Image URLs for the job, from one to four images.</summary>
    [MinLength(1), MaxLength(4)]
    public IReadOnlyList<string>? Images { get; init; }
}
