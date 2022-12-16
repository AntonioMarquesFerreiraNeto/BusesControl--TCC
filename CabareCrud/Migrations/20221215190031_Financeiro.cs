using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class Financeiro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinanceiroContrato",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientesContratoId = table.Column<int>(nullable: true),
                    NomeParcela = table.Column<int>(nullable: true),
                    DataVencimentoParcela = table.Column<DateTime>(nullable: true),
                    StatusPagamento = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinanceiroContrato", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinanceiroContrato_ClientesContrato_ClientesContratoId",
                        column: x => x.ClientesContratoId,
                        principalTable: "ClientesContrato",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinanceiroContrato_ClientesContratoId",
                table: "FinanceiroContrato",
                column: "ClientesContratoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinanceiroContrato");
        }
    }
}
