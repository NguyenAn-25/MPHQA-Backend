using DotNetSkeleton.Modules.ExampleModule.Entities;
using DotNetSkeleton.Modules.AuthModule.Entities;
using DotNetSkeleton.Modules.AuthModule.Converters;
using DotNetSkeleton.Modules.JobModule.Entities;
using DotNetSkeleton.Modules.JobModule.Converters;
using DotNetSkeleton.Modules.SkillModule.Entities;
using DotNetSkeleton.Modules.MessagingModule.Entities;
using DotNetSkeleton.Modules.NotificationModule.Entities;
using DotNetSkeleton.Modules.ProfileModule.Entities;
using DotNetSkeleton.Modules.ProfileModule.Converters;
using DotNetSkeleton.Modules.NotificationModule.Converters;
using DotNetSkeleton.Modules.ReportModule.Entities;
using DotNetSkeleton.Modules.WalletModule.Entities;
using DotNetSkeleton.Modules.WalletModule.Converters;
using DotNetSkeleton.Modules.ReportModule.Converters;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Shared.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public DbSet<Example> Examples { get; set; }
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<AuthSessionEntity> AuthSessions => Set<AuthSessionEntity>();
    public DbSet<AuthOtpEntity> AuthOtps => Set<AuthOtpEntity>();
    public DbSet<PermissionEntity> Permissions => Set<PermissionEntity>();
    public DbSet<PermissionDetailEntity> PermissionDetails => Set<PermissionDetailEntity>();
    public DbSet<FunctionEntity> Functions => Set<FunctionEntity>();
    public DbSet<JobEntity> Jobs => Set<JobEntity>();
    public DbSet<ApplicationEntity> Applications => Set<ApplicationEntity>();
    public DbSet<JobCategoryEntity> JobCategories => Set<JobCategoryEntity>();
    public DbSet<JobFeedbackEntity> JobFeedbacks => Set<JobFeedbackEntity>();
    public DbSet<JobImageEntity> JobImages => Set<JobImageEntity>();
    public DbSet<JobRequiredSkillEntity> JobRequiredSkills => Set<JobRequiredSkillEntity>();
    public DbSet<PostPackageEntity> PostPackages => Set<PostPackageEntity>();
    public DbSet<SavedJobEntity> SavedJobs => Set<SavedJobEntity>();
    public DbSet<ConnectionFeeSettingEntity> ConnectionFeeSettings => Set<ConnectionFeeSettingEntity>();
    public DbSet<SkillEntity> Skills => Set<SkillEntity>();
    public DbSet<ProfileEntity> Profiles => Set<ProfileEntity>();
    public DbSet<ProfileExperienceEntity> ProfileExperiences => Set<ProfileExperienceEntity>();
    public DbSet<ProfileIdentityEntity> ProfileIdentities => Set<ProfileIdentityEntity>();
    public DbSet<ProfileSkillEntity> ProfileSkills => Set<ProfileSkillEntity>();
    public DbSet<VerificationRequestEntity> VerificationRequests => Set<VerificationRequestEntity>();
    public DbSet<WalletEntity> Wallets => Set<WalletEntity>();
    public DbSet<WalletTransactionEntity> WalletTransactions => Set<WalletTransactionEntity>();
    public DbSet<TopUpOrderEntity> TopUpOrders => Set<TopUpOrderEntity>();
    public DbSet<PaymentMethodEntity> PaymentMethods => Set<PaymentMethodEntity>();
    public DbSet<ConversationEntity> Conversations => Set<ConversationEntity>();
    public DbSet<MessageEntity> Messages => Set<MessageEntity>();
    public DbSet<MessageImageEntity> MessageImages => Set<MessageImageEntity>();
    public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();
    public DbSet<NotificationDetailEntity> NotificationDetails => Set<NotificationDetailEntity>();
    public DbSet<ReportEntity> Reports => Set<ReportEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>()
            .HasOne(user => user.Permission)
            .WithMany(permission => permission.Users)
            .HasForeignKey(user => user.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserEntity>()
            .Property(user => user.Role)
            .HasConversion(new UserRoleValueConverter())
            .HasMaxLength(16);

        modelBuilder.Entity<UserEntity>()
            .Property(user => user.IsVerified)
            .HasDefaultValue(false);

        modelBuilder.Entity<UserEntity>()
            .ToTable(table => table.HasCheckConstraint(
                "CK_user_role_permission",
                "(role = 'USER' AND permission_id IS NULL) OR (role = 'ADMIN' AND permission_id IS NOT NULL)"));

        modelBuilder.Entity<UserEntity>()
            .ToTable(table => table.HasCheckConstraint(
                "CK_user_verification_state",
                "(is_verified = TRUE AND verified_at IS NOT NULL) OR (is_verified = FALSE AND verified_at IS NULL)"));

        modelBuilder.Entity<AuthOtpEntity>()
            .Property(otp => otp.Purpose)
            .HasConversion(new OtpPurposeValueConverter())
            .HasMaxLength(32);

        modelBuilder.Entity<VerificationRequestEntity>()
            .Property(request => request.Status)
            .HasConversion(new VerificationRequestStatusValueConverter());

        modelBuilder.Entity<JobCategoryEntity>()
            .HasOne(category => category.Parent)
            .WithMany(category => category.Children)
            .HasForeignKey(category => category.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JobEntity>()
            .HasOne(job => job.Creator)
            .WithMany()
            .HasForeignKey(job => job.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JobEntity>()
            .HasOne(job => job.JobCategory)
            .WithMany()
            .HasForeignKey(job => job.JobCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JobEntity>()
            .HasOne(job => job.PostPackage)
            .WithMany(package => package.Jobs)
            .HasForeignKey(job => job.PostPackageId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<JobImageEntity>()
            .HasOne(image => image.Job)
            .WithMany(job => job.Images)
            .HasForeignKey(image => image.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JobRequiredSkillEntity>()
            .HasOne(requiredSkill => requiredSkill.Job)
            .WithMany(job => job.RequiredSkills)
            .HasForeignKey(requiredSkill => requiredSkill.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JobRequiredSkillEntity>()
            .HasOne(requiredSkill => requiredSkill.Skill)
            .WithMany()
            .HasForeignKey(requiredSkill => requiredSkill.SkillId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JobRequiredSkillEntity>()
            .HasIndex(requiredSkill => new { requiredSkill.JobId, requiredSkill.SkillId })
            .IsUnique();

        modelBuilder.Entity<ApplicationEntity>()
            .HasOne(application => application.Job)
            .WithMany(job => job.Applications)
            .HasForeignKey(application => application.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicationEntity>()
            .HasOne(application => application.User)
            .WithMany()
            .HasForeignKey(application => application.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SavedJobEntity>()
            .HasOne(savedJob => savedJob.Job)
            .WithMany(job => job.SavedByUsers)
            .HasForeignKey(savedJob => savedJob.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SavedJobEntity>()
            .HasOne(savedJob => savedJob.User)
            .WithMany()
            .HasForeignKey(savedJob => savedJob.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JobFeedbackEntity>()
            .HasOne(feedback => feedback.Job)
            .WithMany(job => job.Feedbacks)
            .HasForeignKey(feedback => feedback.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JobFeedbackEntity>()
            .HasOne(feedback => feedback.Reviewer)
            .WithMany()
            .HasForeignKey(feedback => feedback.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JobFeedbackEntity>()
            .HasOne(feedback => feedback.Reviewee)
            .WithMany()
            .HasForeignKey(feedback => feedback.RevieweeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JobEntity>()
            .HasIndex(job => new { job.Status, job.CreatedAt });

        modelBuilder.Entity<JobEntity>()
            .HasIndex(job => job.JobCategoryId);

        modelBuilder.Entity<JobEntity>()
            .HasIndex(job => job.CreatedBy);

        modelBuilder.Entity<ProfileEntity>()
            .HasOne(profile => profile.User)
            .WithOne()
            .HasForeignKey<ProfileEntity>(profile => profile.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProfileIdentityEntity>()
            .HasOne(identity => identity.Profile)
            .WithOne(profile => profile.Identity)
            .HasForeignKey<ProfileIdentityEntity>(identity => identity.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProfileExperienceEntity>()
            .HasOne(experience => experience.Profile)
            .WithMany(profile => profile.Experiences)
            .HasForeignKey(experience => experience.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProfileExperienceEntity>()
            .HasOne(experience => experience.JobCategory)
            .WithMany()
            .HasForeignKey(experience => experience.JobCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProfileSkillEntity>()
            .HasOne(profileSkill => profileSkill.Profile)
            .WithMany(profile => profile.ProfileSkills)
            .HasForeignKey(profileSkill => profileSkill.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProfileSkillEntity>()
            .HasOne(profileSkill => profileSkill.Skill)
            .WithMany()
            .HasForeignKey(profileSkill => profileSkill.SkillId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProfileSkillEntity>()
            .HasIndex(profileSkill => new { profileSkill.ProfileId, profileSkill.SkillId })
            .IsUnique();

        modelBuilder.Entity<VerificationRequestEntity>()
            .HasOne(request => request.Sender)
            .WithMany()
            .HasForeignKey(request => request.SenderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VerificationRequestEntity>()
            .HasIndex(request => request.SenderId)
            .IsUnique()
            .HasDatabaseName("IX_verification_requests_sender_id_pending")
            .HasFilter("\"status\" = 'PENDING'");

        modelBuilder.Entity<NotificationEntity>()
            .HasOne(notification => notification.Creator)
            .WithMany()
            .HasForeignKey(notification => notification.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<NotificationEntity>()
            .Property(notification => notification.SendTo)
            .HasConversion(new NotificationSendToValueConverter());

        modelBuilder.Entity<ReportEntity>()
            .Property(report => report.ObjectType)
            .HasConversion(new ReportObjectTypeValueConverter());

        modelBuilder.Entity<ReportEntity>()
            .Property(report => report.Status)
            .HasConversion(new ReportStatusValueConverter());

        modelBuilder.Entity<ReportEntity>()
            .HasOne(report => report.Sender)
            .WithMany()
            .HasForeignKey(report => report.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReportEntity>()
            .HasIndex(report => new { report.Status, report.CreatedAt });

        modelBuilder.Entity<ReportEntity>()
            .HasIndex(report => new { report.ObjectType, report.ObjectId });

        modelBuilder.Entity<NotificationDetailEntity>()
            .HasOne(detail => detail.Notification)
            .WithMany(notification => notification.Details)
            .HasForeignKey(detail => detail.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NotificationDetailEntity>()
            .HasIndex(detail => new { detail.NotificationId, detail.UserId })
            .IsUnique();

        modelBuilder.Entity<NotificationDetailEntity>()
            .HasOne(detail => detail.User)
            .WithMany()
            .HasForeignKey(detail => detail.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JobEntity>()
            .Property(job => job.Status)
            .HasConversion(new JobStatusValueConverter())
            .HasMaxLength(16);

        modelBuilder.Entity<JobEntity>()
            .Property(job => job.PricingModel)
            .HasConversion(new PricingModelValueConverter())
            .HasMaxLength(24);

        modelBuilder.Entity<ApplicationEntity>()
            .Property(application => application.Status)
            .HasConversion(new ApplicationStatusValueConverter())
            .HasMaxLength(16);

        modelBuilder.Entity<WalletTransactionEntity>()
            .Property(transaction => transaction.ObjectType)
            .HasConversion(new WalletObjectTypeValueConverter())
            .HasMaxLength(16);

        modelBuilder.Entity<WalletTransactionEntity>()
            .Property(transaction => transaction.Type)
            .HasConversion(new WalletTransactionTypeValueConverter())
            .HasMaxLength(16);

    }
}
