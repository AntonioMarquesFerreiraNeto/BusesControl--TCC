using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class ContratoAtributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Aprovacao",
                table: "Contrato",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataEmissao",
                table: "Contrato",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataVencimento",
                table: "Contrato",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Detalhamento",
                table: "Contrato",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "QtParcelas",
                table: "Contrato",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusContrato",
                table: "Contrato",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorMonetario",
                table: "Contrato",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aprovacao",
                table: "Contrato");

            migrationBuilder.DropColumn(
                name: "DataEmissao",
                table: "Contrato");

            migrationBuilder.DropColumn(
                name: "DataVencimento",
                table: "Contrato");

            migrationBuilder.DropColumn(
                name: "Detalhamento",
                table: "Contrato");

            migrationBuilder.DropColumn(
                name: "QtParcelas",
                table: "Contrato");

            migrationBuilder.DropColumn(
                name: "StatusContrato",
                table: "Contrato");

            migrationBuilder.DropColumn(
                name: "ValorMonetario",
                table: "Contrato");
        }
    }
}
