using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHealthCompanion.Migrations
{
    /// <inheritdoc />
    public partial class SmartHealthCompanionProjectDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobStartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JobEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DailyRoutine = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BehaviorLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FollowedDiet = table.Column<bool>(type: "bit", nullable: true),
                    CompletedWorkout = table.Column<bool>(type: "bit", nullable: true),
                    WaterGoalMet = table.Column<bool>(type: "bit", nullable: true),
                    SleepHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UserProfileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviorLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BehaviorLogs_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BreakfastCalories = table.Column<int>(type: "int", nullable: false),
                    LunchCalories = table.Column<int>(type: "int", nullable: false),
                    DinnerCalories = table.Column<int>(type: "int", nullable: false),
                    SnacksCalories = table.Column<int>(type: "int", nullable: false),
                    WaterIntake = table.Column<int>(type: "int", nullable: false),
                    Steps = table.Column<int>(type: "int", nullable: false),
                    OfficeHours = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCalories = table.Column<int>(type: "int", nullable: false),
                    BurnedCalories = table.Column<int>(type: "int", nullable: false),
                    NetCalories = table.Column<int>(type: "int", nullable: false),
                    UserProfileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyLogs_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<long>(type: "bigint", nullable: false),
                    GoalType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goals_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserProfileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RecommendationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DietPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkoutPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WaterSuggestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneratedByAI = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserProfileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recommendations_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BehaviorLogs_UserProfileId",
                table: "BehaviorLogs",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyLogs_UserProfileId",
                table: "DailyLogs",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_UserProfileId",
                table: "Goals",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserProfileId",
                table: "Notifications",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_UserProfileId",
                table: "Recommendations",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BehaviorLogs");

            migrationBuilder.DropTable(
                name: "DailyLogs");

            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Recommendations");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
