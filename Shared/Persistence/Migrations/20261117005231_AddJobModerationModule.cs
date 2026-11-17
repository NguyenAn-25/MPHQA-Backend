using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DotNetSkeleton.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddJobModerationModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "post_packages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    duration_days = table.Column<int>(type: "integer", nullable: false),
                    price_coin = table.Column<int>(type: "integer", nullable: false),
                    price_money = table.Column<decimal>(type: "numeric(15,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_packages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "jobs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    salary_value = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    salary_unit = table.Column<string>(type: "text", nullable: false),
                    total_salary_payment = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    total_platform_fee = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    total_expense = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    completed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    working_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    full_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    rejected_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    accepted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_paid = table.Column<bool>(type: "boolean", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    lng = table.Column<decimal>(type: "numeric(10,7)", nullable: true),
                    lat = table.Column<decimal>(type: "numeric(10,7)", nullable: true),
                    cancelled_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    required_number_of_workers = table.Column<int>(type: "integer", nullable: false),
                    expired_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    time_start_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    time_end_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    rejection_reason = table.Column<string>(type: "text", nullable: true),
                    cancel_reason = table.Column<string>(type: "text", nullable: true),
                    required_description = table.Column<string>(type: "text", nullable: true),
                    auto_reply_message = table.Column<string>(type: "text", nullable: true),
                    pricing_model = table.Column<string>(type: "text", nullable: false),
                    post_fee_coin = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    job_category_id = table.Column<int>(type: "integer", nullable: false),
                    post_package_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobs", x => x.id);
                    table.ForeignKey(
                        name: "FK_jobs_job_categories_job_category_id",
                        column: x => x.job_category_id,
                        principalTable: "job_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_jobs_post_packages_post_package_id",
                        column: x => x.post_package_id,
                        principalTable: "post_packages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_jobs_user_created_by",
                        column: x => x.created_by,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status = table.Column<string>(type: "text", nullable: false),
                    applied_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    connection_fee_coin = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    job_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applications", x => x.id);
                    table.ForeignKey(
                        name: "FK_applications_jobs_job_id",
                        column: x => x.job_id,
                        principalTable: "jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_applications_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_feedbacks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: true),
                    feedback_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    star = table.Column<int>(type: "integer", nullable: false),
                    image = table.Column<string>(type: "text", nullable: true),
                    reviewer_id = table.Column<int>(type: "integer", nullable: false),
                    reviewee_id = table.Column<int>(type: "integer", nullable: false),
                    job_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_feedbacks", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_feedbacks_jobs_job_id",
                        column: x => x.job_id,
                        principalTable: "jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_job_feedbacks_user_reviewee_id",
                        column: x => x.reviewee_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_job_feedbacks_user_reviewer_id",
                        column: x => x.reviewer_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "job_images",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    image = table.Column<string>(type: "text", nullable: false),
                    job_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_images", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_images_jobs_job_id",
                        column: x => x.job_id,
                        principalTable: "jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_required_skills",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_id = table.Column<int>(type: "integer", nullable: false),
                    skill_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_required_skills", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_required_skills_jobs_job_id",
                        column: x => x.job_id,
                        principalTable: "jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_job_required_skills_skills_skill_id",
                        column: x => x.skill_id,
                        principalTable: "skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "saved_jobs",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    job_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saved_jobs", x => new { x.user_id, x.job_id });
                    table.ForeignKey(
                        name: "FK_saved_jobs_jobs_job_id",
                        column: x => x.job_id,
                        principalTable: "jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_saved_jobs_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_applications_job_id",
                table: "applications",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_applications_user_id",
                table: "applications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_feedbacks_job_id",
                table: "job_feedbacks",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_feedbacks_reviewee_id",
                table: "job_feedbacks",
                column: "reviewee_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_feedbacks_reviewer_id",
                table: "job_feedbacks",
                column: "reviewer_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_images_job_id",
                table: "job_images",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_required_skills_job_id_skill_id",
                table: "job_required_skills",
                columns: new[] { "job_id", "skill_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_job_required_skills_skill_id",
                table: "job_required_skills",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_created_by",
                table: "jobs",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_job_category_id",
                table: "jobs",
                column: "job_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_post_package_id",
                table: "jobs",
                column: "post_package_id");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_status_created_at",
                table: "jobs",
                columns: new[] { "status", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_saved_jobs_job_id",
                table: "saved_jobs",
                column: "job_id");

            migrationBuilder.Sql("""
                INSERT INTO functions (code, name_vi, name_en, actions)
                VALUES (
                    'JOBS',
                    'Quản lý công việc',
                    'Job Management',
                    ARRAY['READ', 'UPDATE']::text[])
                ON CONFLICT (code) DO UPDATE
                SET name_vi = EXCLUDED.name_vi,
                    name_en = EXCLUDED.name_en,
                    actions = EXCLUDED.actions;

                INSERT INTO permission_details (permission_id, function_id, "action")
                SELECT permission.id, function_catalog.id, action_grant."action"
                FROM permissions AS permission
                JOIN functions AS function_catalog ON function_catalog.code = 'JOBS'
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
                WHERE function_id = (SELECT id FROM functions WHERE code = 'JOBS');

                DELETE FROM functions WHERE code = 'JOBS';
                """);

            migrationBuilder.DropTable(
                name: "applications");

            migrationBuilder.DropTable(
                name: "job_feedbacks");

            migrationBuilder.DropTable(
                name: "job_images");

            migrationBuilder.DropTable(
                name: "job_required_skills");

            migrationBuilder.DropTable(
                name: "saved_jobs");

            migrationBuilder.DropTable(
                name: "jobs");

            migrationBuilder.DropTable(
                name: "post_packages");
        }
    }
}
