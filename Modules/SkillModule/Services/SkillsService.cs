using DotNetSkeleton.Modules.SkillModule.DTOs.Requests;
using DotNetSkeleton.Modules.SkillModule.DTOs.Responses;
using DotNetSkeleton.Modules.SkillModule.Entities;
using DotNetSkeleton.Modules.SkillModule.Exceptions;
using DotNetSkeleton.Modules.SkillModule.Mappings;
using DotNetSkeleton.Modules.SkillModule.Services.Interfaces;
using DotNetSkeleton.Shared.Extensions;
using DotNetSkeleton.Shared.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.SkillModule.Services;

public sealed class SkillsService(AppDbContext context) : ISkillsService
{
    public async Task<PagedSkillsResponseDto> ListAsync(
        ListSkillsRequestDto request,
        CancellationToken cancellationToken)
    {
        var query = context.Skills.AsNoTracking();
        var search = request.Search?.Trim();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(skill => skill.Name.Contains(search));
        if (request.IsActive is { } isActive)
            query = query.Where(skill => skill.IsActive == isActive);

        var totalCount = await query.CountAsync(cancellationToken);
        var offset = ((long)request.Page - 1) * request.PageSize;
        if (offset > int.MaxValue)
            return new PagedSkillsResponseDto([], request.Page, request.PageSize, totalCount);

        var skills = await query
            .OrderBy(skill => skill.Name)
            .ThenBy(skill => skill.Id)
            .Skip((int)offset)
            .Take(request.PageSize)
            .ProjectToType<SkillSummaryResponseDto>(SkillMappingConfiguration.Config)
            .ToListAsync(cancellationToken);

        return new PagedSkillsResponseDto(skills, request.Page, request.PageSize, totalCount);
    }

    public async Task<IReadOnlyList<SkillOptionResponseDto>> GetOptionsAsync(CancellationToken cancellationToken) =>
        await context.Skills
            .AsNoTracking()
            .Where(skill => skill.IsActive)
            .OrderBy(skill => skill.Name)
            .ThenBy(skill => skill.Id)
            .ProjectToType<SkillOptionResponseDto>(SkillMappingConfiguration.Config)
            .ToListAsync(cancellationToken);

    public async Task<SkillDetailResponseDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var skill = await context.Skills
            .AsNoTracking()
            .Where(skill => skill.Id == id)
            .ProjectToType<SkillDetailResponseDto>(SkillMappingConfiguration.Config)
            .SingleOrDefaultAsync(cancellationToken);

        return skill ?? throw new SkillNotFoundException();
    }

    public async Task<SkillDetailResponseDto> CreateAsync(
        CreateSkillRequestDto request,
        CancellationToken cancellationToken)
    {
        var skill = new SkillEntity
        {
            Name = request.Name.Trim(),
            Description = request.Description.NormalizeOptionalText(),
            Icon = request.Icon.NormalizeOptionalText(),
            IsActive = true
        };

        context.Skills.Add(skill);
        await context.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(skill.Id, cancellationToken);
    }

    public async Task<SkillDetailResponseDto> UpdateAsync(
        int id,
        UpdateSkillRequestDto request,
        CancellationToken cancellationToken)
    {
        var skill = await context.Skills.SingleOrDefaultAsync(
            item => item.Id == id,
            cancellationToken);
        if (skill is null)
            throw new SkillNotFoundException();

        skill.Name = request.Name.Trim();
        skill.Description = request.Description.NormalizeOptionalText();
        skill.Icon = request.Icon.NormalizeOptionalText();
        await context.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<SkillDetailResponseDto> UpdateStatusAsync(
        int id,
        UpdateSkillStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var skill = await context.Skills.SingleOrDefaultAsync(
            item => item.Id == id,
            cancellationToken);
        if (skill is null)
            throw new SkillNotFoundException();

        skill.IsActive = request.IsActive!.Value;
        await context.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var skill = await context.Skills.SingleOrDefaultAsync(
            item => item.Id == id,
            cancellationToken);
        if (skill is null)
            throw new SkillNotFoundException();

        var isUsedByProfiles = await context.HasReferencesAsync(
            "profile_skills",
            "skill_id",
            id,
            cancellationToken);
        var isUsedByJobs = await context.HasReferencesAsync(
            "job_required_skills",
            "skill_id",
            id,
            cancellationToken);
        if (isUsedByProfiles || isUsedByJobs)
            throw new SkillCannotBeDeletedException();

        context.Skills.Remove(skill);
        await context.SaveChangesAsync(cancellationToken);
    }
}
