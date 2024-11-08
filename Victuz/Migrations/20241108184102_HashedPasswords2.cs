using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Victuz.Migrations
{
    /// <inheritdoc />
    public partial class HashedPasswords2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEBvheVwG4KNnrDnEPSA0V126sHMEXyRza1BFh8fw9M/IHFui7ETKRuHJL6lQp6OUWA==");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEBvheVwG4KNnrDnEPSA0V126sHMEXyRza1BFh8fw9M/IHFui7ETKRuHJL6lQp6OUWA==");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEBvheVwG4KNnrDnEPSA0V126sHMEXyRza1BFh8fw9M/IHFui7ETKRuHJL6lQp6OUWA==");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 4,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEBvheVwG4KNnrDnEPSA0V126sHMEXyRza1BFh8fw9M/IHFui7ETKRuHJL6lQp6OUWA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "123");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Password",
                value: "123");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "Password",
                value: "123");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 4,
                column: "Password",
                value: "123");
        }
    }
}
