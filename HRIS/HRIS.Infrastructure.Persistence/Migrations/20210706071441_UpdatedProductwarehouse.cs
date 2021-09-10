using Microsoft.EntityFrameworkCore.Migrations;

namespace HRIS.Infrastructure.Persistence.Migrations
{
    public partial class UpdatedProductwarehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllocatedQuantity",
                table: "ProductWareHouse");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ProductWareHouse",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TransactionType",
                table: "ProductWareHouse",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ProductWareHouse");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "ProductWareHouse");

            migrationBuilder.AddColumn<int>(
                name: "AllocatedQuantity",
                table: "ProductWareHouse",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
