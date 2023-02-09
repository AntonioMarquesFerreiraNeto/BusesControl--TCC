using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class addvaltot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataVencimento",
                table: "Financeiro",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Pagament",
                table: "Financeiro",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorParcelaDR",
                table: "Financeiro",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotDR",
                table: "Financeiro",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataVencimento",
                table: "Financeiro");

            migrationBuilder.DropColumn(
                name: "Pagament",
                table: "Financeiro");

            migrationBuilder.DropColumn(
                name: "ValorParcelaDR",
                table: "Financeiro");

            migrationBuilder.DropColumn(
                name: "ValorTotDR",
                table: "Financeiro");
        }
    }
}
