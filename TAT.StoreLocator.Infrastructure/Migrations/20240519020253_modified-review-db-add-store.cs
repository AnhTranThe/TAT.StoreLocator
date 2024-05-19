using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAT.StoreLocator.Infrastructure.Migrations
{
    public partial class modifiedreviewdbaddstore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StoreId",
                table: "Reviews",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_StoreId",
                table: "Reviews",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Stores_StoreId",
                table: "Reviews",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Stores_StoreId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_StoreId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Reviews");
        }
    }
}
