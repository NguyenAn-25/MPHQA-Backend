using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetSkeleton.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddJobStatusAndPricingModelEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE jobs
                SET pricing_model = CASE pricing_model
                    WHEN 'COIN' THEN 'PER_CONNECTION'
                    WHEN 'PACKAGE' THEN 'TIME_BASED'
                    ELSE pricing_model
                END,
                status = CASE status
                    WHEN 'EXPIRED' THEN 'CANCELLED'
                    ELSE status
                END;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "jobs",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "pricing_model",
                table: "jobs",
                type: "character varying(24)",
                maxLength: 24,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "jobs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "pricing_model",
                table: "jobs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(24)",
                oldMaxLength: 24);
        }
    }
}
