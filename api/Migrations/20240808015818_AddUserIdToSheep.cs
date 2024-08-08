using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToSheep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50be6aaa-9d2b-4169-85e7-a771ac5d473f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b5222c13-e1f2-4ac2-ae91-73b0a2dbba64");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Sheeps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6284ff6f-ca8f-4d66-b3a8-39de05a722dc", null, "Admin", "ADMIN" },
                    { "764f16a5-9484-45f8-a0fa-1948745ded0e", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6284ff6f-ca8f-4d66-b3a8-39de05a722dc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "764f16a5-9484-45f8-a0fa-1948745ded0e");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Sheeps");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "50be6aaa-9d2b-4169-85e7-a771ac5d473f", null, "User", "USER" },
                    { "b5222c13-e1f2-4ac2-ae91-73b0a2dbba64", null, "Admin", "ADMIN" }
                });
        }
    }
}
