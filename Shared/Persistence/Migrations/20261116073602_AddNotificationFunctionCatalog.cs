using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetSkeleton.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationFunctionCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO functions (code, name_vi, name_en, actions)
                VALUES (
                    'NOTIFICATIONS',
                    'Quản lý thông báo',
                    'Notification Management',
                    ARRAY['READ', 'CREATE']::text[])
                ON CONFLICT (code) DO UPDATE
                SET name_vi = EXCLUDED.name_vi,
                    name_en = EXCLUDED.name_en,
                    actions = EXCLUDED.actions;

                INSERT INTO permission_details (permission_id, function_id, "action")
                SELECT permission.id, function_catalog.id, action_grant."action"
                FROM permissions AS permission
                JOIN functions AS function_catalog ON function_catalog.code = 'NOTIFICATIONS'
                CROSS JOIN (VALUES ('READ'), ('CREATE')) AS action_grant("action")
                WHERE permission.name = 'ADMIN_DEFAULT'
                ON CONFLICT (permission_id, function_id, "action") DO NOTHING;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM permission_details
                WHERE function_id = (SELECT id FROM functions WHERE code = 'NOTIFICATIONS');

                DELETE FROM functions WHERE code = 'NOTIFICATIONS';
                """);
        }
    }
}
