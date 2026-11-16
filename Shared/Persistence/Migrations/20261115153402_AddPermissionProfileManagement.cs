using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetSkeleton.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionProfileManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO functions (name_vn, name_en, actions)
                VALUES ('Quản lý nhóm quyền', 'permissions', ARRAY['read', 'create', 'update', 'delete'])
                ON CONFLICT (name_en) DO UPDATE
                SET name_vn = EXCLUDED.name_vn,
                    actions = EXCLUDED.actions;

                INSERT INTO permission_details (permission_id, function_id, "action")
                SELECT permission.id, function_catalog.id, action_grant."action"
                FROM permissions AS permission
                JOIN functions AS function_catalog ON function_catalog.name_en = 'permissions'
                CROSS JOIN (VALUES ('read'), ('create'), ('update'), ('delete')) AS action_grant("action")
                WHERE permission.name = 'ADMIN_DEFAULT'
                ON CONFLICT (permission_id, function_id, "action") DO NOTHING;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM permission_details
                WHERE function_id = (SELECT id FROM functions WHERE name_en = 'permissions');

                DELETE FROM functions WHERE name_en = 'permissions';
                """);
        }
    }
}
