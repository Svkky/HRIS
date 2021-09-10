using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRIS.Infrastructure.Persistence.Migrations
{
    public partial class UpdatedVendorPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "VendorPaymentMaster");

            migrationBuilder.DropColumn(
                name: "DateAllocated",
                table: "ProductWareHouse");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductWareHouse");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "VendorPaymentMaster",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreProductId",
                table: "ProductWareHouse",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VendorPaymentMaster_StoreProductId",
                table: "VendorPaymentMaster",
                column: "StoreProductId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorPaymentMaster_SupplierId",
                table: "VendorPaymentMaster",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWareHouse_StoreProductId",
                table: "ProductWareHouse",
                column: "StoreProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductWareHouse_StoreProduct_StoreProductId",
                table: "ProductWareHouse",
                column: "StoreProductId",
                principalTable: "StoreProduct",
                principalColumn: "StoreProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorPaymentMaster_StoreProduct_StoreProductId",
                table: "VendorPaymentMaster",
                column: "StoreProductId",
                principalTable: "StoreProduct",
                principalColumn: "StoreProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorPaymentMaster_Supplier_SupplierId",
                table: "VendorPaymentMaster",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductWareHouse_StoreProduct_StoreProductId",
                table: "ProductWareHouse");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorPaymentMaster_StoreProduct_StoreProductId",
                table: "VendorPaymentMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorPaymentMaster_Supplier_SupplierId",
                table: "VendorPaymentMaster");

            migrationBuilder.DropIndex(
                name: "IX_VendorPaymentMaster_StoreProductId",
                table: "VendorPaymentMaster");

            migrationBuilder.DropIndex(
                name: "IX_VendorPaymentMaster_SupplierId",
                table: "VendorPaymentMaster");

            migrationBuilder.DropIndex(
                name: "IX_ProductWareHouse_StoreProductId",
                table: "ProductWareHouse");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "VendorPaymentMaster");

            migrationBuilder.DropColumn(
                name: "StoreProductId",
                table: "ProductWareHouse");

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "VendorPaymentMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAllocated",
                table: "ProductWareHouse",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductWareHouse",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
