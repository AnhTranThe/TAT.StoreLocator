using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAT.StoreLocator.Infrastructure.Migrations
{
    public partial class Initial_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<string>(
                name: "AddressId",
                table: "Stores",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Reviews",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "Reviews",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AlterColumn<string>(
                name: "StoreId",
                table: "Products",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "Products",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AlterColumn<string>(
                name: "WishlistId",
                table: "MapStoreWishlists",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AlterColumn<string>(
                name: "StoreId",
                table: "MapStoreWishlists",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AlterColumn<string>(
                name: "WishlistId",
                table: "MapProductWishlists",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "MapProductWishlists",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "Galleries",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AlterColumn<string>(
                name: "GalleryId",
                table: "Categories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<string>(
                name: "AddressId",
                table: "Stores",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Reviews",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "Reviews",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "StoreId",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "WishlistId",
                table: "MapStoreWishlists",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "StoreId",
                table: "MapStoreWishlists",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "WishlistId",
                table: "MapProductWishlists",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "MapProductWishlists",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "Galleries",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "GalleryId",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
