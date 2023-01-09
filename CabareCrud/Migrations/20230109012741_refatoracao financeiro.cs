using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class refatoracaofinanceiro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Financeiro");

            migrationBuilder.CreateTable(
                name: "ParcelasCliente",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientesContratoId = table.Column<int>(nullable: true),
                    NomeParcela = table.Column<string>(nullable: true),
                    ValorJuros = table.Column<decimal>(nullable: true),
                    DataVencimentoParcela = table.Column<DateTime>(nullable: true),
                    StatusPagamento = table.Column<int>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParcelasCliente");

            migrationBuilder.CreateTable(
                name: "Financeiro",
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
                    table.PrimaryKey("PK_Financeiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Financeiro_ClientesContrato_ClientesContratoId",
                        column: x => x.ClientesContratoId,
                        principalTable: "ClientesContrato",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Financeiro_ClientesContratoId",
                table: "Financeiro",
                column: "ClientesContratoId");
        }
    }
}
