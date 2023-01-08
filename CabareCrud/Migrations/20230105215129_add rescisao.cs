using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class addrescisao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rescisao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Multa = table.Column<decimal>(nullable: true),
                    ContratoId = table.Column<int>(nullable: true),
                    PessoaFisicaId = table.Column<int>(nullable: true),
                    PessoaJuridicaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rescisao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rescisao_Contrato_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Contrato",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rescisao_Cliente_PessoaFisicaId",
                        column: x => x.PessoaFisicaId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rescisao_Cliente_PessoaJuridicaId",
                        column: x => x.PessoaJuridicaId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rescisao_ContratoId",
                table: "Rescisao",
                column: "ContratoId");

            migrationBuilder.CreateIndex(
                name: "IX_Rescisao_PessoaFisicaId",
                table: "Rescisao",
                column: "PessoaFisicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rescisao_PessoaJuridicaId",
                table: "Rescisao",
                column: "PessoaJuridicaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rescisao");
        }
    }
}
