using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetSkeleton.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFunctionCatalogFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_functions_NameEn",
                table: "functions");

            migrationBuilder.RenameColumn(
                name: "name_vn",
                table: "functions",
                newName: "name_vi");

            migrationBuilder.RenameColumn(
                name: "actions",
                table: "functions",
                newName: "available_actions");

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "functions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.Sql("""
                DELETE FROM permission_details AS duplicate
                USING permission_details AS retained
                WHERE duplicate.id > retained.id
                  AND duplicate.permission_id = retained.permission_id
                  AND duplicate.function_id = retained.function_id
                  AND UPPER(duplicate."action") = UPPER(retained."action");

                UPDATE permission_details
                SET "action" = UPPER("action");

                UPDATE functions
                SET code = UPPER(name_en),
                    name_en = CASE UPPER(name_en)
                        WHEN 'USERS' THEN 'User Management'
                        WHEN 'JOB_CATEGORIES' THEN 'Job Category Management'
                        WHEN 'SKILLS' THEN 'Skill Management'
                        WHEN 'VERIFICATION_REQUESTS' THEN 'Verification Request Management'
                        WHEN 'PERMISSIONS' THEN 'Permission Management'
                        ELSE name_en
                    END,
                    available_actions = ARRAY(
                        SELECT UPPER(action_value)
                        FROM UNNEST(available_actions) AS action_item(action_value));
                """);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "functions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_functions_code",
                table: "functions",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_functions_code",
                table: "functions");

            migrationBuilder.Sql("""
                UPDATE permission_details
                SET "action" = LOWER("action");

                UPDATE functions
                SET name_en = LOWER(code),
                    available_actions = ARRAY(
                        SELECT LOWER(action_value)
                        FROM UNNEST(available_actions) AS action_item(action_value));
                """);

            migrationBuilder.DropColumn(
                name: "code",
                table: "functions");

            migrationBuilder.RenameColumn(
                name: "name_vi",
                table: "functions",
                newName: "name_vn");

            migrationBuilder.RenameColumn(
                name: "available_actions",
                table: "functions",
                newName: "actions");

            migrationBuilder.CreateIndex(
                name: "IX_functions_NameEn",
                table: "functions",
                column: "name_en",
                unique: true);
        }
    }
}
