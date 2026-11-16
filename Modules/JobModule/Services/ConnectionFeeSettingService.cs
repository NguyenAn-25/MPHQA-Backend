using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.JobModule.Entities;
using DotNetSkeleton.Modules.JobModule.Exceptions;
using DotNetSkeleton.Modules.JobModule.Services.Interfaces;
using DotNetSkeleton.Shared.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.JobModule.Services;

public sealed class ConnectionFeeSettingService(AppDbContext context) : IConnectionFeeSettingService
{
    public async Task<IReadOnlyList<ConnectionFeeSettingResponseDto>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        var currentActiveId = await context.ConnectionFeeSettings
            .AsNoTracking()
            .Where(setting => setting.EffectiveFrom <= now)
            .OrderByDescending(setting => setting.EffectiveFrom)
            .ThenByDescending(setting => setting.Id)
            .Select(setting => (int?)setting.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var settings = await context.ConnectionFeeSettings
            .AsNoTracking()
            .OrderByDescending(setting => setting.EffectiveFrom)
            .ThenByDescending(setting => setting.Id)
            .Select(setting => new
            {
                setting.Id,
                setting.FeeCoin,
                setting.EffectiveFrom,
                setting.CreatedAt,
                setting.CreatedBy,
                CreatorId = setting.Creator.Id,
                CreatorPublicId = setting.Creator.PublicId,
                CreatorPhone = setting.Creator.Phone,
                CreatorFullName = context.Profiles
                    .Where(profile => profile.UserId == setting.CreatedBy)
                    .Select(profile => profile.Fullname)
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        return settings
            .Select(s => new ConnectionFeeSettingResponseDto(
                s.Id,
                s.FeeCoin,
                s.EffectiveFrom,
                s.CreatedAt,
                s.CreatedBy,
                new ConnectionFeeSettingCreatorResponseDto(
                    s.CreatorId,
                    s.CreatorPublicId,
                    s.CreatorPhone,
                    s.CreatorFullName),
                s.Id == currentActiveId))
            .ToList();
    }

    public async Task<ConnectionFeeSettingResponseDto> CreateAsync(
        CreateConnectionFeeSettingRequestDto request,
        int adminId,
        CancellationToken cancellationToken)
    {
        var admin = await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.Id == adminId, cancellationToken);

        if (admin is null)
            throw new ConnectionFeeSettingCreationException(
                StatusCodes.Status401Unauthorized,
                "Tài khoản quản trị viên không tồn tại.",
                "admin_not_found");

        var now = DateTimeOffset.UtcNow;
        var effectiveFrom = request.EffectiveFrom?.ToUniversalTime() ?? now;

        var setting = new ConnectionFeeSettingEntity
        {
            FeeCoin = request.FeeCoin,
            EffectiveFrom = effectiveFrom,
            CreatedAt = now,
            CreatedBy = adminId
        };

        context.ConnectionFeeSettings.Add(setting);
        await context.SaveChangesAsync(cancellationToken);

        var currentActiveId = await context.ConnectionFeeSettings
            .AsNoTracking()
            .Where(s => s.EffectiveFrom <= now)
            .OrderByDescending(s => s.EffectiveFrom)
            .ThenByDescending(s => s.Id)
            .Select(s => (int?)s.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var creatorFullName = await context.Profiles
            .AsNoTracking()
            .Where(profile => profile.UserId == adminId)
            .Select(profile => profile.Fullname)
            .FirstOrDefaultAsync(cancellationToken);

        return new ConnectionFeeSettingResponseDto(
            setting.Id,
            setting.FeeCoin,
            setting.EffectiveFrom,
            setting.CreatedAt,
            setting.CreatedBy,
            new ConnectionFeeSettingCreatorResponseDto(
                admin.Id,
                admin.PublicId,
                admin.Phone,
                creatorFullName),
            setting.Id == currentActiveId);
    }

    public async Task<CurrentConnectionFeeResponseDto> GetCurrentAsync(
        CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        var current = await context.ConnectionFeeSettings
            .AsNoTracking()
            .Where(setting => setting.EffectiveFrom <= now)
            .OrderByDescending(setting => setting.EffectiveFrom)
            .ThenByDescending(setting => setting.Id)
            .Select(setting => new CurrentConnectionFeeResponseDto(
                setting.Id,
                setting.FeeCoin,
                setting.EffectiveFrom))
            .FirstOrDefaultAsync(cancellationToken);

        if (current is null)
            throw new ConnectionFeeNotFoundException();

        return current;
    }
}
