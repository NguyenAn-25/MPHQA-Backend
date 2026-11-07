using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.JobModule.Entities;

[Table("jobs")]
public sealed class JobEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("salary_value", TypeName = "numeric(15,2)")]
    public decimal SalaryValue { get; set; }

    [Required]
    [Column("salary_unit")]
    public string SalaryUnit { get; set; } = string.Empty;

    [Required]
    [Column("total_salary_payment", TypeName = "numeric(15,2)")]
    public decimal TotalSalaryPayment { get; set; }

    [Required]
    [Column("total_platform_fee", TypeName = "numeric(15,2)")]
    public decimal TotalPlatformFee { get; set; }

    [Required]
    [Column("total_expense", TypeName = "numeric(15,2)")]
    public decimal TotalExpense { get; set; }

    [Column("completed_at")]
    public DateTimeOffset? CompletedAt { get; set; }

    [Column("working_at")]
    public DateTimeOffset? WorkingAt { get; set; }

    [Column("full_at")]
    public DateTimeOffset? FullAt { get; set; }

    [Column("rejected_at")]
    public DateTimeOffset? RejectedAt { get; set; }

    [Column("accepted_at")]
    public DateTimeOffset? AcceptedAt { get; set; }

    [Required]
    [Column("is_paid")]
    public bool IsPaid { get; set; }

    [Required]
    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Column("description", TypeName = "text")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Column("address")]
    public string Address { get; set; } = string.Empty;

    [Column("lng", TypeName = "numeric(10,7)")]
    public decimal? Longitude { get; set; }

    [Column("lat", TypeName = "numeric(10,7)")]
    public decimal? Latitude { get; set; }

    [Column("cancelled_at")]
    public DateTimeOffset? CancelledAt { get; set; }

    [Required]
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    [Column("required_number_of_workers")]
    public int RequiredNumberOfWorkers { get; set; }

    [Column("expired_at")]
    public DateTimeOffset? ExpiredAt { get; set; }

    [Required]
    [Column("time_start_at")]
    public DateTimeOffset TimeStartAt { get; set; }

    [Required]
    [Column("time_end_at")]
    public DateTimeOffset TimeEndAt { get; set; }

    [Required]
    [Column("status")]
    public string Status { get; set; } = "PENDING";

    [Column("required_description", TypeName = "text")]
    public string? RequiredDescription { get; set; }

    [Column("auto_reply_message", TypeName = "text")]
    public string? AutoReplyMessage { get; set; }

    [Required]
    [Column("pricing_model")]
    public string PricingModel { get; set; } = string.Empty;

    [Required]
    [Column("post_fee_coin")]
    public int PostFeeCoin { get; set; }

    [Required]
    [Column("created_by")]
    public int CreatedBy { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity Creator { get; set; } = null!;

    [Required]
    [Column("job_category_id")]
    public int JobCategoryId { get; set; }

    [ForeignKey(nameof(JobCategoryId))]
    public JobCategoryEntity JobCategory { get; set; } = null!;

    [Column("post_package_id")]
    public int? PostPackageId { get; set; }

    [ForeignKey(nameof(PostPackageId))]
    public PostPackageEntity? PostPackage { get; set; }

    public ICollection<JobImageEntity> Images { get; set; } = new List<JobImageEntity>();
    public ICollection<JobRequiredSkillEntity> RequiredSkills { get; set; } = new List<JobRequiredSkillEntity>();
    public ICollection<SavedJobEntity> SavedByUsers { get; set; } = new List<SavedJobEntity>();
    public ICollection<ApplicationEntity> Applications { get; set; } = new List<ApplicationEntity>();
    public ICollection<JobFeedbackEntity> Feedbacks { get; set; } = new List<JobFeedbackEntity>();
}
