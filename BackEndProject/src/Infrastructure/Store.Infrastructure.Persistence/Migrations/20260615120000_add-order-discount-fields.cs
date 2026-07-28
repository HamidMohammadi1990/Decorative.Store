using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class addorderdiscountfields : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "AppliedDiscountId",
            table: "Order",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "DiscountAmount",
            table: "Order",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.AddColumn<bool>(
            name: "IsDiscountUsageConsumed",
            table: "Order",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.CreateIndex(
            name: "IX_Order_AppliedDiscountId",
            table: "Order",
            column: "AppliedDiscountId");

        migrationBuilder.AddForeignKey(
            name: "FK_Order_Discount_AppliedDiscountId",
            table: "Order",
            column: "AppliedDiscountId",
            principalTable: "Discount",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Order_Discount_AppliedDiscountId",
            table: "Order");

        migrationBuilder.DropIndex(
            name: "IX_Order_AppliedDiscountId",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "AppliedDiscountId",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "DiscountAmount",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "IsDiscountUsageConsumed",
            table: "Order");
    }
}
