using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentSystem.Server.Migrations
{
    public partial class UpdatedPrivateBankCurrencies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "PrivateBankCurrencies",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "PrivateBankCurrencies");
        }
    }
}
