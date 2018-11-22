using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Patpedhi.Infrastructure.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "profile_photo_url",
                table: "UserProfile",
                type: "varchar",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "signature_photo_url",
                table: "UserProfile",
                type: "varchar",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetRoles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profile_photo_url",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "signature_photo_url",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetRoles");
        }
    }
}
