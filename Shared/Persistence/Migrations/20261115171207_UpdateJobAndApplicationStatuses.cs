using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetSkeleton.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateJobAndApplicationStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE jobs
                SET status = 'ACCEPTED'
                WHERE status = 'FULL';

                UPDATE applications
                SET status = 'ACCEPTED'
                WHERE status = 'IN_PROGRESS';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
