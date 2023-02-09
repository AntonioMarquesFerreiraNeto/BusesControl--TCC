using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class fornecedor4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financeiro_Fornecedors_FornecedorFisicoId",
                table: "Financeiro");

            migrationBuilder.DropForeignKey(
                name: "FK_Financeiro_Fornecedors_FornecedorJuridicoId",
                table: "Financeiro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fornecedors",
                table: "Fornecedors");

            migrationBuilder.RenameTable(
                name: "Fornecedors",
                newName: "Fornecedor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fornecedor",
                table: "Fornecedor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Financeiro_Fornecedor_FornecedorFisicoId",
                table: "Financeiro",
                column: "FornecedorFisicoId",
                principalTable: "Fornecedor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Financeiro_Fornecedor_FornecedorJuridicoId",
                table: "Financeiro",
                column: "FornecedorJuridicoId",
                principalTable: "Fornecedor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financeiro_Fornecedor_FornecedorFisicoId",
                table: "Financeiro");

            migrationBuilder.DropForeignKey(
                name: "FK_Financeiro_Fornecedor_FornecedorJuridicoId",
                table: "Financeiro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fornecedor",
                table: "Fornecedor");

            migrationBuilder.RenameTable(
                name: "Fornecedor",
                newName: "Fornecedors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fornecedors",
                table: "Fornecedors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Financeiro_Fornecedors_FornecedorFisicoId",
                table: "Financeiro",
                column: "FornecedorFisicoId",
                principalTable: "Fornecedors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Financeiro_Fornecedors_FornecedorJuridicoId",
                table: "Financeiro",
                column: "FornecedorJuridicoId",
                principalTable: "Fornecedors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
