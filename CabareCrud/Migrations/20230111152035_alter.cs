using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class alter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParcelasCliente");

            migrationBuilder.DropColumn(
                name: "ValorTotTaxaJurosPaga",
                table: "ClientesContrato");

            migrationBuilder.DropColumn(
                name: "ValorTotalPagoCliente",
                table: "ClientesContrato");

            migrationBuilder.CreateTable(
                name: "Financeiro",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ContratoId = table.Column<int>(nullable: true),
                    PessoaJuridicaId = table.Column<int>(nullable: true),
                    PessoaFisicaId = table.Column<int>(nullable: true),
                    FornecedorFisicoId = table.Column<int>(nullable: true),
                    FornecedorJuridicoId = table.Column<int>(nullable: true),
                    DespesaReceita = table.Column<int>(nullable: false),
                    ValorTotalPagoCliente = table.Column<decimal>(nullable: true),
                    ValorTotTaxaJurosPaga = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Financeiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Financeiro_Contrato_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Contrato",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Financeiro_FornecedorFisico_FornecedorFisicoId",
                        column: x => x.FornecedorFisicoId,
                        principalTable: "FornecedorFisico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Financeiro_FornecedorJuridico_FornecedorJuridicoId",
                        column: x => x.FornecedorJuridicoId,
                        principalTable: "FornecedorJuridico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Financeiro_Cliente_PessoaFisicaId",
                        column: x => x.PessoaFisicaId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Financeiro_Cliente_PessoaJuridicaId",
                        column: x => x.PessoaJuridicaId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Parcelas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FinanceiroId = table.Column<int>(nullable: true),
                    NomeParcela = table.Column<string>(nullable: true),
                    ValorJuros = table.Column<decimal>(nullable: true),
                    DataVencimentoParcela = table.Column<DateTime>(nullable: true),
                    StatusPagamento = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcelas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parcelas_Financeiro_FinanceiroId",
                        column: x => x.FinanceiroId,
                        principalTable: "Financeiro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Financeiro_ContratoId",
                table: "Financeiro",
                column: "ContratoId");

            migrationBuilder.CreateIndex(
                name: "IX_Financeiro_FornecedorFisicoId",
                table: "Financeiro",
                column: "FornecedorFisicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Financeiro_FornecedorJuridicoId",
                table: "Financeiro",
                column: "FornecedorJuridicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Financeiro_PessoaFisicaId",
                table: "Financeiro",
                column: "PessoaFisicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Financeiro_PessoaJuridicaId",
                table: "Financeiro",
                column: "PessoaJuridicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcelas_FinanceiroId",
                table: "Parcelas",
                column: "FinanceiroId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parcelas");

            migrationBuilder.DropTable(
                name: "Financeiro");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotTaxaJurosPaga",
                table: "ClientesContrato",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotalPagoCliente",
                table: "ClientesContrato",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ParcelasCliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientesContratoId = table.Column<int>(type: "int", nullable: true),
                    DataVencimentoParcela = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NomeParcela = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    StatusPagamento = table.Column<int>(type: "int", nullable: false),
                    ValorJuros = table.Column<decimal>(type: "decimal(65,30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelasCliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParcelasCliente_ClientesContrato_ClientesContratoId",
                        column: x => x.ClientesContratoId,
                        principalTable: "ClientesContrato",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParcelasCliente_ClientesContratoId",
                table: "ParcelasCliente",
                column: "ClientesContratoId");
        }
    }
}
