using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class updateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6284ff6f-ca8f-4d66-b3a8-39de05a722dc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "764f16a5-9484-45f8-a0fa-1948745ded0e");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "401443be-7a21-441b-8cc1-836abec19d56", null, "User", "USER" },
                    { "fcaaa7fd-3afa-4506-a644-f073c3f43066", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "401443be-7a21-441b-8cc1-836abec19d56");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fcaaa7fd-3afa-4506-a644-f073c3f43066");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6284ff6f-ca8f-4d66-b3a8-39de05a722dc", null, "Admin", "ADMIN" },
                    { "764f16a5-9484-45f8-a0fa-1948745ded0e", null, "User", "USER" }
                });
        }
    }
}
