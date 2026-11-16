using DotNetSkeleton.Modules.AuthModule.Entities;
using DotNetSkeleton.Modules.AuthModule.Constants;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Shared.Persistence.Seeders;

public sealed class AuthPermissionCatalogSeeder : ISeeder
{
    private static readonly (string Code, string NameVi, string NameEn, string[] Actions)[] FunctionCatalog =
    [
        // Keep this catalog aligned with the permission policies on implemented endpoints.
        ("USERS", "Quản lý người dùng", "User Management", ["READ", "CREATE", "UPDATE"]),
        ("JOB_CATEGORIES", "Quản lý lĩnh vực", "Job Category Management", ["READ", "CREATE", "UPDATE", "DELETE"]),
        ("SKILLS", "Quản lý kỹ năng", "Skill Management", ["READ", "CREATE", "UPDATE", "DELETE"]),
        ("VERIFICATION_REQUESTS", "Xử lý yêu cầu xác thực", "Verification Request Management", ["READ", "UPDATE"]),
        ("NOTIFICATIONS", "Quản lý thông báo", "Notification Management", ["READ", "CREATE"]),
        ("PERMISSIONS", "Quản lý nhóm quyền", "Permission Management", ["READ", "CREATE", "UPDATE", "DELETE"])
    ];

    private static readonly (string Function, string Action)[] InitialAdminGrants =
    [
        ("USERS", "READ"),
        ("USERS", "CREATE"),
        ("USERS", "UPDATE"),
        ("JOB_CATEGORIES", "READ"),
        ("JOB_CATEGORIES", "CREATE"),
        ("JOB_CATEGORIES", "UPDATE"),
        ("JOB_CATEGORIES", "DELETE"),
        ("SKILLS", "READ"),
        ("SKILLS", "CREATE"),
        ("SKILLS", "UPDATE"),
        ("SKILLS", "DELETE"),
        ("VERIFICATION_REQUESTS", "READ"),
        ("VERIFICATION_REQUESTS", "UPDATE"),
        ("NOTIFICATIONS", "READ"),
        ("NOTIFICATIONS", "CREATE"),
        ("PERMISSIONS", "READ"),
        ("PERMISSIONS", "CREATE"),
        ("PERMISSIONS", "UPDATE"),
        ("PERMISSIONS", "DELETE")
    ];

    private static readonly string[] RetiredFunctionCodes = ["AUTH", "PERMISSION_PROFILES"];

    public int Order => 1;

    public async Task<int> SeedAsync(AppDbContext context)
    {
        var added = 0;
        var functions = new Dictionary<string, FunctionEntity>(StringComparer.OrdinalIgnoreCase);
        foreach (var definition in FunctionCatalog)
        {
            var function = await context.Functions
                .SingleOrDefaultAsync(x => x.Code == definition.Code);
            if (function is null)
            {
                function = new FunctionEntity
                {
                    Code = definition.Code,
                    NameVi = definition.NameVi,
                    NameEn = definition.NameEn,
                    Actions = definition.Actions
                };
                context.Functions.Add(function);
                added++;
            }
            else
            {
                function.NameVi = definition.NameVi;
                function.NameEn = definition.NameEn;
                function.Actions = definition.Actions;
            }

            functions.Add(definition.Code, function);
        }

        var adminPermissionWasCreated = !await context.Permissions
            .AnyAsync(x => x.Name == PermissionConstants.DefaultAdminProfileName);
        var adminPermission = await GetOrCreatePermissionAsync(
            context,
            PermissionConstants.DefaultAdminProfileName,
            "Permission profile mặc định cho user có role ADMIN.",
            () => added++);
        await context.SaveChangesAsync();

        // Remove retired functions and unsupported actions while preserving grants
        // from every permission profile, including custom profiles assigned to ADMIN users.
        var obsoleteGrants = await context.PermissionDetails
            .Include(detail => detail.Permission)
            .Include(detail => detail.Function)
            .ToListAsync();
        var supportedActions = FunctionCatalog.ToDictionary(
            definition => definition.Code,
            definition => definition.Actions,
            StringComparer.OrdinalIgnoreCase);
        context.PermissionDetails.RemoveRange(obsoleteGrants.Where(detail =>
            RetiredFunctionCodes.Any(code =>
                string.Equals(code, detail.Function.Code, StringComparison.OrdinalIgnoreCase)) ||
            (supportedActions.TryGetValue(detail.Function.Code, out var actions) &&
             !actions.Any(action => string.Equals(
                 action,
                 detail.Action,
                 StringComparison.OrdinalIgnoreCase)))));

        var retiredFunctions = await context.Functions
            .Where(function => RetiredFunctionCodes.Contains(function.Code))
            .ToListAsync();
        context.Functions.RemoveRange(retiredFunctions);

        // Seed a small initial admin grant set only when the role is first created.
        // On later startups, intentional permission_details edits (including removals) persist.
        if (adminPermissionWasCreated)
        {
            foreach (var grant in InitialAdminGrants)
                added += await AddGrantIfMissingAsync(
                    context,
                    adminPermission,
                    functions[grant.Function],
                    grant.Action);
        }

        await context.SaveChangesAsync();
        return added;
    }

    private static async Task<PermissionEntity> GetOrCreatePermissionAsync(
        AppDbContext context,
        string name,
        string description,
        Action onAdded)
    {
        var permission = await context.Permissions.SingleOrDefaultAsync(x => x.Name == name);
        if (permission is not null)
        {
            permission.Description = description;
            return permission;
        }

        permission = new PermissionEntity { Name = name, IsActive = true, Description = description };
        context.Permissions.Add(permission);
        onAdded();
        return permission;
    }

    private static async Task<int> AddGrantIfMissingAsync(
        AppDbContext context,
        PermissionEntity permission,
        FunctionEntity function,
        string action)
    {
        var exists = await context.PermissionDetails.AnyAsync(x =>
            x.PermissionId == permission.Id && x.FunctionId == function.Id && x.Action == action);
        if (exists)
            return 0;

        context.PermissionDetails.Add(new PermissionDetailEntity
        {
            Permission = permission,
            Function = function,
            Action = action
        });
        return 1;
    }
}
