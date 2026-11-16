using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.JobModule.Entities;
using DotNetSkeleton.Modules.JobModule.Exceptions;
using DotNetSkeleton.Modules.JobModule.Services.Interfaces;
using DotNetSkeleton.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.JobModule.Services;

public sealed class PostPackageService(AppDbContext context) : IPostPackageService
{
    public async Task<IReadOnlyList<PostPackageResponseDto>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await context.PostPackages
            .AsNoTracking()
            .OrderBy(package => package.DurationDays)
            .ThenBy(package => package.PriceCoin)
            .ThenBy(package => package.Id)
            .Select(package => new PostPackageResponseDto(
                package.Id,
                package.Name,
                package.DurationDays,
                package.PriceCoin,
                package.PriceMoney))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AdminPostPackageResponseDto>> GetAllForAdminAsync(
        CancellationToken cancellationToken)
    {
        return await context.PostPackages
            .AsNoTracking()
            .OrderBy(package => package.DurationDays)
            .ThenBy(package => package.PriceCoin)
            .ThenBy(package => package.Id)
            .Select(package => new AdminPostPackageResponseDto(
                package.Id,
                package.Name,
                package.DurationDays,
                package.PriceCoin,
                package.PriceMoney,
                package.Jobs.Count))
            .ToListAsync(cancellationToken);
    }

    public async Task<AdminPostPackageResponseDto> CreateAsync(
        CreatePostPackageRequestDto request,
        CancellationToken cancellationToken)
    {
        var package = new PostPackageEntity
        {
            Name = request.Name.Trim(),
            DurationDays = request.DurationDays,
            PriceCoin = request.PriceCoin,
            PriceMoney = request.PriceMoney
        };

        context.PostPackages.Add(package);
        await context.SaveChangesAsync(cancellationToken);

        return new AdminPostPackageResponseDto(
            package.Id,
            package.Name,
            package.DurationDays,
            package.PriceCoin,
            package.PriceMoney,
            0);
    }

    public async Task<AdminPostPackageResponseDto> UpdateAsync(
        int id,
        UpdatePostPackageRequestDto request,
        CancellationToken cancellationToken)
    {
        var package = await context.PostPackages
            .SingleOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);

        if (package is null)
            throw new PostPackageNotFoundException(id);

        if (request.Name is null &&
            request.DurationDays is null &&
            request.PriceCoin is null &&
            request.PriceMoney is null)
            throw new PostPackageUpdateException();

        if (request.Name is not null)
            package.Name = request.Name.Trim();
        if (request.DurationDays is { } durationDays)
            package.DurationDays = durationDays;
        if (request.PriceCoin is { } priceCoin)
            package.PriceCoin = priceCoin;
        if (request.PriceMoney is { } priceMoney)
            package.PriceMoney = priceMoney;

        await context.SaveChangesAsync(cancellationToken);

        return new AdminPostPackageResponseDto(
            package.Id,
            package.Name,
            package.DurationDays,
            package.PriceCoin,
            package.PriceMoney,
            await context.Jobs.CountAsync(
                job => job.PostPackageId == package.Id,
                cancellationToken));
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var package = await context.PostPackages
            .SingleOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);

        if (package is null)
            throw new PostPackageNotFoundException(id);

        if (await context.Jobs.AnyAsync(
                job => job.PostPackageId == package.Id,
                cancellationToken))
            throw new PostPackageInUseException(id);

        context.PostPackages.Remove(package);
        await context.SaveChangesAsync(cancellationToken);
    }
}
