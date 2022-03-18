using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Somnia.API.Migrations
{
    public partial class campoconsolidado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Consolidado",
                table: "Movimentos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2022, 3, 2, 19, 57, 21, 684, DateTimeKind.Local).AddTicks(430));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Consolidado",
                table: "Movimentos");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2022, 2, 4, 19, 43, 20, 630, DateTimeKind.Local).AddTicks(3617));
        }
    }
}
