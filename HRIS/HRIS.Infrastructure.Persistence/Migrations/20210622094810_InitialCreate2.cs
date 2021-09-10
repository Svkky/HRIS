using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRIS.Infrastructure.Persistence.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDisbled",
                table: "BranchAdmin",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEnabled",
                table: "BranchAdmin",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DisabledBy",
                table: "BranchAdmin",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnabledBy",
                table: "BranchAdmin",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDisbled",
                table: "BranchAdmin");

            migrationBuilder.DropColumn(
                name: "DateEnabled",
                table: "BranchAdmin");

            migrationBuilder.DropColumn(
                name: "DisabledBy",
                table: "BranchAdmin");

            migrationBuilder.DropColumn(
                name: "EnabledBy",
                table: "BranchAdmin");
        }
    }
}
