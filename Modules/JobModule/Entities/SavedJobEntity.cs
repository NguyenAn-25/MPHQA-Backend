using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.JobModule.Entities;

[Table("saved_jobs")]
[PrimaryKey(nameof(UserId), nameof(JobId))]
public sealed class SavedJobEntity
{
    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity User { get; set; } = null!;

    [Column("job_id")]
    public int JobId { get; set; }

    [ForeignKey(nameof(JobId))]
    public JobEntity Job { get; set; } = null!;
}
