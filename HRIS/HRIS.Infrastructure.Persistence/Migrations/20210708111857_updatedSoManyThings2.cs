using Microsoft.EntityFrameworkCore.Migrations;

namespace HRIS.Infrastructure.Persistence.Migrations
{
    public partial class updatedSoManyThings2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchProducts_ProductAllocationUse_ProductAllocationUseId",
                table: "BranchProducts");

            migrationBuilder.DropIndex(
                name: "IX_BranchProducts_ProductAllocationUseId",
                table: "BranchProducts");

            migrationBuilder.DropColumn(
                name: "ProductAllocationUseId",
                table: "BranchProducts");

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "BranchProducts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "StoreProductId",
                table: "BranchProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BranchProducts_StoreProductId",
                table: "BranchProducts",
                column: "StoreProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchProducts_StoreProduct_StoreProductId",
                table: "BranchProducts",
                column: "StoreProductId",
                principalTable: "StoreProduct",
                principalColumn: "StoreProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchProducts_StoreProduct_StoreProductId",
                table: "BranchProducts");

            migrationBuilder.DropIndex(
                name: "IX_BranchProducts_StoreProductId",
                table: "BranchProducts");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "BranchProducts");

            migrationBuilder.DropColumn(
                name: "StoreProductId",
                table: "BranchProducts");

            migrationBuilder.AddColumn<int>(
                name: "ProductAllocationUseId",
                table: "BranchProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BranchProducts_ProductAllocationUseId",
                table: "BranchProducts",
                column: "ProductAllocationUseId");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchProducts_ProductAllocationUse_ProductAllocationUseId",
                table: "BranchProducts",
                column: "ProductAllocationUseId",
                principalTable: "ProductAllocationUse",
                principalColumn: "ProductAllocationUseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
