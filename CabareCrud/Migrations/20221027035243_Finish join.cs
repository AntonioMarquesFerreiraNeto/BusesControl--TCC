using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class Finishjoin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contrato_Cliente_ClienteId",
                table: "Contrato");

            migrationBuilder.DropIndex(
                name: "IX_Contrato_ClienteId",
                table: "Contrato");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Contrato");

            migrationBuilder.AddColumn<int>(
                name: "PessoaFisicaId",
                table: "Contrato",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PessoaJuridicaId",
                table: "Contrato",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contrato_PessoaFisicaId",
                table: "Contrato",
                column: "PessoaFisicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Contrato_PessoaJuridicaId",
                table: "Contrato",
                column: "PessoaJuridicaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contrato_Cliente_PessoaFisicaId",
                table: "Contrato",
                column: "PessoaFisicaId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contrato_Cliente_PessoaJuridicaId",
                table: "Contrato",
                column: "PessoaJuridicaId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contrato_Cliente_PessoaFisicaId",
                table: "Contrato");

            migrationBuilder.DropForeignKey(
                name: "FK_Contrato_Cliente_PessoaJuridicaId",
                table: "Contrato");

            migrationBuilder.DropIndex(
                name: "IX_Contrato_PessoaFisicaId",
                table: "Contrato");

            migrationBuilder.DropIndex(
                name: "IX_Contrato_PessoaJuridicaId",
                table: "Contrato");

            migrationBuilder.DropColumn(
                name: "PessoaFisicaId",
                table: "Contrato");

            migrationBuilder.DropColumn(
                name: "PessoaJuridicaId",
                table: "Contrato");

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Contrato",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contrato_ClienteId",
                table: "Contrato",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contrato_Cliente_ClienteId",
                table: "Contrato",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
