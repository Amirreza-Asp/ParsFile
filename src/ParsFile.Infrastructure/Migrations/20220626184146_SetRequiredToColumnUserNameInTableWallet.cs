using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsFile.Infrastructure.Migrations
{
    public partial class SetRequiredToColumnUserNameInTableWallet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_AspNetUsers_UserName",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_UserName",
                table: "Wallet");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Wallet",
                type: "nvarchar(256)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_UserName",
                table: "Wallet",
                column: "UserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_AspNetUsers_UserName",
                table: "Wallet",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_AspNetUsers_UserName",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_UserName",
                table: "Wallet");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Wallet",
                type: "nvarchar(256)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_UserName",
                table: "Wallet",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_AspNetUsers_UserName",
                table: "Wallet",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "UserName");
        }
    }
}
