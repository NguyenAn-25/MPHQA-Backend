using DotNetSkeleton.Modules.AuthModule.DTOs.Requests;
using DotNetSkeleton.Modules.AuthModule.DTOs.Responses;
using DotNetSkeleton.Modules.AuthModule.Entities;
using DotNetSkeleton.Modules.AuthModule.Exceptions;
using DotNetSkeleton.Modules.AuthModule.Mappings;
using DotNetSkeleton.Modules.AuthModule.Services.Interfaces;
using DotNetSkeleton.Shared.Extensions;
using DotNetSkeleton.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Npgsql;

namespace DotNetSkeleton.Modules.AuthModule.Services;

public sealed class PermissionsService(AppDbContext context) : IPermissionsService
{
    public async Task<PagedPermissionsResponseDto> ListAsync(
        ListPermissionsRequestDto request,
        CancellationToken cancellationToken)
    {
        var query = context.Permissions.AsNoTracking();
        var pagination = request.GetPaginationOptions();
        var search = request.Search?.Trim();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(permission => permission.Name.Contains(search));

        if (request.IsActive is { } isActive)
            query = query.Where(permission => permission.IsActive == isActive);

        var totalCount = await query.CountAsync(cancellationToken);
        IQueryable<PermissionEntity> profilesQuery = query.OrderBy(permission => permission.Id);
        if (pagination.IsEnabled)
        {
            var offset = ((long)pagination.Page - 1) * pagination.PageSize;
            if (offset > int.MaxValue)
                return new PagedPermissionsResponseDto(
                    [], pagination.Page, pagination.PageSize, totalCount);

            profilesQuery = profilesQuery.Skip((int)offset).Take(pagination.PageSize);
        }

        var profiles = await profilesQuery
            .ProjectToType<PermissionSummaryResponseDto>(AuthMappingConfiguration.Config)
            .ToListAsync(cancellationToken);

        return new PagedPermissionsResponseDto(
            profiles,
            pagination.Page,
            pagination.IsEnabled ? pagination.PageSize : profiles.Count,
            totalCount);
    }

    public async Task<PermissionDetailResponseDto> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var profile = await context.Permissions
            .AsNoTracking()
            .Where(permission => permission.Id == id)
            .ProjectToType<PermissionDetailResponseDto>(AuthMappingConfiguration.Config)
            .SingleOrDefaultAsync(cancellationToken);
        if (profile is null)
            throw new AuthPermissionNotFoundException();
        return profile;
    }

    public async Task<PermissionDetailResponseDto> CreateAsync(
        CreatePermissionRequestDto request,
        CancellationToken cancellationToken)
    {
        var name = NormalizeName(request.Name);
        var grants = await ResolveGrantsAsync(request.Details, cancellationToken);
        var profile = new PermissionEntity
        {
            Name = name,
            Description = request.Description.NormalizeOptionalText(),
            IsActive = true,
            Details = grants.Select(grant => new PermissionDetailEntity
            {
                FunctionId = grant.FunctionId,
                Action = grant.Action
            }).ToList()
        };

        context.Permissions.Add(profile);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (IsNameUniqueViolation(exception))
        {
            throw new AuthPermissionNameAlreadyExistsException();
        }

        return await GetByIdAsync(profile.Id, cancellationToken);
    }

    public async Task<PermissionDetailResponseDto> UpdateAsync(
        int id,
        UpdatePermissionRequestDto request,
        CancellationToken cancellationToken)
    {
        var profile = await context.Permissions
            .Include(permission => permission.Details)
            .SingleOrDefaultAsync(permission => permission.Id == id, cancellationToken);
        if (profile is null)
            throw new AuthPermissionNotFoundException();
        if (request.IsActive is not { } isActive)
            throw new AuthPermissionRequestInvalidException("isActive là bắt buộc.");

        var name = NormalizeName(request.Name);
        var grants = await ResolveGrantsAsync(request.Details, cancellationToken);
        var currentGrants = profile.Details
            .Select(detail => GrantKey(detail.FunctionId, detail.Action))
            .ToHashSet(StringComparer.Ordinal);
        var requestedGrants = grants
            .Select(grant => GrantKey(grant.FunctionId, grant.Action))
            .ToHashSet(StringComparer.Ordinal);
        var claimsChanged = profile.IsActive != isActive || !currentGrants.SetEquals(requestedGrants);

        profile.Name = name;
        profile.Description = request.Description.NormalizeOptionalText();
        profile.IsActive = isActive;
        var grantsToRemove = profile.Details
            .Where(detail => !requestedGrants.Contains(GrantKey(detail.FunctionId, detail.Action)))
            .ToList();
        context.PermissionDetails.RemoveRange(grantsToRemove);
        foreach (var grant in grants.Where(grant =>
                     !currentGrants.Contains(GrantKey(grant.FunctionId, grant.Action))))
        {
            profile.Details.Add(new PermissionDetailEntity
            {
                PermissionId = profile.Id,
                FunctionId = grant.FunctionId,
                Action = grant.Action
            });
        }

        try
        {
            if (claimsChanged)
            {
                await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                await RevokeProfileSessionsAsync(id, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            else
            {
                await context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (DbUpdateException exception) when (IsNameUniqueViolation(exception))
        {
            throw new AuthPermissionNameAlreadyExistsException();
        }

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<PermissionDetailResponseDto> UpdateStatusAsync(
        int id,
        UpdatePermissionStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var profile = await context.Permissions.SingleOrDefaultAsync(
            permission => permission.Id == id,
            cancellationToken);
        if (profile is null)
            throw new AuthPermissionNotFoundException();
        if (request.IsActive is not { } isActive)
            throw new AuthPermissionRequestInvalidException("isActive là bắt buộc.");
        if (profile.IsActive == isActive)
            return await GetByIdAsync(id, cancellationToken);

        await using (var transaction = await context.Database.BeginTransactionAsync(cancellationToken))
        {
            profile.IsActive = isActive;
            await RevokeProfileSessionsAsync(id, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var profile = await context.Permissions.SingleOrDefaultAsync(
            permission => permission.Id == id,
            cancellationToken);
        if (profile is null)
            throw new AuthPermissionNotFoundException();

        if (await context.HasReferencesAsync("user", "permission_id", id, cancellationToken))
            throw new AuthPermissionCannotBeDeletedException();

        context.Permissions.Remove(profile);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (IsForeignKeyViolation(exception))
        {
            // The foreign key is the final guard if an ADMIN gets this profile after
            // the reference check but before the delete is committed.
            throw new AuthPermissionCannotBeDeletedException();
        }
    }

    private async Task<IReadOnlyList<PermissionGrant>> ResolveGrantsAsync(
        IReadOnlyList<PermissionGrantRequestDto>? requests,
        CancellationToken cancellationToken)
    {
        if (requests is null)
            throw new AuthPermissionInvalidDetailsException("Danh sách quyền chi tiết là bắt buộc.");
        if (requests.Any(grant => grant is null || grant.FunctionId <= 0 || string.IsNullOrWhiteSpace(grant.Action)))
            throw new AuthPermissionInvalidDetailsException("Mỗi quyền chi tiết cần functionId và action hợp lệ.");

        var normalized = requests
            .Select(grant => new PermissionGrant(grant.FunctionId, grant.Action.Trim()))
            .ToList();
        var duplicateExists = normalized
            .Select(grant => GrantKey(grant.FunctionId, grant.Action))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count() != normalized.Count;
        if (duplicateExists)
            throw new AuthPermissionInvalidDetailsException("Danh sách quyền chi tiết không được chứa function/action trùng lặp.");

        var functionIds = normalized.Select(grant => grant.FunctionId).Distinct().ToArray();
        var functions = await context.Functions
            .Where(function => functionIds.Contains(function.Id))
            .ToDictionaryAsync(function => function.Id, cancellationToken);
        if (functions.Count != functionIds.Length)
            throw new AuthPermissionInvalidDetailsException("Danh sách có function không tồn tại.");

        var canonicalGrants = new List<PermissionGrant>(normalized.Count);
        foreach (var grant in normalized)
        {
            var function = functions[grant.FunctionId];
            var canonicalAction = function.Actions.FirstOrDefault(action =>
                string.Equals(action, grant.Action, StringComparison.OrdinalIgnoreCase));
            if (canonicalAction is null)
                throw new AuthPermissionInvalidDetailsException(
                    $"Action '{grant.Action}' không khả dụng cho function '{function.Code}'.");

            canonicalGrants.Add(new PermissionGrant(grant.FunctionId, canonicalAction));
        }

        return canonicalGrants;
    }

    private async Task RevokeProfileSessionsAsync(int permissionId, CancellationToken cancellationToken)
    {
        var userIds = await context.Users
            .Where(user => user.PermissionId == permissionId)
            .Select(user => user.Id)
            .ToListAsync(cancellationToken);
        if (userIds.Count == 0)
            return;

        var now = DateTimeOffset.UtcNow;
        await context.AuthSessions
            .Where(session => userIds.Contains(session.UserId) && session.RevokedAt == null)
            .ExecuteUpdateAsync(
                update => update.SetProperty(session => session.RevokedAt, now),
                cancellationToken);
    }

    private static string NormalizeName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new AuthPermissionRequestInvalidException("Tên nhóm quyền không được để trống.");

        return name.Trim();
    }

    private static string GrantKey(int functionId, string action) =>
        $"{functionId}:{action.ToLowerInvariant()}";

    private static bool IsNameUniqueViolation(DbUpdateException exception) =>
        exception.InnerException is PostgresException
        {
            SqlState: PostgresErrorCodes.UniqueViolation,
            ConstraintName: "IX_permissions_Name"
        };

    private static bool IsForeignKeyViolation(DbUpdateException exception) =>
        exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.ForeignKeyViolation };

    private sealed record PermissionGrant(int FunctionId, string Action);
}
