using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager_api.Migrations
{
    /// <inheritdoc />
    public partial class ver65 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "board_column",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "board",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "board_column");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "board");
        }
    }
}
