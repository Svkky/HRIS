using Microsoft.EntityFrameworkCore.Migrations;

namespace HRIS.Infrastructure.Persistence.Migrations
{
    public partial class ChangedProductIdtoStoreProductId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "SalesDetail");

            migrationBuilder.AddColumn<int>(
                name: "StoreProductId",
                table: "SalesDetail",
                nullable: false,
                defaultValue: 0);


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "StoreProductId",
                table: "SalesDetail");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "SalesDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
