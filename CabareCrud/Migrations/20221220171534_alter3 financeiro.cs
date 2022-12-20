using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class alter3financeiro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorParcela",
                table: "Financeiro");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorJuros",
                table: "Financeiro",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorJuros",
                table: "Financeiro");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorParcela",
                table: "Financeiro",
                type: "decimal(65,30)",
                nullable: true);
        }
    }
}
