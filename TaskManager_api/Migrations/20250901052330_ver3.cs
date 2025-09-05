using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager_api.Migrations
{
    /// <inheritdoc />
    public partial class ver3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "task");

            migrationBuilder.AddColumn<string>(
                name: "avatar_url",
                table: "user",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bio",
                table: "user",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "column_id",
                table: "task",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar_url",
                table: "user");

            migrationBuilder.DropColumn(
                name: "bio",
                table: "user");

            migrationBuilder.AlterColumn<int>(
                name: "column_id",
                table: "task",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "task",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
