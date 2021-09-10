using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace HRIS.Infrastructure.Persistence.Migrations
{
    public partial class CreatedProductWarehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductWarehouseMaster",
                columns: table => new
                {
                    ProductWarehouseMasterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillNo = table.Column<string>(nullable: true),
                    VendorId = table.Column<int>(nullable: true),
                    Cartons = table.Column<int>(nullable: false),
                    TotalItemsPerCarton = table.Column<int>(nullable: false),
                    TotalQuantity = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<double>(nullable: false),
                    TotalPaid = table.Column<double>(nullable: false),
                    Balance = table.Column<double>(nullable: false),
                    InvoiceDocument = table.Column<string>(nullable: true),
                    StoreProductId = table.Column<int>(nullable: false),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductWarehouseMaster", x => x.ProductWarehouseMasterId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductWarehouseMaster");
        }
    }
}
