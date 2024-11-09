using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Victuz.Migrations
{
    /// <inheritdoc />
    public partial class AddedVotesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuggested",
                table: "gathering",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "votes",
                columns: table => new
                {
                    GatheringId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_votes", x => new { x.UserId, x.GatheringId });
                    table.ForeignKey(
                        name: "FK_votes_gathering_UserId",
                        column: x => x.UserId,
                        principalTable: "gathering",
                        principalColumn: "GatheringId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_votes_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "votes");

            migrationBuilder.DropColumn(
                name: "IsSuggested",
                table: "gathering");
        }
    }
}
