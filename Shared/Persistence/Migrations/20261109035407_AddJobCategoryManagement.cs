using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DotNetSkeleton.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddJobCategoryManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "job_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    icon = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    parent_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_categories_job_categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "job_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_job_categories_parent_id",
                table: "job_categories",
                column: "parent_id");

            migrationBuilder.Sql("""
                INSERT INTO functions (name_vn, name_en, actions)
                VALUES ('Quản lý lĩnh vực', 'job_categories', ARRAY['read', 'create', 'update', 'delete'])
                ON CONFLICT (name_en) DO UPDATE
                SET name_vn = EXCLUDED.name_vn,
                    actions = EXCLUDED.actions;

                INSERT INTO permission_details (permission_id, function_id, "action")
                SELECT permission.id, function_catalog.id, action_grant."action"
                FROM permissions AS permission
                JOIN functions AS function_catalog ON function_catalog.name_en = 'job_categories'
                CROSS JOIN (VALUES ('read'), ('create'), ('update'), ('delete')) AS action_grant("action")
                WHERE permission.name = 'ADMIN_DEFAULT'
                ON CONFLICT (permission_id, function_id, action) DO NOTHING;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM permission_details
                WHERE function_id = (SELECT id FROM functions WHERE name_en = 'job_categories');

                DELETE FROM functions WHERE name_en = 'job_categories';
                """);

            migrationBuilder.DropTable(
                name: "job_categories");
        }
    }
}
