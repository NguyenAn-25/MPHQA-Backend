using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DotNetSkeleton.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSkillManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "skills",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    icon = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_skills", x => x.id);
                });

            migrationBuilder.Sql("""
                INSERT INTO functions (name_vn, name_en, actions)
                VALUES ('Quản lý kỹ năng', 'skills', ARRAY['read', 'create', 'update', 'delete'])
                ON CONFLICT (name_en) DO UPDATE
                SET name_vn = EXCLUDED.name_vn,
                    actions = EXCLUDED.actions;

                INSERT INTO permission_details (permission_id, function_id, "action")
                SELECT permission.id, function_catalog.id, action_grant."action"
                FROM permissions AS permission
                JOIN functions AS function_catalog ON function_catalog.name_en = 'skills'
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
                WHERE function_id = (SELECT id FROM functions WHERE name_en = 'skills');

                DELETE FROM functions WHERE name_en = 'skills';
                """);

            migrationBuilder.DropTable(
                name: "skills");
        }
    }
}
