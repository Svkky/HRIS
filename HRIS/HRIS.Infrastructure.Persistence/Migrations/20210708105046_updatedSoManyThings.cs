using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace HRIS.Infrastructure.Persistence.Migrations
{
    public partial class updatedSoManyThings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductAllocationUse");

            migrationBuilder.AddColumn<int>(
                name: "StoreProductId",
                table: "ProductAllocationUse",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BranchProducts",
                columns: table => new
                {
                    BranchProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    BranchId = table.Column<int>(nullable: false),
                    SellingPrice = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    CanExpire = table.Column<bool>(nullable: false),
                    ManufactureDate = table.Column<DateTime>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: true),
                    ProductTypeVariationId = table.Column<int>(nullable: true),
                    VariationQuantity = table.Column<string>(nullable: true),
                    CriticalLevel = table.Column<int>(nullable: false),
                    VatPercent = table.Column<double>(nullable: false),
                    ProductAllocationUseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchProducts", x => x.BranchProductId);
                    table.ForeignKey(
                        name: "FK_BranchProducts_ProductAllocationUse_ProductAllocationUseId",
                        column: x => x.ProductAllocationUseId,
                        principalTable: "ProductAllocationUse",
                        principalColumn: "ProductAllocationUseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchProducts_ProductTypeVariation_ProductTypeVariationId",
                        column: x => x.ProductTypeVariationId,
                        principalTable: "ProductTypeVariation",
                        principalColumn: "ProductTypeVariationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAllocationUse_StoreProductId",
                table: "ProductAllocationUse",
                column: "StoreProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchProducts_ProductAllocationUseId",
                table: "BranchProducts",
                column: "ProductAllocationUseId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchProducts_ProductTypeVariationId",
                table: "BranchProducts",
                column: "ProductTypeVariationId");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAllocationUse_StoreProduct_StoreProductId",
                table: "ProductAllocationUse");

            migrationBuilder.DropTable(
                name: "BranchProducts");

            migrationBuilder.DropIndex(
                name: "IX_ProductAllocationUse_StoreProductId",
                table: "ProductAllocationUse");

            migrationBuilder.DropColumn(
                name: "StoreProductId",
                table: "ProductAllocationUse");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductAllocationUse",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CanExpire = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CostPrice = table.Column<double>(type: "float", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CriticalLevel = table.Column<int>(type: "int", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManufactureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductTypeVariationId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    SellingPrice = table.Column<double>(type: "float", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VatPercent = table.Column<double>(type: "float", nullable: false),
                    variationQuantity = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductTypeVariation_ProductTypeVariationId",
                        column: x => x.ProductTypeVariationId,
                        principalTable: "ProductTypeVariation",
                        principalColumn: "ProductTypeVariationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_SubCategory_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategory",
                        principalColumn: "SubCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeVariationId",
                table: "Products",
                column: "ProductTypeVariationId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubCategoryId",
                table: "Products",
                column: "SubCategoryId");
        }
    }
}
