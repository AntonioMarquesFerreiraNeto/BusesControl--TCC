using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class Financeiro2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinanceiroContrato_ClientesContrato_ClientesContratoId",
                table: "FinanceiroContrato");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FinanceiroContrato",
                table: "FinanceiroContrato");

            migrationBuilder.RenameTable(
                name: "FinanceiroContrato",
                newName: "Financeiro");

            migrationBuilder.RenameIndex(
                name: "IX_FinanceiroContrato_ClientesContratoId",
                table: "Financeiro",
                newName: "IX_Financeiro_ClientesContratoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Financeiro",
                table: "Financeiro",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Financeiro_ClientesContrato_ClientesContratoId",
                table: "Financeiro",
                column: "ClientesContratoId",
                principalTable: "ClientesContrato",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financeiro_ClientesContrato_ClientesContratoId",
                table: "Financeiro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Financeiro",
                table: "Financeiro");

            migrationBuilder.RenameTable(
                name: "Financeiro",
                newName: "FinanceiroContrato");

            migrationBuilder.RenameIndex(
                name: "IX_Financeiro_ClientesContratoId",
                table: "FinanceiroContrato",
                newName: "IX_FinanceiroContrato_ClientesContratoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinanceiroContrato",
                table: "FinanceiroContrato",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FinanceiroContrato_ClientesContrato_ClientesContratoId",
                table: "FinanceiroContrato",
                column: "ClientesContratoId",
                principalTable: "ClientesContrato",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
