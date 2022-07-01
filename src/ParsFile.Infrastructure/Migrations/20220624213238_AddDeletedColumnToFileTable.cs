using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsFile.Infrastructure.Migrations
{
    public partial class AddDeletedColumnToFileTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "File",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "File");
        }
    }
}
