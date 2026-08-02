using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class remove_marketplace_pricing : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_OrderItem_Company_CompanyId",
            table: "OrderItem");

        migrationBuilder.DropIndex(
            name: "IX_OrderItem_CompanyId",
            table: "OrderItem");

        migrationBuilder.DropColumn(
            name: "CompanyId",
            table: "OrderItem");

        migrationBuilder.DropTable(
            name: "ProductPriceDeliveryOption");

        migrationBuilder.Sql("ALTER TABLE [ProductPrice] SET (SYSTEM_VERSIONING = OFF);");

        migrationBuilder.DropTable(
            name: "ProductPrice");

        migrationBuilder.DropTable(
            name: "ProductPriceHistory");

        migrationBuilder.DropTable(
            name: "CompanyProduct");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "CompanyProduct",
            columns: table => new
            {
                ProductId = table.Column<int>(type: "int", nullable: false),
                CompanyId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CompanyProduct", x => new { x.ProductId, x.CompanyId });
                table.ForeignKey(
                    name: "FK_CompanyProduct_Company_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Company",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_CompanyProduct_Product_ProductId",
                    column: x => x.ProductId,
                    principalTable: "Product",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "ProductPrice",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CompanyId = table.Column<int>(type: "int", nullable: false),
                ProductId = table.Column<int>(type: "int", nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                CooperationPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                    .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                    .Annotation("SqlServer:TemporalIsPeriodStartColumn", true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductPrice", x => x.Id);
                table.ForeignKey(
                    name: "FK_ProductPrice_Company_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Company",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ProductPrice_Product_ProductId",
                    column: x => x.ProductId,
                    principalTable: "Product",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            })
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "ProductPriceHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.CreateTable(
            name: "ProductPriceDeliveryOption",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProductPriceId = table.Column<int>(type: "int", nullable: false),
                DeliveryOptionId = table.Column<int>(type: "int", nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                CooperationPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductPriceDeliveryOption", x => x.Id);
                table.ForeignKey(
                    name: "FK_ProductPriceDeliveryOption_DeliveryOption_DeliveryOptionId",
                    column: x => x.DeliveryOptionId,
                    principalTable: "DeliveryOption",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ProductPriceDeliveryOption_ProductPrice_ProductPriceId",
                    column: x => x.ProductPriceId,
                    principalTable: "ProductPrice",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.AddColumn<int>(
            name: "CompanyId",
            table: "OrderItem",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_OrderItem_CompanyId",
            table: "OrderItem",
            column: "CompanyId");

        migrationBuilder.CreateIndex(
            name: "IX_CompanyProduct_CompanyId",
            table: "CompanyProduct",
            column: "CompanyId");

        migrationBuilder.CreateIndex(
            name: "IX_ProductPrice_CompanyId",
            table: "ProductPrice",
            column: "CompanyId");

        migrationBuilder.CreateIndex(
            name: "IX_ProductPrice_ProductId",
            table: "ProductPrice",
            column: "ProductId");

        migrationBuilder.CreateIndex(
            name: "IX_ProductPriceDeliveryOption_DeliveryOptionId",
            table: "ProductPriceDeliveryOption",
            column: "DeliveryOptionId");

        migrationBuilder.CreateIndex(
            name: "IX_ProductPriceDeliveryOption_ProductPriceId",
            table: "ProductPriceDeliveryOption",
            column: "ProductPriceId");

        migrationBuilder.AddForeignKey(
            name: "FK_OrderItem_Company_CompanyId",
            table: "OrderItem",
            column: "CompanyId",
            principalTable: "Company",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
