using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAT.StoreLocator.Infrastructure.Migrations
{
    public partial class Initial_V6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Stores_StoreId",
                table: "Addresses");

            _ = migrationBuilder.DropIndex(
                name: "IX_Addresses_StoreId",
                table: "Addresses");

            _ = migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Addresses");

            _ = migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            _ = migrationBuilder.AddColumn<string>(
                name: "AddressId",
                table: "Stores",
                type: "text",
                nullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Galleries",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Galleries",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Galleries",
                type: "text",
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Galleries",
                type: "text",
                nullable: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Stores_AddressId",
                table: "Stores",
                column: "AddressId",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Galleries_UserId",
                table: "Galleries",
                column: "UserId",
                unique: true);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Users_UserId",
                table: "Galleries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Stores_Addresses_AddressId",
                table: "Stores",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Users_UserId",
                table: "Galleries");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_Stores_Addresses_AddressId",
                table: "Stores");

            _ = migrationBuilder.DropIndex(
                name: "IX_Stores_AddressId",
                table: "Stores");

            _ = migrationBuilder.DropIndex(
                name: "IX_Galleries_UserId",
                table: "Galleries");

            _ = migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Stores");

            _ = migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Galleries");

            _ = migrationBuilder.DropColumn(
                name: "UserId",
                table: "Galleries");

            _ = migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Galleries",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Galleries",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "StoreId",
                table: "Addresses",
                type: "text",
                nullable: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Addresses_StoreId",
                table: "Addresses",
                column: "StoreId",
                unique: true);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Stores_StoreId",
                table: "Addresses",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}