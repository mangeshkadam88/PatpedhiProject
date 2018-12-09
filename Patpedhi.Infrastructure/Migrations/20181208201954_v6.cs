using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Patpedhi.Infrastructure.Migrations
{
    public partial class v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "added_by",
                table: "Savings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "approved_by",
                table: "Savings",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "approved_on",
                table: "Savings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Savings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "added_by",
                table: "Savings");

            migrationBuilder.DropColumn(
                name: "approved_by",
                table: "Savings");

            migrationBuilder.DropColumn(
                name: "approved_on",
                table: "Savings");

            migrationBuilder.DropColumn(
                name: "description",
                table: "Savings");
        }
    }
}
