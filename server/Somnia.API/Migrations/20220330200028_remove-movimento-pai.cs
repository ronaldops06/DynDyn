using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Somnia.API.Migrations
{
    public partial class removemovimentopai : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movimentos_Movimentos_MovimentoPaiID",
                table: "Movimentos");

            migrationBuilder.DropIndex(
                name: "IX_Movimentos_MovimentoPaiID",
                table: "Movimentos");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2022, 3, 30, 17, 0, 28, 284, DateTimeKind.Local).AddTicks(6719));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2022, 3, 2, 20, 31, 34, 677, DateTimeKind.Local).AddTicks(3227));

            migrationBuilder.CreateIndex(
                name: "IX_Movimentos_MovimentoPaiID",
                table: "Movimentos",
                column: "MovimentoPaiID");

            migrationBuilder.AddForeignKey(
                name: "FK_Movimentos_Movimentos_MovimentoPaiID",
                table: "Movimentos",
                column: "MovimentoPaiID",
                principalTable: "Movimentos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
