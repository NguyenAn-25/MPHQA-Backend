using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetSkeleton.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLegacyPermissionProfilesFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE auth_sessions AS session
                SET revoked_at = now()
                WHERE session.revoked_at IS NULL
                  AND session.user_id IN (
                    SELECT DISTINCT account.id
                    FROM "user" AS account
                    JOIN permission_details AS detail ON detail.permission_id = account.permission_id
                    JOIN functions AS old_function ON old_function.id = detail.function_id
                    WHERE old_function.name_en = 'permission_profiles'
                  );

                INSERT INTO permission_details (permission_id, function_id, "action")
                SELECT old_detail.permission_id, current_function.id, old_detail."action"
                FROM permission_details AS old_detail
                JOIN functions AS old_function ON old_function.id = old_detail.function_id
                JOIN functions AS current_function ON current_function.name_en = 'permissions'
                WHERE old_function.name_en = 'permission_profiles'
                ON CONFLICT (permission_id, function_id, "action") DO NOTHING;

                DELETE FROM permission_details
                WHERE function_id IN (SELECT id FROM functions WHERE name_en = 'permission_profiles');

                DELETE FROM functions WHERE name_en = 'permission_profiles';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO functions (name_vn, name_en, actions)
                VALUES ('Quản lý nhóm quyền', 'permission_profiles', ARRAY['read', 'create', 'update', 'delete'])
                ON CONFLICT (name_en) DO UPDATE
                SET name_vn = EXCLUDED.name_vn,
                    actions = EXCLUDED.actions;

                INSERT INTO permission_details (permission_id, function_id, "action")
                SELECT current_detail.permission_id, old_function.id, current_detail."action"
                FROM permission_details AS current_detail
                JOIN functions AS current_function ON current_function.id = current_detail.function_id
                JOIN functions AS old_function ON old_function.name_en = 'permission_profiles'
                WHERE current_function.name_en = 'permissions'
                ON CONFLICT (permission_id, function_id, "action") DO NOTHING;
                """);
        }
    }
}
