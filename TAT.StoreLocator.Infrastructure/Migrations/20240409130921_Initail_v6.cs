using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAT.StoreLocator.Infrastructure.Migrations
{
    public partial class Initail_v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Categories_CategoryId",
                table: "Galleries");

            migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Products_ProductId",
                table: "Galleries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapStoreWishlists",
                table: "MapStoreWishlists");

            migrationBuilder.DropIndex(
                name: "IX_MapStoreWishlists_StoreId",
                table: "MapStoreWishlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapProductWishlists",
                table: "MapProductWishlists");

            migrationBuilder.DropIndex(
                name: "IX_MapProductWishlists_ProductId",
                table: "MapProductWishlists");

            migrationBuilder.DropIndex(
                name: "IX_Galleries_CategoryId",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MapStoreWishlists");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MapProductWishlists");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "FileStatus",
                table: "Galleries");

            migrationBuilder.AlterColumn<string>(
                name: "WishlistId",
                table: "MapStoreWishlists",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StoreId",
                table: "MapStoreWishlists",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WishlistId",
                table: "MapProductWishlists",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "MapProductWishlists",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryId",
                table: "Categories",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapStoreWishlists",
                table: "MapStoreWishlists",
                columns: new[] { "StoreId", "WishlistId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapProductWishlists",
                table: "MapProductWishlists",
                columns: new[] { "ProductId", "WishlistId" });

            migrationBuilder.CreateTable(
                name: "MapGalleryProducts",
                columns: table => new
                {
                    GalleryId = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapGalleryProducts", x => new { x.ProductId, x.GalleryId });
                    table.ForeignKey(
                        name: "FK_MapGalleryProducts_Galleries_GalleryId",
                        column: x => x.GalleryId,
                        principalTable: "Galleries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MapGalleryProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MapGalleryStores",
                columns: table => new
                {
                    GalleryId = table.Column<string>(type: "text", nullable: false),
                    StoreId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapGalleryStores", x => new { x.StoreId, x.GalleryId });
                    table.ForeignKey(
                        name: "FK_MapGalleryStores_Galleries_GalleryId",
                        column: x => x.GalleryId,
                        principalTable: "Galleries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MapGalleryStores_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_GalleryId",
                table: "Categories",
                column: "GalleryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MapGalleryProducts_GalleryId",
                table: "MapGalleryProducts",
                column: "GalleryId");

            migrationBuilder.CreateIndex(
                name: "IX_MapGalleryStores_GalleryId",
                table: "MapGalleryStores",
                column: "GalleryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Galleries_GalleryId",
                table: "Categories",
                column: "GalleryId",
                principalTable: "Galleries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Products_ProductId",
                table: "Galleries",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Galleries_GalleryId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Products_ProductId",
                table: "Galleries");

            migrationBuilder.DropTable(
                name: "MapGalleryProducts");

            migrationBuilder.DropTable(
                name: "MapGalleryStores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapStoreWishlists",
                table: "MapStoreWishlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapProductWishlists",
                table: "MapProductWishlists");

            migrationBuilder.DropIndex(
                name: "IX_Categories_GalleryId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "GalleryId",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "WishlistId",
                table: "MapStoreWishlists",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "StoreId",
                table: "MapStoreWishlists",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "MapStoreWishlists",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "WishlistId",
                table: "MapProductWishlists",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "MapProductWishlists",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "MapProductWishlists",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Galleries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FileStatus",
                table: "Galleries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapStoreWishlists",
                table: "MapStoreWishlists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapProductWishlists",
                table: "MapProductWishlists",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MapStoreWishlists_StoreId",
                table: "MapStoreWishlists",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_MapProductWishlists_ProductId",
                table: "MapProductWishlists",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Galleries_CategoryId",
                table: "Galleries",
                column: "CategoryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Categories_CategoryId",
                table: "Galleries",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Products_ProductId",
                table: "Galleries",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
