using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class Finishjoin3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contrato_Cliente_PessoaFisicaId",
                table: "Contrato");

            migrationBuilder.DropForeignKey(
                name: "FK_Contrato_Cliente_PessoaJuridicaId",
                table: "Contrato");

            migrationBuilder.AlterColumn<int>(
                name: "PessoaJuridicaId",
                table: "Contrato",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PessoaFisicaId",
                table: "Contrato",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Contrato_Cliente_PessoaFisicaId",
                table: "Contrato",
                column: "PessoaFisicaId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contrato_Cliente_PessoaJuridicaId",
                table: "Contrato",
                column: "PessoaJuridicaId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contrato_Cliente_PessoaFisicaId",
                table: "Contrato");

            migrationBuilder.DropForeignKey(
                name: "FK_Contrato_Cliente_PessoaJuridicaId",
                table: "Contrato");

            migrationBuilder.AlterColumn<int>(
                name: "PessoaJuridicaId",
                table: "Contrato",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PessoaFisicaId",
                table: "Contrato",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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
    }
}
