using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Victuz.Migrations
{
    /// <inheritdoc />
    public partial class AddedDataToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activitieregistration_gathering_GatheringId",
                table: "activitieregistration");

            migrationBuilder.DropForeignKey(
                name: "FK_activitieregistration_users_UserId",
                table: "activitieregistration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_activitieregistration",
                table: "activitieregistration");

            migrationBuilder.RenameTable(
                name: "activitieregistration",
                newName: "gatheringRegistration");

            migrationBuilder.RenameIndex(
                name: "IX_activitieregistration_GatheringId",
                table: "gatheringRegistration",
                newName: "IX_gatheringRegistration_GatheringId");

            migrationBuilder.AlterColumn<string>(
                name: "Photopath",
                table: "gathering",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_gatheringRegistration",
                table: "gatheringRegistration",
                columns: new[] { "UserId", "GatheringId" });

            migrationBuilder.InsertData(
                table: "categorie",
                columns: new[] { "CatName" },
                values: new object[,]
                {
                    { "feest" },
                    { "bijeenkomst" },
                    { "event" }
                });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "RoleName" },
                values: new object[,]
                {
                    { "admin" },
                    { "user" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Password", "RoleId", "UserName" },
                values: new object[,]
                {
                    { "123", 1, "admin" },
                    { "123", 2, "mika" },
                    { "123", 2, "sven" },
                    { "123", 2, "charlotte" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_gatheringRegistration_gathering_GatheringId",
                table: "gatheringRegistration",
                column: "GatheringId",
                principalTable: "gathering",
                principalColumn: "GatheringId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_gatheringRegistration_users_UserId",
                table: "gatheringRegistration",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_gatheringRegistration_gathering_GatheringId",
                table: "gatheringRegistration");

            migrationBuilder.DropForeignKey(
                name: "FK_gatheringRegistration_users_UserId",
                table: "gatheringRegistration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_gatheringRegistration",
                table: "gatheringRegistration");

            migrationBuilder.DeleteData(
                table: "categorie",
                keyColumn: "CatId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "categorie",
                keyColumn: "CatId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "categorie",
                keyColumn: "CatId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "role",
                keyColumn: "RoleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "role",
                keyColumn: "RoleId",
                keyValue: 2);

            migrationBuilder.RenameTable(
                name: "gatheringRegistration",
                newName: "activitieregistration");

            migrationBuilder.RenameIndex(
                name: "IX_gatheringRegistration_GatheringId",
                table: "activitieregistration",
                newName: "IX_activitieregistration_GatheringId");

            migrationBuilder.AlterColumn<string>(
                name: "Photopath",
                table: "gathering",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_activitieregistration",
                table: "activitieregistration",
                columns: new[] { "UserId", "GatheringId" });

            migrationBuilder.AddForeignKey(
                name: "FK_activitieregistration_gathering_GatheringId",
                table: "activitieregistration",
                column: "GatheringId",
                principalTable: "gathering",
                principalColumn: "GatheringId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_activitieregistration_users_UserId",
                table: "activitieregistration",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
