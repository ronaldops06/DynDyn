using Microsoft.EntityFrameworkCore.Migrations;

namespace DynDyn.API.Migrations
{
    public partial class at_conta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contas_Contas_ContaPaiID",
                table: "Contas");

            migrationBuilder.AlterColumn<int>(
                name: "ContaPaiID",
                table: "Contas",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Contas_Contas_ContaPaiID",
                table: "Contas",
                column: "ContaPaiID",
                principalTable: "Contas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contas_Contas_ContaPaiID",
                table: "Contas");

            migrationBuilder.AlterColumn<int>(
                name: "ContaPaiID",
                table: "Contas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contas_Contas_ContaPaiID",
                table: "Contas",
                column: "ContaPaiID",
                principalTable: "Contas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
