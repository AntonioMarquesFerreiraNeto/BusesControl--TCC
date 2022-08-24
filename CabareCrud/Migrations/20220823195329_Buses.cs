using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class Buses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Onibus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Marca = table.Column<string>(nullable: false),
                    NameBus = table.Column<string>(nullable: false),
                    DataFabricacao = table.Column<DateTime>(nullable: false),
                    Renavam = table.Column<string>(nullable: false),
                    Placa = table.Column<string>(nullable: false),
                    Chassi = table.Column<string>(nullable: false),
                    Assentos = table.Column<long>(nullable: false),
                    StatusOnibus = table.Column<int>(nullable: false),
                    corBus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Onibus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Onibus_Chassi",
                table: "Onibus",
                column: "Chassi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Onibus_Placa",
                table: "Onibus",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Onibus_Renavam",
                table: "Onibus",
                column: "Renavam",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Onibus");
        }
    }
}
