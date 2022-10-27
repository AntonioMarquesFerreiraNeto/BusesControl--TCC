﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class partialjoin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PessoaFisicaId",
                table: "Contrato",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PessoaJuridicaId",
                table: "Contrato",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contrato_PessoaFisicaId",
                table: "Contrato",
                column: "PessoaFisicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Contrato_PessoaJuridicaId",
                table: "Contrato",
                column: "PessoaJuridicaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contrato_Cliente_PessoaFisicaId",
                table: "Contrato",
                column: "PessoaFisicaId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contrato_Cliente_PessoaJuridicaId",
                table: "Contrato",
                column: "PessoaJuridicaId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contrato_Cliente_PessoaFisicaId",
                table: "Contrato");

            migrationBuilder.DropForeignKey(
                name: "FK_Contrato_Cliente_PessoaJuridicaId",
                table: "Contrato");

            migrationBuilder.DropIndex(
                name: "IX_Contrato_PessoaFisicaId",
                table: "Contrato");

            migrationBuilder.DropIndex(
                name: "IX_Contrato_PessoaJuridicaId",
                table: "Contrato");

            migrationBuilder.DropColumn(
                name: "PessoaFisicaId",
                table: "Contrato");

            migrationBuilder.DropColumn(
                name: "PessoaJuridicaId",
                table: "Contrato");
        }
    }
}
