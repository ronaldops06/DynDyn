using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Somnia.API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Login = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    Role = table.Column<string>(type: "TEXT", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Contas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoriaID = table.Column<int>(type: "INTEGER", nullable: false),
                    ContaPaiID = table.Column<int>(type: "INTEGER", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Contas_Categorias_CategoriaID",
                        column: x => x.CategoriaID,
                        principalTable: "Categorias",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contas_Contas_ContaPaiID",
                        column: x => x.ContaPaiID,
                        principalTable: "Contas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operacoes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Recorrente = table.Column<int>(type: "INTEGER", nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoriaID = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operacoes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Operacoes_Categorias_CategoriaID",
                        column: x => x.CategoriaID,
                        principalTable: "Categorias",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Saldos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Valor = table.Column<double>(type: "REAL", nullable: false),
                    Acumulado = table.Column<double>(type: "REAL", nullable: false),
                    Valorizacao = table.Column<double>(type: "REAL", nullable: false),
                    Dividendos = table.Column<double>(type: "REAL", nullable: false),
                    Rendimento = table.Column<double>(type: "REAL", nullable: false),
                    Credito = table.Column<double>(type: "REAL", nullable: false),
                    Debito = table.Column<double>(type: "REAL", nullable: false),
                    Entrada = table.Column<double>(type: "REAL", nullable: false),
                    Saida = table.Column<double>(type: "REAL", nullable: false),
                    DataSaldo = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ContaID = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Saldos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Saldos_Contas_ContaID",
                        column: x => x.ContaID,
                        principalTable: "Contas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movimentos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Valor = table.Column<double>(type: "REAL", nullable: false),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true),
                    ContaID = table.Column<int>(type: "INTEGER", nullable: false),
                    ContaDestinoID = table.Column<int>(type: "INTEGER", nullable: false),
                    OperacaoID = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimentos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Movimentos_Contas_ContaDestinoID",
                        column: x => x.ContaDestinoID,
                        principalTable: "Contas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movimentos_Contas_ContaID",
                        column: x => x.ContaID,
                        principalTable: "Contas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movimentos_Operacoes_OperacaoID",
                        column: x => x.OperacaoID,
                        principalTable: "Operacoes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "DataAlteracao", "DataCriacao", "Login", "Name", "Password", "Role" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 2, 4, 19, 23, 43, 870, DateTimeKind.Local).AddTicks(4695), "admin", "Admin", "pgadmin", "" });

            migrationBuilder.CreateIndex(
                name: "IX_Contas_CategoriaID",
                table: "Contas",
                column: "CategoriaID");

            migrationBuilder.CreateIndex(
                name: "IX_Contas_ContaPaiID",
                table: "Contas",
                column: "ContaPaiID");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentos_ContaDestinoID",
                table: "Movimentos",
                column: "ContaDestinoID");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentos_ContaID",
                table: "Movimentos",
                column: "ContaID");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentos_OperacaoID",
                table: "Movimentos",
                column: "OperacaoID");

            migrationBuilder.CreateIndex(
                name: "IX_Operacoes_CategoriaID",
                table: "Operacoes",
                column: "CategoriaID");

            migrationBuilder.CreateIndex(
                name: "IX_Saldos_ContaID",
                table: "Saldos",
                column: "ContaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movimentos");

            migrationBuilder.DropTable(
                name: "Saldos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Operacoes");

            migrationBuilder.DropTable(
                name: "Contas");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
