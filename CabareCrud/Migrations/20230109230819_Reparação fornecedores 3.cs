using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class Reparaçãofornecedores3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FornecedorJuridicos",
                table: "FornecedorJuridicos");

            migrationBuilder.RenameTable(
                name: "FornecedorJuridicos",
                newName: "FornecedorJuridico");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FornecedorJuridico",
                table: "FornecedorJuridico",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FornecedorJuridico",
                table: "FornecedorJuridico");

            migrationBuilder.RenameTable(
                name: "FornecedorJuridico",
                newName: "FornecedorJuridicos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FornecedorJuridicos",
                table: "FornecedorJuridicos",
                column: "Id");
        }
    }
}
