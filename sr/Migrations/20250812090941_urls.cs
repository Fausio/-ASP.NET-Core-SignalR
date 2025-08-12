using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sr.Migrations
{
    public partial class urls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "url",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "url",
                table: "Notifications");
        }
    }
}
