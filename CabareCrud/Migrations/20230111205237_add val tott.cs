using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class addvaltott : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataEmissao",
                table: "Financeiro",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Detalhamento",
                table: "Financeiro",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "QtParcelas",
                table: "Financeiro",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeEfetuacao",
                table: "Financeiro",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataEmissao",
                table: "Financeiro");

            migrationBuilder.DropColumn(
                name: "Detalhamento",
                table: "Financeiro");

            migrationBuilder.DropColumn(
                name: "QtParcelas",
                table: "Financeiro");

            migrationBuilder.DropColumn(
                name: "TypeEfetuacao",
                table: "Financeiro");
        }
    }
}
