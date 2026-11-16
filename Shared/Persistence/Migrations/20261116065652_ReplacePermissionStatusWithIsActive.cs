using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetSkeleton.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplacePermissionStatusWithIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "permissions",
                type: "boolean",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE permissions
                SET is_active = (UPPER(status) = 'ACTIVE');
                """);

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "permissions",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "status",
                table: "permissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "permissions",
                type: "character varying(24)",
                maxLength: 24,
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE permissions
                SET status = CASE WHEN is_active THEN 'ACTIVE' ELSE 'INACTIVE' END;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "permissions",
                type: "character varying(24)",
                maxLength: 24,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(24)",
                oldMaxLength: 24,
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "permissions");
        }
    }
}
