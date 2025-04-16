using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Department", "FullName", "JoiningDate" },
                values: new object[,]
                {
                    { 1, "Informatique", "Aziz Ben Hmida", new DateTime(2020, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Comptabilité", "Ahmed Gharbi", new DateTime(2019, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Ressources Humaines", "Ons Trabelsi", new DateTime(2021, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "LeaveRequests",
                columns: new[] { "Id", "CreatedAt", "EmployeeId", "EndDate", "LeaveType", "Reason", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2023, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Vacances à Hammamet", new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, new DateTime(2023, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2023, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Grippe saisonnière", new DateTime(2023, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, new DateTime(2023, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2023, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Voyage longue durée", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 4, new DateTime(2023, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2023, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Urgence familiale", new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 5, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2023, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Extraction dentaire", new DateTime(2023, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
