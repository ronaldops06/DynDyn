using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Somnia.API.Migrations
{
    public partial class campoparcela : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Parcela",
                table: "Movimentos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalParcelas",
                table: "Movimentos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2022, 3, 2, 20, 31, 34, 677, DateTimeKind.Local).AddTicks(3227));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Parcela",
                table: "Movimentos");

            migrationBuilder.DropColumn(
                name: "TotalParcelas",
                table: "Movimentos");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2022, 3, 2, 20, 22, 49, 917, DateTimeKind.Local).AddTicks(5622));
        }
    }
}
