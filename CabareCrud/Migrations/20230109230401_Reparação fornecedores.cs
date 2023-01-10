using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class Reparaçãofornecedores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fornecedor");

            migrationBuilder.CreateTable(
                name: "FornecedorFisico",
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
                    Status = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Cpf = table.Column<string>(nullable: false),
                    DataNascimento = table.Column<DateTime>(nullable: false),
                    Rg = table.Column<string>(nullable: false),
                    NameMae = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FornecedorFisico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FornecedorJuridicos",
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
                    Status = table.Column<int>(nullable: false),
                    NomeFantasia = table.Column<string>(nullable: false),
                    Cnpj = table.Column<string>(nullable: false),
                    RazaoSocial = table.Column<string>(nullable: false),
                    InscricaoEstadual = table.Column<string>(nullable: false),
                    InscricaoMunicipal = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FornecedorJuridicos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FornecedorFisico");

            migrationBuilder.DropTable(
                name: "FornecedorJuridicos");

            migrationBuilder.CreateTable(
                name: "Fornecedor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Bairro = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Cep = table.Column<string>(type: "varchar(8) CHARACTER SET utf8mb4", maxLength: 8, nullable: false),
                    Cidade = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    ComplementoResidencial = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Ddd = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Email = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Estado = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Logradouro = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    NumeroResidencial = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Telefone = table.Column<string>(type: "varchar(9) CHARACTER SET utf8mb4", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedor", x => x.Id);
                });
        }
    }
}
