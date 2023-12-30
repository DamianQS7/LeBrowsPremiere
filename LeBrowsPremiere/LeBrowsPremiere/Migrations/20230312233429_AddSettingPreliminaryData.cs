using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeBrowsPremiere.Migrations
{
    /// <inheritdoc />
    public partial class AddSettingPreliminaryData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedDate", "UpdatedBy", "UpdatedDate", "Value" },
                values: new object[,]
                {
                    { 1, "AppointmentStartTime", "admin", new DateTime(2023, 3, 12, 19, 34, 29, 232, DateTimeKind.Local).AddTicks(7685), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1/1/0001 10:00:00 AM" },
                    { 2, "AppointmentEndTime", "admin", new DateTime(2023, 3, 12, 19, 34, 29, 232, DateTimeKind.Local).AddTicks(7715), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1/1/0001 4:00:00 PM" },
                    { 3, "AppointmentIntervalInMinutes", "admin", new DateTime(2023, 3, 12, 19, 34, 29, 232, DateTimeKind.Local).AddTicks(7718), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "30" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
