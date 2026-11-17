using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DotNetSkeleton.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddReportsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    object_type = table.Column<string>(type: "text", nullable: false),
                    object_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    processed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    title = table.Column<string>(type: "text", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    processed_note = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true),
                    sender_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reports", x => x.id);
                    table.ForeignKey(
                        name: "FK_reports_user_sender_id",
                        column: x => x.sender_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reports_object_type_object_id",
                table: "reports",
                columns: new[] { "object_type", "object_id" });

            migrationBuilder.CreateIndex(
                name: "IX_reports_sender_id",
                table: "reports",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_reports_status_created_at",
                table: "reports",
                columns: new[] { "status", "created_at" });

            migrationBuilder.Sql("""
                INSERT INTO functions (code, name_vi, name_en, actions)
                VALUES (
                    'REPORTS',
                    'Quản lý báo cáo',
                    'Report Management',
                    ARRAY['READ', 'UPDATE']::text[])
                ON CONFLICT (code) DO UPDATE
                SET name_vi = EXCLUDED.name_vi,
                    name_en = EXCLUDED.name_en,
                    actions = EXCLUDED.actions;

                INSERT INTO permission_details (permission_id, function_id, "action")
                SELECT permission.id, function_catalog.id, action_grant."action"
                FROM permissions AS permission
                JOIN functions AS function_catalog ON function_catalog.code = 'REPORTS'
                CROSS JOIN (VALUES ('READ'), ('UPDATE')) AS action_grant("action")
                WHERE permission.name = 'ADMIN_DEFAULT'
                ON CONFLICT (permission_id, function_id, "action") DO NOTHING;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM permission_details
                WHERE function_id = (SELECT id FROM functions WHERE code = 'REPORTS');

                DELETE FROM functions WHERE code = 'REPORTS';
                """);

            migrationBuilder.DropTable(
                name: "reports");
        }
    }
}
