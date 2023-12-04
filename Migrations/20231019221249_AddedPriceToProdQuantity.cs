using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseholdOnlineStore.Migrations
{
    public partial class AddedPriceToProdQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "ProductsQuantity",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProductsQuantity");
        }
    }
}
