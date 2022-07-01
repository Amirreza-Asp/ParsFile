using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsFile.Infrastructure.Migrations
{
    public partial class ChangeRelationTypeBetweenBasketAndUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Basket_UserName",
                table: "Basket");

            migrationBuilder.CreateIndex(
                name: "IX_Basket_UserName",
                table: "Basket",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Basket_UserName",
                table: "Basket");

            migrationBuilder.CreateIndex(
                name: "IX_Basket_UserName",
                table: "Basket",
                column: "UserName",
                unique: true);
        }
    }
}
