using DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;
using DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;
using DotNetSkeleton.Modules.ProfileModule.Entities;
using DotNetSkeleton.Modules.ProfileModule.Exceptions;
using DotNetSkeleton.Modules.ProfileModule.Services.Interfaces;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.SkillModule.DTOs.Responses;
using DotNetSkeleton.Shared.Extensions;
using DotNetSkeleton.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DotNetSkeleton.Modules.ProfileModule.Services;

public sealed class ProfilesService(AppDbContext context) : IProfilesService
{
    public async Task<ProfileResponseDto> GetProfileAsync(
        int userId,
        CancellationToken cancellationToken)
    {
        var profile = await context.Profiles
            .AsNoTracking()
            .SingleOrDefaultAsync(item => item.UserId == userId, cancellationToken)
            ?? throw new ProfileNotFoundException();

        return await ToResponseAsync(profile, cancellationToken);
    }

    public async Task<ProfileResponseDto> UpdateProfileAsync(
        int userId,
        UpdateProfileRequestDto request,
        CancellationToken cancellationToken)
    {
        var profile = await context.Profiles.SingleOrDefaultAsync(
            item => item.UserId == userId,
            cancellationToken);
        if (profile is null)
        {
            profile = new ProfileEntity { UserId = userId };
            context.Profiles.Add(profile);
        }

        profile.Fullname = request.Fullname.Trim();
        profile.Note = request.Note.NormalizeOptionalText();
        profile.Major = request.Major.NormalizeOptionalText();
        profile.Gender = request.Gender.NormalizeOptionalText();
        profile.Avatar = request.Avatar.NormalizeOptionalText();
        profile.Birthdate = request.Birthdate;
        profile.Address = request.Address.NormalizeOptionalText();
        profile.Longitude = request.Longitude;
        profile.Latitude = request.Latitude;

        await context.SaveChangesAsync(cancellationToken);
        return await GetProfileAsync(userId, cancellationToken);
    }

    public async Task<ProfileResponseDto> UpdateProfileSkillsAsync(
        int userId,
        UpdateProfileSkillsRequestDto request,
        CancellationToken cancellationToken)
    {
        if (request.SkillIds is null)
            throw new ProfileSkillUnavailableException();

        var skillIds = request.SkillIds.Distinct().ToArray();
        if (skillIds.Any(id => id <= 0))
            throw new ProfileSkillUnavailableException();

        var availableSkillIds = await context.Skills
            .AsNoTracking()
            .Where(skill => skill.IsActive && skillIds.Contains(skill.Id))
            .Select(skill => skill.Id)
            .ToListAsync(cancellationToken);
        if (availableSkillIds.Count != skillIds.Length)
            throw new ProfileSkillUnavailableException();

        var profile = await GetRequiredProfileForUpdateAsync(userId, cancellationToken);
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        var existingProfileSkills = await context.ProfileSkills
            .Where(profileSkill => profileSkill.ProfileId == profile.Id)
            .ToListAsync(cancellationToken);
        var existingSkillIds = existingProfileSkills.Select(profileSkill => profileSkill.SkillId).ToHashSet();
        context.ProfileSkills.RemoveRange(
            existingProfileSkills.Where(profileSkill => !skillIds.Contains(profileSkill.SkillId)));
        await context.ProfileSkills.AddRangeAsync(
            skillIds
                .Where(skillId => !existingSkillIds.Contains(skillId))
                .Select(skillId => new ProfileSkillEntity
                {
                    ProfileId = profile.Id,
                    SkillId = skillId
                }),
            cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (
            IsForeignKeyViolation(exception, "FK_profile_skills_skills_skill_id"))
        {
            throw new ProfileSkillUnavailableException();
        }

        await transaction.CommitAsync(cancellationToken);
        return await GetProfileAsync(userId, cancellationToken);
    }

    public async Task<ProfileResponseDto> UpdateProfileExperiencesAsync(
        int userId,
        UpdateProfileExperiencesRequestDto request,
        CancellationToken cancellationToken)
    {
        if (request.Experiences is null || request.Experiences.Any(experience => experience is null))
            throw new ProfileExperienceInvalidException();

        var experiences = request.Experiences;
        var experienceIds = experiences
            .Where(experience => experience.Id.HasValue)
            .Select(experience => experience.Id!.Value)
            .ToArray();
        if (experienceIds.Any(id => id <= 0) || experienceIds.Distinct().Count() != experienceIds.Length)
            throw new ProfileExperienceInvalidException();

        var jobCategoryIds = experiences
            .Where(experience => experience.JobCategoryId.HasValue)
            .Select(experience => experience.JobCategoryId!.Value)
            .Distinct()
            .ToArray();
        if (jobCategoryIds.Any(id => id <= 0))
            throw new ProfileJobCategoryUnavailableException();

        var availableJobCategoryIds = await context.JobCategories
            .AsNoTracking()
            .Where(category => jobCategoryIds.Contains(category.Id))
            .Select(category => category.Id)
            .ToListAsync(cancellationToken);
        if (availableJobCategoryIds.Count != jobCategoryIds.Length)
            throw new ProfileJobCategoryUnavailableException();

        var profile = await GetRequiredProfileForUpdateAsync(userId, cancellationToken);
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var experiencesToUpdate = experienceIds.Length == 0
            ? new Dictionary<int, ProfileExperienceEntity>()
            : await context.ProfileExperiences
                .Where(experience => experience.ProfileId == profile.Id && experienceIds.Contains(experience.Id))
                .ToDictionaryAsync(experience => experience.Id, cancellationToken);
        if (experiencesToUpdate.Count != experienceIds.Length)
            throw new ProfileExperienceInvalidException();

        foreach (var experience in experiences)
        {
            if (experience.Id is { } experienceId)
            {
                var existing = experiencesToUpdate[experienceId];
                existing.JobTitle = experience.JobTitle.Trim();
                existing.CompanyName = experience.CompanyName.Trim();
                existing.Description = experience.Description.NormalizeOptionalText();
                existing.Title = experience.Title.NormalizeOptionalText();
                existing.JobCategoryId = experience.JobCategoryId;
                continue;
            }

            context.ProfileExperiences.Add(new ProfileExperienceEntity
            {
                ProfileId = profile.Id,
                JobTitle = experience.JobTitle.Trim(),
                CompanyName = experience.CompanyName.Trim(),
                Description = experience.Description.NormalizeOptionalText(),
                Title = experience.Title.NormalizeOptionalText(),
                JobCategoryId = experience.JobCategoryId,
                CreatedAt = DateTimeOffset.UtcNow
            });
        }

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (
            IsForeignKeyViolation(exception, "FK_profile_experiences_job_categories_job_category_id"))
        {
            throw new ProfileJobCategoryUnavailableException();
        }
        await transaction.CommitAsync(cancellationToken);

        return await GetProfileAsync(userId, cancellationToken);
    }

    private async Task<ProfileEntity> GetRequiredProfileForUpdateAsync(
        int userId,
        CancellationToken cancellationToken) =>
        await context.Profiles.SingleOrDefaultAsync(
            profile => profile.UserId == userId,
            cancellationToken)
        ?? throw new ProfileNotFoundException();

    private async Task<ProfileResponseDto> ToResponseAsync(
        ProfileEntity profile,
        CancellationToken cancellationToken)
    {
        var experiences = await context.ProfileExperiences
            .AsNoTracking()
            .Where(experience => experience.ProfileId == profile.Id)
            .OrderBy(experience => experience.Id)
            .Select(experience => new ProfileExperienceResponseDto(
                experience.Id,
                experience.JobTitle,
                experience.CompanyName,
                experience.Description,
                experience.CreatedAt,
                experience.Title,
                experience.JobCategory == null
                    ? null
                    : new JobCategoryDetailResponseDto(
                        experience.JobCategory.Id,
                        experience.JobCategory.Name,
                        experience.JobCategory.Description,
                        experience.JobCategory.Icon,
                        experience.JobCategory.IsActive,
                        experience.JobCategory.ParentId,
                        experience.JobCategory.Parent == null
                            ? null
                            : new JobCategoryParentResponseDto(
                                experience.JobCategory.Parent.Id,
                                experience.JobCategory.Parent.Name,
                                experience.JobCategory.Parent.Description,
                                experience.JobCategory.Parent.Icon,
                                experience.JobCategory.Parent.IsActive,
                                experience.JobCategory.Parent.ParentId))))
            .ToListAsync(cancellationToken);

        var skills = await context.ProfileSkills
            .AsNoTracking()
            .Where(profileSkill => profileSkill.ProfileId == profile.Id)
            .OrderBy(profileSkill => profileSkill.Id)
            .Select(profileSkill => new ProfileSkillResponseDto(
                profileSkill.Id,
                new SkillDetailResponseDto(
                    profileSkill.Skill.Id,
                    profileSkill.Skill.Name,
                    profileSkill.Skill.Description,
                    profileSkill.Skill.Icon,
                    profileSkill.Skill.IsActive)))
            .ToListAsync(cancellationToken);

        var identity = await context.ProfileIdentities
            .AsNoTracking()
            .Where(item => item.ProfileId == profile.Id)
            .Select(item => new ProfileIdentityResponseDto(
                item.CardId,
                item.FullNameOnCard,
                item.CardFrontImage,
                item.CardBackImage))
            .SingleOrDefaultAsync(cancellationToken);

        return new ProfileResponseDto(
            profile.Id,
            profile.Fullname,
            profile.Note,
            profile.Major,
            profile.Gender,
            profile.Avatar,
            profile.Birthdate,
            profile.Address,
            profile.Longitude,
            profile.Latitude,
            identity,
            experiences,
            skills);
    }

    private static bool IsForeignKeyViolation(DbUpdateException exception, string constraintName) =>
        exception.InnerException is PostgresException
        {
            SqlState: PostgresErrorCodes.ForeignKeyViolation,
            ConstraintName: var actualConstraintName
        } && actualConstraintName == constraintName;
}
