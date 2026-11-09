using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.JobModule.Entities;
using DotNetSkeleton.Modules.JobModule.Exceptions;
using DotNetSkeleton.Modules.JobModule.Mappings;
using DotNetSkeleton.Modules.JobModule.Services.Interfaces;
using Mapster;
using DotNetSkeleton.Shared.Extensions;
using DotNetSkeleton.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.JobModule.Services;

public sealed class JobCategoriesService(AppDbContext context) : IJobCategoriesService
{
    public async Task<PagedJobCategoriesResponseDto> ListAsync(
        ListJobCategoriesRequestDto request,
        CancellationToken cancellationToken)
    {
        var categories = await GetOrderedCategoriesAsync(cancellationToken);
        var query = categories.Where(category => category.ParentId == request.ParentId);
        var search = request.Search?.Trim();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(category => category.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        if (request.IsActive is { } isActive)
            query = query.Where(category => category.IsActive == isActive);

        var matchingCategories = query.ToList();
        var totalCount = matchingCategories.Count;
        var offset = ((long)request.Page - 1) * request.PageSize;
        if (offset > int.MaxValue)
            return new PagedJobCategoriesResponseDto([], request.Page, request.PageSize, totalCount);

        var childrenByParentId = categories.ToLookup(category => category.ParentId);
        var items = matchingCategories
            .Skip((int)offset)
            .Take(request.PageSize)
            .Select(category => ToSummary(category, childrenByParentId, new HashSet<int>()))
            .ToArray();

        return new PagedJobCategoriesResponseDto(items, request.Page, request.PageSize, totalCount);
    }

    public async Task<IReadOnlyList<JobCategoryOptionResponseDto>> GetOptionsAsync(
        CancellationToken cancellationToken)
    {
        var activeCategories = await context.JobCategories
            .AsNoTracking()
            .Where(category => category.IsActive)
            .OrderBy(category => category.Name)
            .ThenBy(category => category.Id)
            .ToListAsync(cancellationToken);
        var childrenByParentId = activeCategories.ToLookup(category => category.ParentId);
        var options = new List<JobCategoryOptionResponseDto>();

        AddOptionsUnderParent(null);
        return options;

        void AddOptionsUnderParent(int? parentId)
        {
            foreach (var category in childrenByParentId[parentId])
            {
                options.Add(new JobCategoryOptionResponseDto(category.Id, category.Name, category.ParentId));
                AddOptionsUnderParent(category.Id);
            }
        }
    }

    public async Task<JobCategoryDetailResponseDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var category = await context.JobCategories
            .AsNoTracking()
            .Include(item => item.Parent)
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken)
            ?? throw new JobCategoryNotFoundException();

        var response = category.Adapt<JobCategoryDetailResponseDto>(JobCategoryMappingConfiguration.Config);
        return response with { ParentName = category.Parent?.Name };
    }

    public async Task<JobCategoryDetailResponseDto> CreateAsync(
        CreateJobCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        await ValidateParentAsync(request.ParentId, null, cancellationToken);

        var category = new JobCategoryEntity
        {
            Name = request.Name.Trim(),
            Description = request.Description.NormalizeOptionalText(),
            Icon = request.Icon.NormalizeOptionalText(),
            ParentId = request.ParentId,
            IsActive = true
        };

        context.JobCategories.Add(category);
        await context.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(category.Id, cancellationToken);
    }

    public async Task<JobCategoryDetailResponseDto> UpdateAsync(
        int id,
        UpdateJobCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        var category = await context.JobCategories.SingleOrDefaultAsync(
            item => item.Id == id,
            cancellationToken);
        if (category is null)
            throw new JobCategoryNotFoundException();

        await ValidateParentAsync(request.ParentId, id, cancellationToken);

        category.Name = request.Name.Trim();
        category.Description = request.Description.NormalizeOptionalText();
        category.Icon = request.Icon.NormalizeOptionalText();
        category.ParentId = request.ParentId;
        await context.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<JobCategoryDetailResponseDto> UpdateStatusAsync(
        int id,
        UpdateJobCategoryStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var category = await context.JobCategories.SingleOrDefaultAsync(
            item => item.Id == id,
            cancellationToken);
        if (category is null)
            throw new JobCategoryNotFoundException();

        category.IsActive = request.IsActive!.Value;
        await context.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var category = await context.JobCategories.SingleOrDefaultAsync(
            item => item.Id == id,
            cancellationToken);
        if (category is null)
            throw new JobCategoryNotFoundException();

        var hasChildren = await context.JobCategories.AnyAsync(
            item => item.ParentId == id,
            cancellationToken);
        var hasJobs = await context.HasReferencesAsync("jobs", "job_category_id", id, cancellationToken);
        var hasProfileExperiences = await context.HasReferencesAsync(
            "profile_experiences",
            "job_category_id",
            id,
            cancellationToken);
        if (hasChildren || hasJobs || hasProfileExperiences)
            throw new JobCategoryCannotBeDeletedException();

        context.JobCategories.Remove(category);
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<List<JobCategoryEntity>> GetOrderedCategoriesAsync(CancellationToken cancellationToken) =>
        await context.JobCategories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .ThenBy(category => category.Id)
            .ToListAsync(cancellationToken);

    private async Task ValidateParentAsync(
        int? parentId,
        int? categoryId,
        CancellationToken cancellationToken)
    {
        if (parentId is null)
            return;

        var parentIds = await context.JobCategories
            .AsNoTracking()
            .Select(category => new { category.Id, category.ParentId })
            .ToDictionaryAsync(category => category.Id, category => category.ParentId, cancellationToken);
        if (!parentIds.ContainsKey(parentId.Value))
            throw new JobCategoryParentNotFoundException();

        var visited = new HashSet<int>();
        int? currentId = parentId;
        while (currentId is { } ancestorId && parentIds.TryGetValue(ancestorId, out var ancestorParentId))
        {
            if (ancestorId == categoryId)
                throw new JobCategoryHierarchyCycleException();
            if (!visited.Add(ancestorId))
                throw new JobCategoryHierarchyCycleException();

            currentId = ancestorParentId;
        }
    }

    private static JobCategorySummaryResponseDto ToSummary(
        JobCategoryEntity category,
        ILookup<int?, JobCategoryEntity> childrenByParentId,
        HashSet<int> currentPath)
    {
        var response = category.Adapt<JobCategorySummaryResponseDto>(JobCategoryMappingConfiguration.Config);
        if (!currentPath.Add(category.Id))
            return response;

        var children = childrenByParentId[category.Id]
            .Where(child => !currentPath.Contains(child.Id))
            .Select(child => ToSummary(child, childrenByParentId, currentPath))
            .ToArray();
        currentPath.Remove(category.Id);

        return response with
        {
            Children = children
        };
    }

}
