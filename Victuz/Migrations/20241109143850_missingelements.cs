using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Victuz.Migrations
{
    /// <inheritdoc />
    public partial class missingelements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "role",
                keyColumn: "RoleId",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[] { 3, "guest" });
        }
    }
}
