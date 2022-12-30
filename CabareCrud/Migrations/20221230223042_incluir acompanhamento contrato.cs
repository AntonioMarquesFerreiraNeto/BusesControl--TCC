using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class incluiracompanhamentocontrato : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Situacao",
                table: "Contrato",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Situacao",
                table: "Contrato");
        }
    }
}
