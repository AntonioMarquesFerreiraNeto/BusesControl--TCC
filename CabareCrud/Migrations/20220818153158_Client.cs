using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class Client : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(maxLength: 9, nullable: false),
                    Cep = table.Column<string>(maxLength: 8, nullable: false),
                    NumeroResidencial = table.Column<string>(nullable: false),
                    Logradouro = table.Column<string>(nullable: false),
                    ComplementoResidencial = table.Column<string>(nullable: false),
                    Bairro = table.Column<string>(nullable: false),
                    Cidade = table.Column<string>(nullable: false),
                    Estado = table.Column<string>(nullable: false),
                    Ddd = table.Column<string>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Cpf = table.Column<string>(nullable: true),
                    DataNascimento = table.Column<DateTime>(nullable: true),
                    Rg = table.Column<string>(nullable: true),
                    NameMae = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    NomeFantasia = table.Column<string>(nullable: true),
                    Cnpj = table.Column<string>(nullable: true),
                    razaoSocial = table.Column<string>(nullable: true),
                    inscricaoEstadual = table.Column<string>(nullable: true),
                    inscricaoMunicipal = table.Column<string>(nullable: true),
                    PessoaJuridica_Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Email",
                table: "Cliente",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Id",
                table: "Cliente",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Telefone",
                table: "Cliente",
                column: "Telefone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Cpf",
                table: "Cliente",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Rg",
                table: "Cliente",
                column: "Rg",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Cnpj",
                table: "Cliente",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_NomeFantasia",
                table: "Cliente",
                column: "NomeFantasia",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_inscricaoEstadual",
                table: "Cliente",
                column: "inscricaoEstadual",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_razaoSocial",
                table: "Cliente",
                column: "razaoSocial",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cliente");
        }
    }
}
