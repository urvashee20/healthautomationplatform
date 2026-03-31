using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHealthCompanion.Migrations
{
    /// <inheritdoc />
    public partial class NewAIChatEntityADD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatSession_AIPlan_AIPlanId",
                table: "ChatSession");

            migrationBuilder.DropIndex(
                name: "IX_ChatSession_AIPlanId",
                table: "ChatSession");

            migrationBuilder.DropColumn(
                name: "AIPlanId",
                table: "ChatSession");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AIPlanId",
                table: "ChatSession",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatSession_AIPlanId",
                table: "ChatSession",
                column: "AIPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatSession_AIPlan_AIPlanId",
                table: "ChatSession",
                column: "AIPlanId",
                principalTable: "AIPlan",
                principalColumn: "Id");
        }
    }
}
