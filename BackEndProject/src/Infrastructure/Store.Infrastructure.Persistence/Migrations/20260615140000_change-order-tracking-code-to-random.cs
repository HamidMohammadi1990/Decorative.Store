using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class changeordertrackingcodetorandom : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Order_TrackingCode",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "TrackingCode",
            table: "Order");

        migrationBuilder.AddColumn<long>(
            name: "TrackingCode",
            table: "Order",
            type: "bigint",
            nullable: false,
            defaultValue: 0L);

        migrationBuilder.Sql("""
            UPDATE o
            SET o.TrackingCode = CAST((1000000000 + (o.Id * 10007) + (ABS(CHECKSUM(NEWID())) % 1000000)) AS bigint)
            FROM [Order] o
            """);

        migrationBuilder.CreateIndex(
            name: "IX_Order_TrackingCode",
            table: "Order",
            column: "TrackingCode",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Order_TrackingCode",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "TrackingCode",
            table: "Order");

        migrationBuilder.AddColumn<decimal>(
            name: "TrackingCode",
            table: "Order",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: false,
            computedColumnSql: "Id + 1000");

        migrationBuilder.CreateIndex(
            name: "IX_Order_TrackingCode",
            table: "Order",
            column: "TrackingCode",
            unique: true);
    }
}
