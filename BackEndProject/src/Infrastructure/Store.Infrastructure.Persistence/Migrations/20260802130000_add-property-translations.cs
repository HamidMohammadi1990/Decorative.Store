using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class add_property_translations : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Code",
            table: "PropertyCategory",
            type: "VARCHAR(20)",
            unicode: false,
            maxLength: 20,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Code",
            table: "Property",
            type: "VARCHAR(20)",
            unicode: false,
            maxLength: 20,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Code",
            table: "PropertyItem",
            type: "VARCHAR(30)",
            unicode: false,
            maxLength: 30,
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "PropertyItemId",
            table: "ProductProperty",
            type: "int",
            nullable: true);

        migrationBuilder.Sql(
            """
            UPDATE PropertyCategory SET Code = CONCAT('pc-', Id) WHERE Code IS NULL;
            UPDATE Property SET Code = CONCAT('p-', Id) WHERE Code IS NULL;
            UPDATE PropertyItem SET Code = CONCAT('pi-', Id) WHERE Code IS NULL;
            """);

        migrationBuilder.AlterColumn<string>(
            name: "Code",
            table: "PropertyCategory",
            type: "VARCHAR(20)",
            unicode: false,
            maxLength: 20,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "VARCHAR(20)",
            oldUnicode: false,
            oldMaxLength: 20,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Code",
            table: "Property",
            type: "VARCHAR(20)",
            unicode: false,
            maxLength: 20,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "VARCHAR(20)",
            oldUnicode: false,
            oldMaxLength: 20,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Code",
            table: "PropertyItem",
            type: "VARCHAR(30)",
            unicode: false,
            maxLength: 30,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "VARCHAR(30)",
            oldUnicode: false,
            oldMaxLength: 30,
            oldNullable: true);

        migrationBuilder.CreateTable(
            name: "PropertyCategoryTranslation",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PropertyCategoryId = table.Column<int>(type: "int", nullable: false),
                LanguageId = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PropertyCategoryTranslation", x => x.Id);
                table.ForeignKey(
                    name: "FK_PropertyCategoryTranslation_Language_LanguageId",
                    column: x => x.LanguageId,
                    principalTable: "Language",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_PropertyCategoryTranslation_PropertyCategory_PropertyCategoryId",
                    column: x => x.PropertyCategoryId,
                    principalTable: "PropertyCategory",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PropertyTranslation",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PropertyId = table.Column<int>(type: "int", nullable: false),
                LanguageId = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: false),
                Description = table.Column<string>(type: "NVARCHAR(250)", maxLength: 250, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PropertyTranslation", x => x.Id);
                table.ForeignKey(
                    name: "FK_PropertyTranslation_Language_LanguageId",
                    column: x => x.LanguageId,
                    principalTable: "Language",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_PropertyTranslation_Property_PropertyId",
                    column: x => x.PropertyId,
                    principalTable: "Property",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PropertyItemTranslation",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PropertyItemId = table.Column<int>(type: "int", nullable: false),
                LanguageId = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PropertyItemTranslation", x => x.Id);
                table.ForeignKey(
                    name: "FK_PropertyItemTranslation_Language_LanguageId",
                    column: x => x.LanguageId,
                    principalTable: "Language",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_PropertyItemTranslation_PropertyItem_PropertyItemId",
                    column: x => x.PropertyItemId,
                    principalTable: "PropertyItem",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ProductPropertyRuleTranslation",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProductPropertyRuleId = table.Column<int>(type: "int", nullable: false),
                LanguageId = table.Column<int>(type: "int", nullable: false),
                Description = table.Column<string>(type: "NVARCHAR(250)", maxLength: 250, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductPropertyRuleTranslation", x => x.Id);
                table.ForeignKey(
                    name: "FK_ProductPropertyRuleTranslation_Language_LanguageId",
                    column: x => x.LanguageId,
                    principalTable: "Language",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ProductPropertyRuleTranslation_ProductPropertyRule_ProductPropertyRuleId",
                    column: x => x.ProductPropertyRuleId,
                    principalTable: "ProductPropertyRule",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_PropertyCategory_Code",
            table: "PropertyCategory",
            column: "Code",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Property_PropertyCategoryId_Code",
            table: "Property",
            columns: new[] { "PropertyCategoryId", "Code" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PropertyItem_PropertyId_Code",
            table: "PropertyItem",
            columns: new[] { "PropertyId", "Code" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ProductProperty_PropertyItemId",
            table: "ProductProperty",
            column: "PropertyItemId");

        migrationBuilder.CreateIndex(
            name: "IX_PropertyCategoryTranslation_PropertyCategoryId_LanguageId",
            table: "PropertyCategoryTranslation",
            columns: new[] { "PropertyCategoryId", "LanguageId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PropertyTranslation_PropertyId_LanguageId",
            table: "PropertyTranslation",
            columns: new[] { "PropertyId", "LanguageId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PropertyItemTranslation_PropertyItemId_LanguageId",
            table: "PropertyItemTranslation",
            columns: new[] { "PropertyItemId", "LanguageId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ProductPropertyRuleTranslation_ProductPropertyRuleId_LanguageId",
            table: "ProductPropertyRuleTranslation",
            columns: new[] { "ProductPropertyRuleId", "LanguageId" },
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_ProductProperty_PropertyItem_PropertyItemId",
            table: "ProductProperty",
            column: "PropertyItemId",
            principalTable: "PropertyItem",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.Sql(
            """
            DECLARE @DefaultLanguageId INT = (SELECT TOP 1 Id FROM Language WHERE IsDefault = 1);
            IF @DefaultLanguageId IS NULL
                SET @DefaultLanguageId = (SELECT TOP 1 Id FROM Language ORDER BY Id);

            IF @DefaultLanguageId IS NOT NULL
            BEGIN
                INSERT INTO PropertyCategoryTranslation (PropertyCategoryId, LanguageId, Title)
                SELECT pc.Id, @DefaultLanguageId, pc.Title
                FROM PropertyCategory pc;

                INSERT INTO PropertyTranslation (PropertyId, LanguageId, Title, Description)
                SELECT p.Id, @DefaultLanguageId, p.Title, LEFT(p.Description, 250)
                FROM Property p;

                INSERT INTO PropertyItemTranslation (PropertyItemId, LanguageId, Title)
                SELECT pi.Id, @DefaultLanguageId, pi.Title
                FROM PropertyItem pi;

                INSERT INTO ProductPropertyRuleTranslation (ProductPropertyRuleId, LanguageId, Description)
                SELECT ppr.Id, @DefaultLanguageId, LEFT(ppr.Description, 250)
                FROM ProductPropertyRule ppr
                WHERE ppr.Description IS NOT NULL;
            END
            """);

        migrationBuilder.DropColumn(
            name: "Title",
            table: "PropertyCategory");

        migrationBuilder.DropColumn(
            name: "Title",
            table: "Property");

        migrationBuilder.DropColumn(
            name: "Description",
            table: "Property");

        migrationBuilder.DropColumn(
            name: "Title",
            table: "PropertyItem");

        migrationBuilder.DropColumn(
            name: "Description",
            table: "ProductPropertyRule");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ProductProperty_PropertyItem_PropertyItemId",
            table: "ProductProperty");

        migrationBuilder.DropTable(
            name: "PropertyCategoryTranslation");

        migrationBuilder.DropTable(
            name: "PropertyTranslation");

        migrationBuilder.DropTable(
            name: "PropertyItemTranslation");

        migrationBuilder.DropTable(
            name: "ProductPropertyRuleTranslation");

        migrationBuilder.DropIndex(
            name: "IX_PropertyCategory_Code",
            table: "PropertyCategory");

        migrationBuilder.DropIndex(
            name: "IX_Property_PropertyCategoryId_Code",
            table: "Property");

        migrationBuilder.DropIndex(
            name: "IX_PropertyItem_PropertyId_Code",
            table: "PropertyItem");

        migrationBuilder.DropIndex(
            name: "IX_ProductProperty_PropertyItemId",
            table: "ProductProperty");

        migrationBuilder.DropColumn(
            name: "PropertyItemId",
            table: "ProductProperty");

        migrationBuilder.DropColumn(
            name: "Code",
            table: "PropertyCategory");

        migrationBuilder.DropColumn(
            name: "Code",
            table: "Property");

        migrationBuilder.DropColumn(
            name: "Code",
            table: "PropertyItem");

        migrationBuilder.AddColumn<string>(
            name: "Title",
            table: "PropertyCategory",
            type: "NVARCHAR(30)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Title",
            table: "Property",
            type: "NVARCHAR(30)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Description",
            table: "Property",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Title",
            table: "PropertyItem",
            type: "NVARCHAR(30)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Description",
            table: "ProductPropertyRule",
            type: "NVARCHAR(250)",
            maxLength: 250,
            nullable: true);
    }
}
