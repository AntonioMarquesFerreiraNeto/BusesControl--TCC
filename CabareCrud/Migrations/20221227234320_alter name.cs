using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class altername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorTotTaxaJuros",
                table: "ClientesContrato");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotTaxaJurosPaga",
                table: "ClientesContrato",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorTotTaxaJurosPaga",
                table: "ClientesContrato");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotTaxaJuros",
                table: "ClientesContrato",
                type: "decimal(65,30)",
                nullable: true);
        }
    }
}
