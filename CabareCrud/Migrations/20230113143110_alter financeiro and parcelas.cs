using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class alterfinanceiroandparcelas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataEfetuacao",
                table: "Parcelas",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorParcelaDR",
                table: "Financeiro",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AddColumn<int>(
                name: "FinanceiroStatus",
                table: "Financeiro",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataEfetuacao",
                table: "Parcelas");

            migrationBuilder.DropColumn(
                name: "FinanceiroStatus",
                table: "Financeiro");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorParcelaDR",
                table: "Financeiro",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
