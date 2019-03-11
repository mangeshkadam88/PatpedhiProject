using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Patpedhi.Infrastructure.Migrations
{
    public partial class v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    added_date = table.Column<DateTime>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: true),
                    user_id = table.Column<Guid>(nullable: false),
                    amount = table.Column<decimal>(nullable: false),
                    interest_rate = table.Column<decimal>(nullable: false),
                    monthly_emi = table.Column<decimal>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<bool>(nullable: false),
                    is_approved = table.Column<bool>(nullable: false),
                    added_by = table.Column<Guid>(nullable: false),
                    added_on = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    approved_by = table.Column<Guid>(nullable: true),
                    approved_on = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoansHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    added_date = table.Column<DateTime>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: true),
                    loan_id = table.Column<Guid>(nullable: false),
                    emi_paid = table.Column<decimal>(nullable: false),
                    date_paid = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<bool>(nullable: false),
                    is_approved = table.Column<bool>(nullable: false),
                    added_by = table.Column<Guid>(nullable: false),
                    added_on = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    approved_by = table.Column<Guid>(nullable: true),
                    approved_on = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoansHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoansHistory_Loans_loan_id",
                        column: x => x.loan_id,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoansHistory_loan_id",
                table: "LoansHistory",
                column: "loan_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoansHistory");

            migrationBuilder.DropTable(
                name: "Loans");
        }
    }
}
