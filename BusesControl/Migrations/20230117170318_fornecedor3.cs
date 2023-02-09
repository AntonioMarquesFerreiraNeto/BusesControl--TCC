using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class fornecedor3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financeiro_FornecedorFisico_FornecedorFisicoId",
                table: "Financeiro");

            migrationBuilder.DropForeignKey(
                name: "FK_Financeiro_FornecedorJuridico_FornecedorJuridicoId",
                table: "Financeiro");

            migrationBuilder.DropTable(
                name: "FornecedorFisico");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FornecedorJuridico",
                table: "FornecedorJuridico");

            migrationBuilder.RenameTable(
                name: "FornecedorJuridico",
                newName: "Fornecedors");

            migrationBuilder.AlterColumn<string>(
                name: "RazaoSocial",
                table: "Fornecedors",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "NomeFantasia",
                table: "Fornecedors",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "InscricaoMunicipal",
                table: "Fornecedors",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "InscricaoEstadual",
                table: "Fornecedors",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "Fornecedors",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Fornecedors",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Fornecedors",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataNascimento",
                table: "Fornecedors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Fornecedors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameMae",
                table: "Fornecedors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rg",
                table: "Fornecedors",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Fornecedors");

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Fornecedors");

            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "Fornecedors");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Fornecedors");

            migrationBuilder.DropColumn(
                name: "NameMae",
                table: "Fornecedors");

            migrationBuilder.DropColumn(
                name: "Rg",
                table: "Fornecedors");

            migrationBuilder.RenameTable(
                name: "Fornecedors",
                newName: "FornecedorJuridico");

            migrationBuilder.AlterColumn<string>(
                name: "RazaoSocial",
                table: "FornecedorJuridico",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NomeFantasia",
                table: "FornecedorJuridico",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InscricaoMunicipal",
                table: "FornecedorJuridico",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InscricaoEstadual",
                table: "FornecedorJuridico",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "FornecedorJuridico",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FornecedorJuridico",
                table: "FornecedorJuridico",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FornecedorFisico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Bairro = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Cep = table.Column<string>(type: "varchar(8) CHARACTER SET utf8mb4", maxLength: 8, nullable: false),
                    Cidade = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    ComplementoResidencial = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Cpf = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Ddd = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Email = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Estado = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Logradouro = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    NameMae = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    NumeroResidencial = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Rg = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Telefone = table.Column<string>(type: "varchar(9) CHARACTER SET utf8mb4", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FornecedorFisico", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Financeiro_FornecedorFisico_FornecedorFisicoId",
                table: "Financeiro",
                column: "FornecedorFisicoId",
                principalTable: "FornecedorFisico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Financeiro_FornecedorJuridico_FornecedorJuridicoId",
                table: "Financeiro",
                column: "FornecedorJuridicoId",
                principalTable: "FornecedorJuridico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
