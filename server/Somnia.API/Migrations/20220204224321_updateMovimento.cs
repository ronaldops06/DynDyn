using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Somnia.API.Migrations
{
    public partial class updateMovimento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movimentos_Contas_ContaDestinoID",
                table: "Movimentos");

            migrationBuilder.AlterColumn<int>(
                name: "ContaDestinoID",
                table: "Movimentos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2022, 2, 4, 19, 43, 20, 630, DateTimeKind.Local).AddTicks(3617));

            migrationBuilder.AddForeignKey(
                name: "FK_Movimentos_Contas_ContaDestinoID",
                table: "Movimentos",
                column: "ContaDestinoID",
                principalTable: "Contas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movimentos_Contas_ContaDestinoID",
                table: "Movimentos");

            migrationBuilder.AlterColumn<int>(
                name: "ContaDestinoID",
                table: "Movimentos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2022, 2, 4, 19, 23, 43, 870, DateTimeKind.Local).AddTicks(4695));

            migrationBuilder.AddForeignKey(
                name: "FK_Movimentos_Contas_ContaDestinoID",
                table: "Movimentos",
                column: "ContaDestinoID",
                principalTable: "Contas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
