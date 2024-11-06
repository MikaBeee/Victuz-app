using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Victuz.Migrations
{
    /// <inheritdoc />
    public partial class DataInsertIntoDB2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "categorie",
                keyColumn: "CatId",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "categorie",
                keyColumn: "CatId",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "categorie",
                keyColumn: "CatId",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "UserId",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "UserId",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "UserId",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "UserId",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "role",
                keyColumn: "RoleId",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "role",
                keyColumn: "RoleId",
                keyValue: -1);

            migrationBuilder.InsertData(
                table: "categorie",
                columns: new[] { "CatId", "CatName" },
                values: new object[,]
                {
                    { 1, "feest" },
                    { 2, "bijeenkomst" },
                    { 3, "event" }
                });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "admin" },
                    { 2, "user" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "UserId", "Password", "RoleId", "UserName" },
                values: new object[,]
                {
                    { 1, "123", 1, "admin" },
                    { 2, "123", 2, "mika" },
                    { 3, "123", 2, "sven" },
                    { 4, "123", 2, "charlotte" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "categorie",
                columns: new[] { "CatId", "CatName" },
                values: new object[,]
                {
                    { -3, "event" },
                    { -2, "bijeenkomst" },
                    { -1, "feest" }
                });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { -2, "user" },
                    { -1, "admin" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "UserId", "Password", "RoleId", "UserName" },
                values: new object[,]
                {
                    { -4, "123", -2, "charlotte" },
                    { -3, "123", -2, "sven" },
                    { -2, "123", -2, "mika" },
                    { -1, "123", -1, "admin" }
                });
        }
    }
}
