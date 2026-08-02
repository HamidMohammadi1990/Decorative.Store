using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class add_product_price : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>(
            name: "Price",
            table: "Product",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.AddColumn<decimal>(
            name: "CompareAtPrice",
            table: "Product",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CompareAtPrice",
            table: "Product");

        migrationBuilder.DropColumn(
            name: "Price",
            table: "Product");
    }
}
