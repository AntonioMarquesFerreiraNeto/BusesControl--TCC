using Microsoft.EntityFrameworkCore.Migrations;

namespace BusesControl.Migrations
{
    public partial class database2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientesContrato_Contrato_ContratoId",
                table: "ClientesContrato");

            migrationBuilder.AlterColumn<int>(
                name: "ContratoId",
                table: "ClientesContrato",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientesContrato_Contrato_ContratoId",
                table: "ClientesContrato",
                column: "ContratoId",
                principalTable: "Contrato",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientesContrato_Contrato_ContratoId",
                table: "ClientesContrato");

            migrationBuilder.AlterColumn<int>(
                name: "ContratoId",
                table: "ClientesContrato",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientesContrato_Contrato_ContratoId",
                table: "ClientesContrato",
                column: "ContratoId",
                principalTable: "Contrato",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
