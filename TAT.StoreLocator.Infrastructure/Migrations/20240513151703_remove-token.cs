using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAT.StoreLocator.Infrastructure.Migrations
{
    public partial class removetoken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            _ = migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "text",
                nullable: true);

            _ = migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}