using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class alterrescisao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ValorPagoContrato",
                table: "Rescisao",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessRescisao",
                table: "ClientesContrato",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorPagoContrato",
                table: "Rescisao");

            migrationBuilder.DropColumn(
                name: "ProcessRescisao",
                table: "ClientesContrato");
        }
    }
}
