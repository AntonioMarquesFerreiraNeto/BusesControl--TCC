using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class Situacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Situacao",
                table: "Contrato");

            migrationBuilder.AddColumn<int>(
                name: "Andamento",
                table: "Contrato",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Andamento",
                table: "Contrato");

            migrationBuilder.AddColumn<int>(
                name: "Situacao",
                table: "Contrato",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
