using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAT.StoreLocator.Infrastructure.Migrations
{
    public partial class Initial_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "Locations");

            _ = migrationBuilder.AddColumn<decimal>(
                name: "latitude",
                table: "Addresses",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            _ = migrationBuilder.AddColumn<decimal>(
                name: "longitude",
                table: "Addresses",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "latitude",
                table: "Addresses");

            _ = migrationBuilder.DropColumn(
                name: "longitude",
                table: "Addresses");

            _ = migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    AddressId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    latitude = table.Column<decimal>(type: "numeric", nullable: false),
                    longitude = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Locations", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Locations_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Locations_AddressId",
                table: "Locations",
                column: "AddressId",
                unique: true);
        }
    }
}