using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAT.StoreLocator.Infrastructure.Migrations
{
    public partial class Initail_v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Categories_CategoryId",
                table: "Galleries");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Products_ProductId",
                table: "Galleries");

            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_MapStoreWishlists",
                table: "MapStoreWishlists");

            _ = migrationBuilder.DropIndex(
                name: "IX_MapStoreWishlists_StoreId",
                table: "MapStoreWishlists");

            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_MapProductWishlists",
                table: "MapProductWishlists");

            _ = migrationBuilder.DropIndex(
                name: "IX_MapProductWishlists_ProductId",
                table: "MapProductWishlists");

            _ = migrationBuilder.DropIndex(
                name: "IX_Galleries_CategoryId",
                table: "Galleries");

            _ = migrationBuilder.DropColumn(
                name: "Id",
                table: "MapStoreWishlists");

            _ = migrationBuilder.DropColumn(
                name: "Id",
                table: "MapProductWishlists");

            _ = migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Galleries");

            _ = migrationBuilder.DropColumn(
                name: "FileStatus",
                table: "Galleries");

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

            _ = migrationBuilder.AddColumn<string>(
                name: "GalleryId",
                table: "Categories",
                type: "text",
                nullable: true);

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_MapStoreWishlists",
                table: "MapStoreWishlists",
                columns: new[] { "StoreId", "WishlistId" });

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_MapProductWishlists",
                table: "MapProductWishlists",
                columns: new[] { "ProductId", "WishlistId" });

            _ = migrationBuilder.CreateTable(
                name: "MapGalleryProducts",
                columns: table => new
                {
                    GalleryId = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_MapGalleryProducts", x => new { x.ProductId, x.GalleryId });
                    _ = table.ForeignKey(
                        name: "FK_MapGalleryProducts_Galleries_GalleryId",
                        column: x => x.GalleryId,
                        principalTable: "Galleries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_MapGalleryProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "MapGalleryStores",
                columns: table => new
                {
                    GalleryId = table.Column<string>(type: "text", nullable: false),
                    StoreId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_MapGalleryStores", x => new { x.StoreId, x.GalleryId });
                    _ = table.ForeignKey(
                        name: "FK_MapGalleryStores_Galleries_GalleryId",
                        column: x => x.GalleryId,
                        principalTable: "Galleries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_MapGalleryStores_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Categories_GalleryId",
                table: "Categories",
                column: "GalleryId",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_MapGalleryProducts_GalleryId",
                table: "MapGalleryProducts",
                column: "GalleryId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_MapGalleryStores_GalleryId",
                table: "MapGalleryStores",
                column: "GalleryId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Categories_Galleries_GalleryId",
                table: "Categories",
                column: "GalleryId",
                principalTable: "Galleries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Products_ProductId",
                table: "Galleries",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Categories_Galleries_GalleryId",
                table: "Categories");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Products_ProductId",
                table: "Galleries");

            _ = migrationBuilder.DropTable(
                name: "MapGalleryProducts");

            _ = migrationBuilder.DropTable(
                name: "MapGalleryStores");

            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_MapStoreWishlists",
                table: "MapStoreWishlists");

            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_MapProductWishlists",
                table: "MapProductWishlists");

            _ = migrationBuilder.DropIndex(
                name: "IX_Categories_GalleryId",
                table: "Categories");

            _ = migrationBuilder.DropColumn(
                name: "GalleryId",
                table: "Categories");

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

            _ = migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "MapStoreWishlists",
                type: "text",
                nullable: false,
                defaultValue: "");

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

            _ = migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "MapProductWishlists",
                type: "text",
                nullable: false,
                defaultValue: "");

            _ = migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Galleries",
                type: "text",
                nullable: true);

            _ = migrationBuilder.AddColumn<int>(
                name: "FileStatus",
                table: "Galleries",
                type: "integer",
                nullable: true);

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_MapStoreWishlists",
                table: "MapStoreWishlists",
                column: "Id");

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_MapProductWishlists",
                table: "MapProductWishlists",
                column: "Id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_MapStoreWishlists_StoreId",
                table: "MapStoreWishlists",
                column: "StoreId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_MapProductWishlists_ProductId",
                table: "MapProductWishlists",
                column: "ProductId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Galleries_CategoryId",
                table: "Galleries",
                column: "CategoryId",
                unique: true);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Categories_CategoryId",
                table: "Galleries",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Products_ProductId",
                table: "Galleries",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}