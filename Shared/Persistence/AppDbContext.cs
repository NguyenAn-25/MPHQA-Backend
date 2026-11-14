using DotNetSkeleton.Modules.ExampleModule.Entities;
using DotNetSkeleton.Modules.AuthModule.Entities;
using DotNetSkeleton.Modules.AuthModule.Converters;
using DotNetSkeleton.Modules.JobModule.Entities;
using DotNetSkeleton.Modules.SkillModule.Entities;
using DotNetSkeleton.Modules.ProfileModule.Entities;
using DotNetSkeleton.Modules.NotificationModule.Entities;
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
    public DbSet<JobCategoryEntity> JobCategories => Set<JobCategoryEntity>();
    public DbSet<SkillEntity> Skills => Set<SkillEntity>();
    public DbSet<ProfileEntity> Profiles => Set<ProfileEntity>();
    public DbSet<ProfileIdentityEntity> ProfileIdentities => Set<ProfileIdentityEntity>();
    public DbSet<VerificationRequestEntity> VerificationRequests => Set<VerificationRequestEntity>();
    public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();
    public DbSet<NotificationDetailEntity> NotificationDetails => Set<NotificationDetailEntity>();

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

        modelBuilder.Entity<PermissionEntity>()
            .Property(permission => permission.Status)
            .HasConversion(new PermissionStatusValueConverter())
            .HasMaxLength(24);

        modelBuilder.Entity<JobCategoryEntity>()
            .HasOne(category => category.Parent)
            .WithMany(category => category.Children)
            .HasForeignKey(category => category.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

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

        modelBuilder.Entity<VerificationRequestEntity>()
            .HasOne(request => request.Sender)
            .WithMany()
            .HasForeignKey(request => request.SenderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NotificationEntity>()
            .HasOne(notification => notification.Creator)
            .WithMany()
            .HasForeignKey(notification => notification.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<NotificationDetailEntity>()
            .HasOne(detail => detail.Notification)
            .WithMany(notification => notification.Details)
            .HasForeignKey(detail => detail.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NotificationDetailEntity>()
            .HasOne(detail => detail.User)
            .WithMany()
            .HasForeignKey(detail => detail.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
