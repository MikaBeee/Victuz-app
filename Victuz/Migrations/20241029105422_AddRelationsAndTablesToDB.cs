using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Victuz.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationsAndTablesToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categorie",
                columns: table => new
                {
                    CatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorie", x => x.CatId);
                });

            migrationBuilder.CreateTable(
                name: "forum",
                columns: table => new
                {
                    ForumId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forum", x => x.ForumId);
                });

            migrationBuilder.CreateTable(
                name: "location",
                columns: table => new
                {
                    LocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_location", x => x.LocId);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "gathering",
                columns: table => new
                {
                    GatheringId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GatheringTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GatheringDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxParticipants = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gathering", x => x.GatheringId);
                    table.ForeignKey(
                        name: "FK_gathering_categorie_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "categorie",
                        principalColumn: "CatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gathering_location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "location",
                        principalColumn: "LocId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_users_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "activitieregistration",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GatheringId = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activitieregistration", x => new { x.UserId, x.GatheringId });
                    table.ForeignKey(
                        name: "FK_activitieregistration_gathering_GatheringId",
                        column: x => x.GatheringId,
                        principalTable: "gathering",
                        principalColumn: "GatheringId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_activitieregistration_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ForumId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_post_forum_ForumId",
                        column: x => x.ForumId,
                        principalTable: "forum",
                        principalColumn: "ForumId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_post_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_activitieregistration_GatheringId",
                table: "activitieregistration",
                column: "GatheringId");

            migrationBuilder.CreateIndex(
                name: "IX_gathering_CategoryId",
                table: "gathering",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_gathering_LocationId",
                table: "gathering",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_post_ForumId",
                table: "post",
                column: "ForumId");

            migrationBuilder.CreateIndex(
                name: "IX_post_UserId",
                table: "post",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_RoleId",
                table: "users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activitieregistration");

            migrationBuilder.DropTable(
                name: "post");

            migrationBuilder.DropTable(
                name: "gathering");

            migrationBuilder.DropTable(
                name: "forum");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "categorie");

            migrationBuilder.DropTable(
                name: "location");

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
