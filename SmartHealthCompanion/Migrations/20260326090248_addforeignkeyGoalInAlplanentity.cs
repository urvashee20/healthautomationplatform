using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHealthCompanion.Migrations
{
    /// <inheritdoc />
    public partial class addforeignkeyGoalInAlplanentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GoalId",
                table: "AIPlan",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AIPlan_GoalId",
                table: "AIPlan",
                column: "GoalId",
                unique: true,
                filter: "[GoalId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AIPlan_Goals_GoalId",
                table: "AIPlan",
                column: "GoalId",
                principalTable: "Goals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AIPlan_Goals_GoalId",
                table: "AIPlan");

            migrationBuilder.DropIndex(
                name: "IX_AIPlan_GoalId",
                table: "AIPlan");

            migrationBuilder.DropColumn(
                name: "GoalId",
                table: "AIPlan");
        }
    }
}
