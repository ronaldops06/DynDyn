using Microsoft.EntityFrameworkCore.Migrations;

namespace DynDyn.API.Migrations
{
    public partial class at_conta_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Saldos",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Operacoes",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Movimentos",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Contas",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Categorias",
                newName: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Saldos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Operacoes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Movimentos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Contas",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Categorias",
                newName: "Id");
        }
    }
}
