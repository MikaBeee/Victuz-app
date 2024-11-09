using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Victuz.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyErrorInVoteFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_votes_gathering_UserId",
                table: "votes");

            migrationBuilder.CreateIndex(
                name: "IX_votes_GatheringId",
                table: "votes",
                column: "GatheringId");

            migrationBuilder.AddForeignKey(
                name: "FK_votes_gathering_GatheringId",
                table: "votes",
                column: "GatheringId",
                principalTable: "gathering",
                principalColumn: "GatheringId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_votes_gathering_GatheringId",
                table: "votes");

            migrationBuilder.DropIndex(
                name: "IX_votes_GatheringId",
                table: "votes");

            migrationBuilder.AddForeignKey(
                name: "FK_votes_gathering_UserId",
                table: "votes",
                column: "UserId",
                principalTable: "gathering",
                principalColumn: "GatheringId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
