using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsFile.Infrastructure.Migrations
{
    public partial class InsertAcceptAndAcceptTimeColumnsToWithdrawalTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Accept",
                table: "Withdrawal",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptTime",
                table: "Withdrawal",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accept",
                table: "Withdrawal");

            migrationBuilder.DropColumn(
                name: "AcceptTime",
                table: "Withdrawal");
        }
    }
}
