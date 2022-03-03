using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Somnia.API.Migrations
{
    public partial class campoparcelas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovimentoPaiID",
                table: "Movimentos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2022, 3, 2, 20, 22, 49, 917, DateTimeKind.Local).AddTicks(5622));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movimentos_Movimentos_MovimentoPaiID",
                table: "Movimentos");

            migrationBuilder.DropIndex(
                name: "IX_Movimentos_MovimentoPaiID",
                table: "Movimentos");

            migrationBuilder.DropColumn(
                name: "MovimentoPaiID",
                table: "Movimentos");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2022, 3, 2, 19, 57, 21, 684, DateTimeKind.Local).AddTicks(430));
        }
    }
}
