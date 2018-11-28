using Microsoft.EntityFrameworkCore.Migrations;

namespace Patpedhi.Infrastructure.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "signature_photo_url",
                table: "UserProfile",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "profile_photo_url",
                table: "UserProfile",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar",
                oldMaxLength: 5000,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "signature_photo_url",
                table: "UserProfile",
                type: "varchar",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "profile_photo_url",
                table: "UserProfile",
                type: "varchar",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
