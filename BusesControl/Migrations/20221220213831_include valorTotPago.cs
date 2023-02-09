using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class includevalorTotPago : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotalPagoContrato",
                table: "Contrato",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotalPagoCliente",
                table: "ClientesContrato",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorTotalPagoContrato",
                table: "Contrato");

            migrationBuilder.DropColumn(
                name: "ValorTotalPagoCliente",
                table: "ClientesContrato");
        }
    }
}
