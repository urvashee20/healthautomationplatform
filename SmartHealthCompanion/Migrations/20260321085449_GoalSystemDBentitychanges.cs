using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHealthCompanion.Migrations
{
    /// <inheritdoc />
    public partial class GoalSystemDBentitychanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recommendations");

            migrationBuilder.DropColumn(
                name: "GoalType",
                table: "Goals");

            migrationBuilder.RenameColumn(
                name: "MiddleName",
                table: "UserProfiles",
                newName: "HealthConditions");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Goals",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "FoodPreference",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "SleepHours",
                table: "UserProfiles",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomGoalText",
                table: "Goals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationInDays",
                table: "Goals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasGymAccess",
                table: "Goals",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HealthConditions",
                table: "Goals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomGoal",
                table: "Goals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryGoal",
                table: "Goals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryGoals",
                table: "Goals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SleepHours",
                table: "Goals",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AIPlan",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<long>(type: "bigint", nullable: false),
                    DietPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkoutPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WaterPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SleepPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AIPlan_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HealthRisk",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<long>(type: "bigint", nullable: false),
                    RiskType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Suggestion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthRisk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthRisk_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIPlan_UserProfileId",
                table: "AIPlan",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthRisk_UserProfileId",
                table: "HealthRisk",
                column: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AIPlan");

            migrationBuilder.DropTable(
                name: "HealthRisk");

            migrationBuilder.DropColumn(
                name: "FoodPreference",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "SleepHours",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "CustomGoalText",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "DurationInDays",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "HasGymAccess",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "HealthConditions",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "IsCustomGoal",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "PrimaryGoal",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "SecondaryGoals",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "SleepHours",
                table: "Goals");

            migrationBuilder.RenameColumn(
                name: "HealthConditions",
                table: "UserProfiles",
                newName: "MiddleName");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Goals",
                newName: "StartDate");

            migrationBuilder.AddColumn<string>(
                name: "GoalType",
                table: "Goals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DietPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneratedByAI = table.Column<bool>(type: "bit", nullable: true),
                    RecommendationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    WaterSuggestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkoutPlan = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "IX_Recommendations_UserProfileId",
                table: "Recommendations",
                column: "UserProfileId");
        }
    }
}
