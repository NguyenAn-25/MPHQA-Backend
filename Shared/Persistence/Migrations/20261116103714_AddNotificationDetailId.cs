using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DotNetSkeleton.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationDetailId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_notification_details",
                table: "notification_details");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "notification_details",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_notification_details",
                table: "notification_details",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_details_notification_id_user_id",
                table: "notification_details",
                columns: new[] { "notification_id", "user_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_notification_details",
                table: "notification_details");

            migrationBuilder.DropIndex(
                name: "IX_notification_details_notification_id_user_id",
                table: "notification_details");

            migrationBuilder.DropColumn(
                name: "id",
                table: "notification_details");

            migrationBuilder.AddPrimaryKey(
                name: "PK_notification_details",
                table: "notification_details",
                columns: new[] { "notification_id", "user_id" });
        }
    }
}
