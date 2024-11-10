using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Victuz.Migrations
{
    /// <inheritdoc />
    public partial class updatepwminchar8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEB/mvjZCAt7PHl3COgHBFEjHDrFSZxDjJee6ReypuN148BI7ON8V1RvJtUPXugNBhg==");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEB/mvjZCAt7PHl3COgHBFEjHDrFSZxDjJee6ReypuN148BI7ON8V1RvJtUPXugNBhg==");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEB/mvjZCAt7PHl3COgHBFEjHDrFSZxDjJee6ReypuN148BI7ON8V1RvJtUPXugNBhg==");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 4,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEB/mvjZCAt7PHl3COgHBFEjHDrFSZxDjJee6ReypuN148BI7ON8V1RvJtUPXugNBhg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
