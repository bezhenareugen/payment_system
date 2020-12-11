using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentSystem.Server.Migrations
{
    public partial class AddedApiCurrencyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrivateBankCurrencies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ccy = table.Column<string>(nullable: true),
                    Base_Ccy = table.Column<string>(nullable: true),
                    Buy = table.Column<string>(nullable: true),
                    Sale = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateBankCurrencies", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivateBankCurrencies");
        }
    }
}
