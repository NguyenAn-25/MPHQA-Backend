using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DotNetSkeleton.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileExperienceAndSkillLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_verification_requests_sender_id",
                table: "verification_requests");

            migrationBuilder.CreateTable(
                name: "profile_experiences",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_title = table.Column<string>(type: "text", nullable: false),
                    company_name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "text", nullable: true),
                    profile_id = table.Column<int>(type: "integer", nullable: false),
                    job_category_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profile_experiences", x => x.id);
                    table.ForeignKey(
                        name: "FK_profile_experiences_job_categories_job_category_id",
                        column: x => x.job_category_id,
                        principalTable: "job_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_profile_experiences_profiles_profile_id",
                        column: x => x.profile_id,
                        principalTable: "profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "profile_skills",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    profile_id = table.Column<int>(type: "integer", nullable: false),
                    skill_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profile_skills", x => x.id);
                    table.ForeignKey(
                        name: "FK_profile_skills_profiles_profile_id",
                        column: x => x.profile_id,
                        principalTable: "profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_profile_skills_skills_skill_id",
                        column: x => x.skill_id,
                        principalTable: "skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_verification_requests_sender_id_pending",
                table: "verification_requests",
                column: "sender_id",
                unique: true,
                filter: "\"status\" = 'PENDING'");

            migrationBuilder.CreateIndex(
                name: "IX_profile_experiences_job_category_id",
                table: "profile_experiences",
                column: "job_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_profile_experiences_profile_id",
                table: "profile_experiences",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_profile_skills_profile_id_skill_id",
                table: "profile_skills",
                columns: new[] { "profile_id", "skill_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_profile_skills_skill_id",
                table: "profile_skills",
                column: "skill_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "profile_experiences");

            migrationBuilder.DropTable(
                name: "profile_skills");

            migrationBuilder.DropIndex(
                name: "IX_verification_requests_sender_id_pending",
                table: "verification_requests");

            migrationBuilder.CreateIndex(
                name: "IX_verification_requests_sender_id",
                table: "verification_requests",
                column: "sender_id");
        }
    }
}
