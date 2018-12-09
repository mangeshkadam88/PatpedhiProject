using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Patpedhi.Infrastructure.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Savings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    added_date = table.Column<DateTime>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: true),
                    user_id = table.Column<Guid>(nullable: false),
                    amount = table.Column<decimal>(nullable: false),
                    is_active = table.Column<bool>(nullable: false),
                    is_approved = table.Column<bool>(nullable: false),
                    savings_type = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Savings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Savings");
        }
    }
}
