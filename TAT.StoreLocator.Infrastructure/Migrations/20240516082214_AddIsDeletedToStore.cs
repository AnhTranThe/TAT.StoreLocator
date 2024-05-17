using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAT.StoreLocator.Infrastructure.Migrations
{
    public partial class AddIsDeletedToStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Stores_StoreId",
                table: "Galleries");

            migrationBuilder.DropIndex(
                name: "IX_Galleries_StoreId",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Galleries");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Stores",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Stores");

            migrationBuilder.AddColumn<string>(
                name: "StoreId",
                table: "Galleries",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Galleries_StoreId",
                table: "Galleries",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Stores_StoreId",
                table: "Galleries",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
        }
    }
}
