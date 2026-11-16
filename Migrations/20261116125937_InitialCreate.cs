using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DotNetSkeleton.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Examples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examples", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "functions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name_vi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name_en = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    actions = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_functions", x => x.id);
                });

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

            migrationBuilder.CreateTable(
                name: "payment_methods",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    icon = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_methods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.id);
                });

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

            migrationBuilder.CreateTable(
                name: "permission_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    permission_id = table.Column<int>(type: "integer", nullable: false),
                    function_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_permission_details_functions_function_id",
                        column: x => x.function_id,
                        principalTable: "functions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_permission_details_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    public_id = table.Column<Guid>(type: "uuid", nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    password = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_verified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    verified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    identity_verified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    identity_rejected_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_active_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    role = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    permission_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                    table.CheckConstraint("CK_user_role_permission", "(role = 'USER' AND permission_id IS NULL) OR (role = 'ADMIN' AND permission_id IS NOT NULL)");
                    table.CheckConstraint("CK_user_verification_state", "(is_verified = TRUE AND verified_at IS NOT NULL) OR (is_verified = FALSE AND verified_at IS NULL)");
                    table.ForeignKey(
                        name: "FK_user_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "auth_otps",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    public_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    purpose = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    failed_attempts = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    expired_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    verified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auth_otps", x => x.id);
                    table.ForeignKey(
                        name: "FK_auth_otps_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "auth_sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    refresh_token = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    expired_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    revoked_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auth_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_auth_sessions_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "connection_fee_settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fee_coin = table.Column<int>(type: "integer", nullable: false),
                    effective_from = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_connection_fee_settings", x => x.id);
                    table.ForeignKey(
                        name: "FK_connection_fee_settings_user_created_by",
                        column: x => x.created_by,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conversations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_a_id = table.Column<int>(type: "integer", nullable: false),
                    user_b_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversations", x => x.id);
                    table.ForeignKey(
                        name: "FK_conversations_user_user_a_id",
                        column: x => x.user_a_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_conversations_user_user_b_id",
                        column: x => x.user_b_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    required_description = table.Column<string>(type: "text", nullable: true),
                    auto_reply_message = table.Column<string>(type: "text", nullable: true),
                    pricing_model = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_jobs_post_packages_post_package_id",
                        column: x => x.post_package_id,
                        principalTable: "post_packages",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_jobs_user_created_by",
                        column: x => x.created_by,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    content = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    send_to = table.Column<string>(type: "text", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_notifications_user_created_by",
                        column: x => x.created_by,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "profiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    note = table.Column<string>(type: "text", nullable: true),
                    major = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<string>(type: "text", nullable: true),
                    avatar = table.Column<string>(type: "text", nullable: true),
                    fullname = table.Column<string>(type: "text", nullable: false),
                    birthdate = table.Column<DateOnly>(type: "date", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    lng = table.Column<decimal>(type: "numeric(10,7)", nullable: true),
                    lat = table.Column<decimal>(type: "numeric(10,7)", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profiles", x => x.id);
                    table.ForeignKey(
                        name: "FK_profiles_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "verification_requests",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    full_name_on_card = table.Column<string>(type: "text", nullable: false),
                    id_number = table.Column<string>(type: "text", nullable: false),
                    card_front_image = table.Column<string>(type: "text", nullable: false),
                    card_back_image = table.Column<string>(type: "text", nullable: false),
                    portrait_image = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    rejection_reason = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    verified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    rejected_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    sender_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_verification_requests", x => x.id);
                    table.ForeignKey(
                        name: "FK_verification_requests_user_sender_id",
                        column: x => x.sender_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "wallets",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    balance = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallets", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_wallets_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    reply_to_message_id = table.Column<int>(type: "integer", nullable: true),
                    sender_id = table.Column<int>(type: "integer", nullable: false),
                    conversation_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_messages_conversations_conversation_id",
                        column: x => x.conversation_id,
                        principalTable: "conversations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_messages_messages_reply_to_message_id",
                        column: x => x.reply_to_message_id,
                        principalTable: "messages",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_messages_user_sender_id",
                        column: x => x.sender_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_job_feedbacks_user_reviewer_id",
                        column: x => x.reviewer_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "notification_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    read_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    notification_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_notification_details_notifications_notification_id",
                        column: x => x.notification_id,
                        principalTable: "notifications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_notification_details_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "profile_identities",
                columns: table => new
                {
                    profile_id = table.Column<int>(type: "integer", nullable: false),
                    card_id = table.Column<string>(type: "text", nullable: false),
                    card_front_image = table.Column<string>(type: "text", nullable: false),
                    card_back_image = table.Column<string>(type: "text", nullable: false),
                    full_name_on_card = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profile_identities", x => x.profile_id);
                    table.ForeignKey(
                        name: "FK_profile_identities_profiles_profile_id",
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

            migrationBuilder.CreateTable(
                name: "topup_orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    coin_amount = table.Column<int>(type: "integer", nullable: false),
                    money_amount = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    order_code = table.Column<string>(type: "text", nullable: false),
                    paid_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    transaction_id = table.Column<string>(type: "text", nullable: true),
                    wallet_id = table.Column<int>(type: "integer", nullable: false),
                    payment_method_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topup_orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_topup_orders_payment_methods_payment_method_id",
                        column: x => x.payment_method_id,
                        principalTable: "payment_methods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_topup_orders_wallets_wallet_id",
                        column: x => x.wallet_id,
                        principalTable: "wallets",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "wallet_transactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    coin_amount = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    balance_after = table.Column<int>(type: "integer", nullable: false),
                    object_type = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    object_id = table.Column<int>(type: "integer", nullable: true),
                    type = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    wallet_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_transactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_wallet_transactions_wallets_wallet_id",
                        column: x => x.wallet_id,
                        principalTable: "wallets",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "message_images",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    image = table.Column<string>(type: "text", nullable: false),
                    message_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message_images", x => x.id);
                    table.ForeignKey(
                        name: "FK_message_images_messages_message_id",
                        column: x => x.message_id,
                        principalTable: "messages",
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
                name: "IX_auth_otps_public_id",
                table: "auth_otps",
                column: "public_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_auth_otps_user_id_purpose_created_at",
                table: "auth_otps",
                columns: new[] { "user_id", "purpose", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_auth_sessions_refresh_token",
                table: "auth_sessions",
                column: "refresh_token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_auth_sessions_user_id",
                table: "auth_sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_connection_fee_settings_created_by",
                table: "connection_fee_settings",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_conversations_user_a_id",
                table: "conversations",
                column: "user_a_id");

            migrationBuilder.CreateIndex(
                name: "IX_conversations_user_b_id",
                table: "conversations",
                column: "user_b_id");

            migrationBuilder.CreateIndex(
                name: "IX_functions_code",
                table: "functions",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_job_categories_parent_id",
                table: "job_categories",
                column: "parent_id");

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
                name: "IX_job_required_skills_job_id",
                table: "job_required_skills",
                column: "job_id");

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
                name: "IX_message_images_message_id",
                table: "message_images",
                column: "message_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_conversation_id",
                table: "messages",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_reply_to_message_id",
                table: "messages",
                column: "reply_to_message_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_sender_id",
                table: "messages",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_details_notification_id_user_id",
                table: "notification_details",
                columns: new[] { "notification_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notification_details_user_id",
                table: "notification_details",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_created_by",
                table: "notifications",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_permission_details_function_id",
                table: "permission_details",
                column: "function_id");

            migrationBuilder.CreateIndex(
                name: "IX_permission_details_permission_id_function_id_action",
                table: "permission_details",
                columns: new[] { "permission_id", "function_id", "action" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_permissions_name",
                table: "permissions",
                column: "name",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_profiles_user_id",
                table: "profiles",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reports_sender_id",
                table: "reports",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_saved_jobs_job_id",
                table: "saved_jobs",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_topup_orders_payment_method_id",
                table: "topup_orders",
                column: "payment_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_topup_orders_wallet_id",
                table: "topup_orders",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_permission_id",
                table: "user",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_phone",
                table: "user",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_public_id",
                table: "user",
                column: "public_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_verification_requests_sender_id_pending",
                table: "verification_requests",
                column: "sender_id",
                unique: true,
                filter: "\"status\" = 'PENDING'");

            migrationBuilder.CreateIndex(
                name: "IX_verification_requests_status_created_at",
                table: "verification_requests",
                columns: new[] { "status", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_transactions_wallet_id",
                table: "wallet_transactions",
                column: "wallet_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applications");

            migrationBuilder.DropTable(
                name: "auth_otps");

            migrationBuilder.DropTable(
                name: "auth_sessions");

            migrationBuilder.DropTable(
                name: "connection_fee_settings");

            migrationBuilder.DropTable(
                name: "Examples");

            migrationBuilder.DropTable(
                name: "job_feedbacks");

            migrationBuilder.DropTable(
                name: "job_images");

            migrationBuilder.DropTable(
                name: "job_required_skills");

            migrationBuilder.DropTable(
                name: "message_images");

            migrationBuilder.DropTable(
                name: "notification_details");

            migrationBuilder.DropTable(
                name: "permission_details");

            migrationBuilder.DropTable(
                name: "profile_experiences");

            migrationBuilder.DropTable(
                name: "profile_identities");

            migrationBuilder.DropTable(
                name: "profile_skills");

            migrationBuilder.DropTable(
                name: "reports");

            migrationBuilder.DropTable(
                name: "saved_jobs");

            migrationBuilder.DropTable(
                name: "topup_orders");

            migrationBuilder.DropTable(
                name: "verification_requests");

            migrationBuilder.DropTable(
                name: "wallet_transactions");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "functions");

            migrationBuilder.DropTable(
                name: "profiles");

            migrationBuilder.DropTable(
                name: "skills");

            migrationBuilder.DropTable(
                name: "jobs");

            migrationBuilder.DropTable(
                name: "payment_methods");

            migrationBuilder.DropTable(
                name: "wallets");

            migrationBuilder.DropTable(
                name: "conversations");

            migrationBuilder.DropTable(
                name: "job_categories");

            migrationBuilder.DropTable(
                name: "post_packages");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "permissions");
        }
    }
}
