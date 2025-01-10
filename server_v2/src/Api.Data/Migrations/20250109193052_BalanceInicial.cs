using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Data.Migrations
{
    public partial class BalanceInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Balance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    Valuation = table.Column<double>(type: "double precision", nullable: true),
                    Dividends = table.Column<double>(type: "double precision", nullable: true),
                    Income = table.Column<double>(type: "double precision", nullable: true),
                    PercentageValuation = table.Column<double>(type: "double precision", nullable: true),
                    PercentageIncome = table.Column<double>(type: "double precision", nullable: true),
                    Credit = table.Column<double>(type: "double precision", nullable: false),
                    Debit = table.Column<double>(type: "double precision", nullable: false),
                    SalaryCredit = table.Column<double>(type: "double precision", nullable: true),
                    SalaryDebit = table.Column<double>(type: "double precision", nullable: true),
                    BalanceDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Balance_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            
            migrationBuilder.CreateIndex(
                name: "IX_Balance_AccountId",
                table: "Balance",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Balance_BalanceDate",
                table: "Balance",
                column: "BalanceDate",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Balance");

        }
    }
}
