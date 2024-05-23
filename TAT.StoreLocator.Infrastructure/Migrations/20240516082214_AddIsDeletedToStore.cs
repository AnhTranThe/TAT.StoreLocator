using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAT.StoreLocator.Infrastructure.Migrations
{
    public partial class AddIsDeletedToStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Stores_StoreId",
                table: "Galleries");

            _ = migrationBuilder.DropIndex(
                name: "IX_Galleries_StoreId",
                table: "Galleries");

            _ = migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Galleries");

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Stores",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Stores");

            _ = migrationBuilder.AddColumn<string>(
                name: "StoreId",
                table: "Galleries",
                type: "text",
                nullable: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Galleries_StoreId",
                table: "Galleries",
                column: "StoreId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Stores_StoreId",
                table: "Galleries",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
        }
    }
}