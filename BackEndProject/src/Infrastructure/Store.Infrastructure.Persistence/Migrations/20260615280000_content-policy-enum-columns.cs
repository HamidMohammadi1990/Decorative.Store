using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class content_policy_enum_columns : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "EntityTypeInt",
            table: "ContentPolicy",
            type: "int",
            nullable: true);

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
                    WHEN 'Bank' THEN 39
                    WHEN 'BankAccount' THEN 40
                    WHEN 'BankTransaction' THEN 41
                    WHEN 'ChequeTransaction' THEN 42
                    WHEN 'CompanyProduct' THEN 43
                    WHEN 'Currency' THEN 44
                    WHEN 'Discount' THEN 45
                    WHEN 'Expense' THEN 46
                    WHEN 'ExpenseType' THEN 47
                    WHEN 'FinancialDocument' THEN 48
                    WHEN 'FinancialDocumentDetail' THEN 49
                    WHEN 'OrderCommission' THEN 50
                    WHEN 'OrderItemAttachment' THEN 51
                    WHEN 'OrderItemAttachmentType' THEN 52
                    WHEN 'OrderItemAttachmentTypeRestriction' THEN 53
                    WHEN 'OrderItemProperty' THEN 54
                    WHEN 'OrderNote' THEN 55
                    WHEN 'OrderVat' THEN 56
                    WHEN 'Page' THEN 57
                    WHEN 'PageSection' THEN 58
                    WHEN 'PosTransaction' THEN 59
                    WHEN 'ProductFeature' THEN 60
                    WHEN 'ProductFeatureType' THEN 61
                    WHEN 'ProductOrderItemAttachmentType' THEN 62
                    WHEN 'ProductProperty' THEN 63
                    WHEN 'ProductPropertyRule' THEN 64
                    WHEN 'PropertyItemDependency' THEN 65
                    WHEN 'RefreshToken' THEN 66
                    WHEN 'Section' THEN 67
                    WHEN 'SectionItem' THEN 68
                    WHEN 'SectionType' THEN 69
                    WHEN 'UserSession' THEN 70
                    WHEN 'Wallet' THEN 71
                    WHEN 'WalletTransaction' THEN 72
                    WHEN 'WebSiteSetting' THEN 73
                    ELSE 1
                END
            WHERE EntityTypeInt IS NULL;
            """);

        migrationBuilder.DropIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy");

        migrationBuilder.DropIndex(
            name: "IX_ContentPolicy_UserId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy");

        migrationBuilder.DropColumn(
            name: "EntityType",
            table: "ContentPolicy");

        migrationBuilder.RenameColumn(
            name: "EntityTypeInt",
            table: "ContentPolicy",
            newName: "EntityType");

        migrationBuilder.AlterColumn<int>(
            name: "EntityType",
            table: "ContentPolicy",
            type: "int",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AddColumn<int>(
            name: "QueryActionInt",
            table: "ContentPolicy",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.Sql(
            """
            UPDATE ContentPolicy SET QueryActionInt =
                CASE
                    WHEN QueryAction IS NULL THEN 0
                    WHEN QueryAction = 'Get' THEN 1
                    WHEN QueryAction = 'GetAll' THEN 2
                    WHEN QueryAction = 'Search' THEN 3
                    WHEN QueryAction = 'GetUserAddresses' THEN 4
                    ELSE 0
                END;
            """);

        migrationBuilder.DropColumn(
            name: "QueryAction",
            table: "ContentPolicy");

        migrationBuilder.RenameColumn(
            name: "QueryActionInt",
            table: "ContentPolicy",
            newName: "QueryAction");

        migrationBuilder.CreateIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy",
            columns: new[] { "RoleId", "EntityType", "QueryAction", "IsActive" });

        migrationBuilder.CreateIndex(
            name: "IX_ContentPolicy_UserId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy",
            columns: new[] { "UserId", "EntityType", "QueryAction", "IsActive" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy");

        migrationBuilder.DropIndex(
            name: "IX_ContentPolicy_UserId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy");

        migrationBuilder.AddColumn<string>(
            name: "QueryActionString",
            table: "ContentPolicy",
            type: "VARCHAR(100)",
            unicode: false,
            maxLength: 100,
            nullable: true);

        migrationBuilder.Sql(
            """
            UPDATE ContentPolicy SET QueryActionString =
                CASE QueryAction
                    WHEN 0 THEN NULL
                    WHEN 1 THEN 'Get'
                    WHEN 2 THEN 'GetAll'
                    WHEN 3 THEN 'Search'
                    WHEN 4 THEN 'GetUserAddresses'
                    ELSE NULL
                END;
            """);

        migrationBuilder.DropColumn(
            name: "QueryAction",
            table: "ContentPolicy");

        migrationBuilder.RenameColumn(
            name: "QueryActionString",
            table: "ContentPolicy",
            newName: "QueryAction");

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
                    WHEN 39 THEN 'Bank'
                    WHEN 40 THEN 'BankAccount'
                    WHEN 41 THEN 'BankTransaction'
                    WHEN 42 THEN 'ChequeTransaction'
                    WHEN 43 THEN 'CompanyProduct'
                    WHEN 44 THEN 'Currency'
                    WHEN 45 THEN 'Discount'
                    WHEN 46 THEN 'Expense'
                    WHEN 47 THEN 'ExpenseType'
                    WHEN 48 THEN 'FinancialDocument'
                    WHEN 49 THEN 'FinancialDocumentDetail'
                    WHEN 50 THEN 'OrderCommission'
                    WHEN 51 THEN 'OrderItemAttachment'
                    WHEN 52 THEN 'OrderItemAttachmentType'
                    WHEN 53 THEN 'OrderItemAttachmentTypeRestriction'
                    WHEN 54 THEN 'OrderItemProperty'
                    WHEN 55 THEN 'OrderNote'
                    WHEN 56 THEN 'OrderVat'
                    WHEN 57 THEN 'Page'
                    WHEN 58 THEN 'PageSection'
                    WHEN 59 THEN 'PosTransaction'
                    WHEN 60 THEN 'ProductFeature'
                    WHEN 61 THEN 'ProductFeatureType'
                    WHEN 62 THEN 'ProductOrderItemAttachmentType'
                    WHEN 63 THEN 'ProductProperty'
                    WHEN 64 THEN 'ProductPropertyRule'
                    WHEN 65 THEN 'PropertyItemDependency'
                    WHEN 66 THEN 'RefreshToken'
                    WHEN 67 THEN 'Section'
                    WHEN 68 THEN 'SectionItem'
                    WHEN 69 THEN 'SectionType'
                    WHEN 70 THEN 'UserSession'
                    WHEN 71 THEN 'Wallet'
                    WHEN 72 THEN 'WalletTransaction'
                    WHEN 73 THEN 'WebSiteSetting'
                    ELSE 'BlogPost'
                END;
            """);

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
            name: "IX_ContentPolicy_RoleId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy",
            columns: new[] { "RoleId", "EntityType", "QueryAction", "IsActive" });

        migrationBuilder.CreateIndex(
            name: "IX_ContentPolicy_UserId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy",
            columns: new[] { "UserId", "EntityType", "QueryAction", "IsActive" });
    }
}
