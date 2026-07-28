using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class content_policy_entity_type_string : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "EntityTypeName",
            table: "ContentPolicy",
            type: "VARCHAR(100)",
            unicode: false,
            maxLength: 100,
            nullable: true);

        migrationBuilder.Sql(
            """
            UPDATE ContentPolicy SET EntityTypeName =
                CASE EntityType
                    WHEN 1 THEN 'BlogPost'
                    WHEN 2 THEN 'Order'
                    WHEN 3 THEN 'Company'
                    WHEN 4 THEN 'User'
                    WHEN 5 THEN 'Product'
                    WHEN 6 THEN 'BlogPostComment'
                    WHEN 7 THEN 'ProductComment'
                    WHEN 8 THEN 'CompanyComment'
                    WHEN 9 THEN 'FinancialYear'
                    WHEN 10 THEN 'ProductPrice'
                    WHEN 11 THEN 'ProductPropertyPrice'
                    WHEN 12 THEN 'PropertyItemPrice'
                    WHEN 13 THEN 'CompanyPosDevice'
                    WHEN 14 THEN 'BlogPostCategory'
                    WHEN 15 THEN 'BlogPostTag'
                    WHEN 16 THEN 'BlogPostLike'
                    WHEN 17 THEN 'Category'
                    WHEN 18 THEN 'SubCategory'
                    WHEN 19 THEN 'Tag'
                    WHEN 20 THEN 'City'
                    WHEN 21 THEN 'Province'
                    WHEN 22 THEN 'DeliveryType'
                    WHEN 23 THEN 'DeliveryOption'
                    WHEN 24 THEN 'PostType'
                    WHEN 25 THEN 'Property'
                    WHEN 26 THEN 'PropertyItem'
                    WHEN 27 THEN 'PropertyCategory'
                    WHEN 28 THEN 'ChartOfAccount'
                    WHEN 29 THEN 'CommentTopic'
                    WHEN 30 THEN 'ProductFile'
                    WHEN 31 THEN 'ProductDescription'
                    WHEN 32 THEN 'ProductPriceDeliveryOption'
                    WHEN 33 THEN 'UserAddress'
                    WHEN 34 THEN 'UserRole'
                    WHEN 35 THEN 'Role'
                    WHEN 36 THEN 'Permission'
                    WHEN 37 THEN 'RolePermission'
                    WHEN 38 THEN 'OrderItem'
                    ELSE 'Unknown'
                END
            WHERE EntityTypeName IS NULL;
            """);

        migrationBuilder.DropIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_IsActive",
            table: "ContentPolicy");

        migrationBuilder.DropColumn(
            name: "EntityType",
            table: "ContentPolicy");

        migrationBuilder.RenameColumn(
            name: "EntityTypeName",
            table: "ContentPolicy",
            newName: "EntityType");

        migrationBuilder.AlterColumn<string>(
            name: "EntityType",
            table: "ContentPolicy",
            type: "VARCHAR(100)",
            unicode: false,
            maxLength: 100,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "VARCHAR(100)",
            oldUnicode: false,
            oldMaxLength: 100,
            oldNullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_IsActive",
            table: "ContentPolicy",
            columns: new[] { "RoleId", "EntityType", "IsActive" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "EntityTypeInt",
            table: "ContentPolicy",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.Sql(
            """
            UPDATE ContentPolicy SET EntityTypeInt =
                CASE EntityType
                    WHEN 'BlogPost' THEN 1
                    WHEN 'Order' THEN 2
                    WHEN 'Company' THEN 3
                    WHEN 'User' THEN 4
                    WHEN 'Product' THEN 5
                    WHEN 'BlogPostComment' THEN 6
                    WHEN 'ProductComment' THEN 7
                    WHEN 'CompanyComment' THEN 8
                    WHEN 'FinancialYear' THEN 9
                    WHEN 'ProductPrice' THEN 10
                    WHEN 'ProductPropertyPrice' THEN 11
                    WHEN 'PropertyItemPrice' THEN 12
                    WHEN 'CompanyPosDevice' THEN 13
                    WHEN 'BlogPostCategory' THEN 14
                    WHEN 'BlogPostTag' THEN 15
                    WHEN 'BlogPostLike' THEN 16
                    WHEN 'Category' THEN 17
                    WHEN 'SubCategory' THEN 18
                    WHEN 'Tag' THEN 19
                    WHEN 'City' THEN 20
                    WHEN 'Province' THEN 21
                    WHEN 'DeliveryType' THEN 22
                    WHEN 'DeliveryOption' THEN 23
                    WHEN 'PostType' THEN 24
                    WHEN 'Property' THEN 25
                    WHEN 'PropertyItem' THEN 26
                    WHEN 'PropertyCategory' THEN 27
                    WHEN 'ChartOfAccount' THEN 28
                    WHEN 'CommentTopic' THEN 29
                    WHEN 'ProductFile' THEN 30
                    WHEN 'ProductDescription' THEN 31
                    WHEN 'ProductPriceDeliveryOption' THEN 32
                    WHEN 'UserAddress' THEN 33
                    WHEN 'UserRole' THEN 34
                    WHEN 'Role' THEN 35
                    WHEN 'Permission' THEN 36
                    WHEN 'RolePermission' THEN 37
                    WHEN 'OrderItem' THEN 38
                    ELSE 0
                END;
            """);

        migrationBuilder.DropIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_IsActive",
            table: "ContentPolicy");

        migrationBuilder.DropColumn(
            name: "EntityType",
            table: "ContentPolicy");

        migrationBuilder.RenameColumn(
            name: "EntityTypeInt",
            table: "ContentPolicy",
            newName: "EntityType");

        migrationBuilder.CreateIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_IsActive",
            table: "ContentPolicy",
            columns: new[] { "RoleId", "EntityType", "IsActive" });
    }
}
