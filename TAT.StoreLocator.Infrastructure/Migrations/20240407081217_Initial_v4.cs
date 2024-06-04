using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAT.StoreLocator.Infrastructure.Migrations
{
    public partial class Initial_v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Categories_Galleries_GalleryId",
                table: "Categories");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_Stores_Addresses_AddressId",
                table: "Stores");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_Users_Addresses_AddressId",
                table: "Users");

            _ = migrationBuilder.DropIndex(
                name: "IX_Users_AddressId",
                table: "Users");

            _ = migrationBuilder.DropIndex(
                name: "IX_Stores_AddressId",
                table: "Stores");

            _ = migrationBuilder.DropIndex(
                name: "IX_Categories_GalleryId",
                table: "Categories");

            _ = migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Users");

            _ = migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Stores");

            _ = migrationBuilder.DropColumn(
                name: "GalleryId",
                table: "Categories");

            _ = migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Galleries",
                type: "text",
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "StoreId",
                table: "Addresses",
                type: "text",
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Addresses",
                type: "text",
                nullable: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Galleries_CategoryId",
                table: "Galleries",
                column: "CategoryId",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Addresses_StoreId",
                table: "Addresses",
                column: "StoreId",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId",
                unique: true);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Stores_StoreId",
                table: "Addresses",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Categories_CategoryId",
                table: "Galleries",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Stores_StoreId",
                table: "Addresses");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Categories_CategoryId",
                table: "Galleries");

            _ = migrationBuilder.DropIndex(
                name: "IX_Galleries_CategoryId",
                table: "Galleries");

            _ = migrationBuilder.DropIndex(
                name: "IX_Addresses_StoreId",
                table: "Addresses");

            _ = migrationBuilder.DropIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses");

            _ = migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Galleries");

            _ = migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Addresses");

            _ = migrationBuilder.DropColumn(
                name: "UserId",
                table: "Addresses");

            _ = migrationBuilder.AddColumn<string>(
                name: "AddressId",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            _ = migrationBuilder.AddColumn<string>(
                name: "AddressId",
                table: "Stores",
                type: "text",
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "GalleryId",
                table: "Categories",
                type: "text",
                nullable: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Users_AddressId",
                table: "Users",
                column: "AddressId",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Stores_AddressId",
                table: "Stores",
                column: "AddressId",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Categories_GalleryId",
                table: "Categories",
                column: "GalleryId",
                unique: true);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Categories_Galleries_GalleryId",
                table: "Categories",
                column: "GalleryId",
                principalTable: "Galleries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Stores_Addresses_AddressId",
                table: "Stores",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Users_Addresses_AddressId",
                table: "Users",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}