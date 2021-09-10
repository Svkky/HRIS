using Microsoft.EntityFrameworkCore.Migrations;

namespace HRIS.Infrastructure.Identity.Migrations
{
    public partial class BranchIdToUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "Identity",
                table: "User",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "Identity",
                table: "User");
        }
    }
}
