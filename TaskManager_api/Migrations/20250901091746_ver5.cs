using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager_api.Migrations
{
    /// <inheritdoc />
    public partial class ver5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "device_id",
                table: "refresh_token",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "device_name",
                table: "refresh_token",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "device_id",
                table: "refresh_token");

            migrationBuilder.DropColumn(
                name: "device_name",
                table: "refresh_token");
        }
    }
}
