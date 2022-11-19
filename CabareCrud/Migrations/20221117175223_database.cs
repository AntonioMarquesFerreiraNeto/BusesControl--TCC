using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
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
                    IdVinculacaoContratual = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    NomeFantasia = table.Column<string>(nullable: true),
                    Cnpj = table.Column<string>(nullable: true),
                    RazaoSocial = table.Column<string>(nullable: true),
                    InscricaoEstadual = table.Column<string>(nullable: true),
                    InscricaoMunicipal = table.Column<string>(nullable: true),
                    PessoaJuridica_Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funcionario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Cpf = table.Column<string>(nullable: false),
                    DataNascimento = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Telefone = table.Column<string>(maxLength: 9, nullable: false),
                    Cep = table.Column<string>(maxLength: 8, nullable: false),
                    NumeroResidencial = table.Column<string>(nullable: false),
                    Logradouro = table.Column<string>(nullable: false),
                    ComplementoResidencial = table.Column<string>(nullable: false),
                    Bairro = table.Column<string>(nullable: false),
                    Cidade = table.Column<string>(nullable: false),
                    Estado = table.Column<string>(nullable: false),
                    Ddd = table.Column<string>(nullable: false),
                    Apelido = table.Column<string>(nullable: true),
                    Senha = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Cargos = table.Column<int>(nullable: false),
                    StatusUsuario = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Onibus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Marca = table.Column<string>(nullable: false),
                    NameBus = table.Column<string>(nullable: false),
                    DataFabricacao = table.Column<string>(nullable: false),
                    Renavam = table.Column<string>(nullable: false),
                    Placa = table.Column<string>(nullable: false),
                    Chassi = table.Column<string>(nullable: false),
                    Assentos = table.Column<string>(nullable: false),
                    StatusOnibus = table.Column<int>(nullable: false),
                    corBus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Onibus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contrato",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MotoristaId = table.Column<int>(nullable: false),
                    OnibusId = table.Column<int>(nullable: false),
                    ValorMonetario = table.Column<decimal>(nullable: false),
                    ValorParcelaContrato = table.Column<decimal>(nullable: true),
                    QtParcelas = table.Column<int>(nullable: false),
                    DataEmissao = table.Column<DateTime>(nullable: false),
                    DataVencimento = table.Column<DateTime>(nullable: false),
                    Detalhamento = table.Column<string>(nullable: false),
                    StatusContrato = table.Column<int>(nullable: false),
                    Aprovacao = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contrato", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contrato_Funcionario_MotoristaId",
                        column: x => x.MotoristaId,
                        principalTable: "Funcionario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contrato_Onibus_OnibusId",
                        column: x => x.OnibusId,
                        principalTable: "Onibus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientesContrato",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ContratoId = table.Column<int>(nullable: false),
                    PessoaJuridicaId = table.Column<int>(nullable: true),
                    PessoaFisicaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientesContrato", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientesContrato_Contrato_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Contrato",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientesContrato_Cliente_PessoaFisicaId",
                        column: x => x.PessoaFisicaId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientesContrato_Cliente_PessoaJuridicaId",
                        column: x => x.PessoaJuridicaId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Cliente_InscricaoEstadual",
                table: "Cliente",
                column: "InscricaoEstadual",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_NomeFantasia",
                table: "Cliente",
                column: "NomeFantasia",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_RazaoSocial",
                table: "Cliente",
                column: "RazaoSocial",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientesContrato_ContratoId",
                table: "ClientesContrato",
                column: "ContratoId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientesContrato_PessoaFisicaId",
                table: "ClientesContrato",
                column: "PessoaFisicaId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientesContrato_PessoaJuridicaId",
                table: "ClientesContrato",
                column: "PessoaJuridicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Contrato_MotoristaId",
                table: "Contrato",
                column: "MotoristaId");

            migrationBuilder.CreateIndex(
                name: "IX_Contrato_OnibusId",
                table: "Contrato",
                column: "OnibusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientesContrato");

            migrationBuilder.DropTable(
                name: "Contrato");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Funcionario");

            migrationBuilder.DropTable(
                name: "Onibus");
        }
    }
}
