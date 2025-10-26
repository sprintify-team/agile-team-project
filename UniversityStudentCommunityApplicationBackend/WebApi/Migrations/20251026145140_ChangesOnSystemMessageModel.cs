using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangesOnSystemMessageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SystemMessages");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "SystemMessages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "SystemMessages",
                columns: new[] { "Id", "Code", "Message" },
                values: new object[] { 1, "COMING_SOON", "Çok yakında hizmetinizdeyiz!" });

            migrationBuilder.CreateIndex(
                name: "IX_SystemMessages_Code",
                table: "SystemMessages",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SystemMessages_Code",
                table: "SystemMessages");

            migrationBuilder.DeleteData(
                table: "SystemMessages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Code",
                table: "SystemMessages");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SystemMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
