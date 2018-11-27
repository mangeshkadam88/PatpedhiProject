using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Patpedhi.Infrastructure.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "account_no",
                table: "UserProfile",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "UserProfile",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "account_no",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "UserProfile");
        }
    }
}
