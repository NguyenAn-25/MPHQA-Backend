using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.JobModule.Services.Interfaces;
using DotNetSkeleton.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.JobModule.Services;

public sealed class CategoryService(AppDbContext context) : ICategoryService
{
    public async Task<IReadOnlyList<CategoryResponseDto>> GetActiveCategoriesAsync(
        CancellationToken cancellationToken)
    {
        var categories = await context.JobCategories
            .AsNoTracking()
            .Where(category => category.IsActive)
            .Select(category => new
            {
                category.Id,
                category.Name,
                category.Description,
                category.Icon,
                category.ParentId
            })
            .ToListAsync(cancellationToken);

        var childrenByParentId = categories
            .Where(category => category.ParentId.HasValue)
            .GroupBy(category => category.ParentId!.Value)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<CategoryChildResponseDto>)group
                    .OrderBy(category => category.Name)
                    .Select(category => new CategoryChildResponseDto(
                        category.Id,
                        category.Name,
                        category.Description,
                        category.Icon))
                    .ToList());

        return categories
            .Where(category => category.ParentId is null)
            .OrderBy(category => category.Name)
            .Select(category => new CategoryResponseDto(
                category.Id,
                category.Name,
                category.Description,
                category.Icon,
                childrenByParentId.GetValueOrDefault(category.Id, [])))
            .ToList();
    }
}
