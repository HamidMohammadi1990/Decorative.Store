using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class add_product_translations : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ProductTranslation",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProductId = table.Column<int>(type: "int", nullable: false),
                LanguageId = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                Slug = table.Column<string>(type: "VARCHAR(150)", unicode: false, maxLength: 150, nullable: false),
                Description = table.Column<string>(type: "NVARCHAR(400)", maxLength: 400, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductTranslation", x => x.Id);
                table.ForeignKey(
                    name: "FK_ProductTranslation_Language_LanguageId",
                    column: x => x.LanguageId,
                    principalTable: "Language",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ProductTranslation_Product_ProductId",
                    column: x => x.ProductId,
                    principalTable: "Product",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ProductFileTranslation",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProductFileId = table.Column<int>(type: "int", nullable: false),
                LanguageId = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductFileTranslation", x => x.Id);
                table.ForeignKey(
                    name: "FK_ProductFileTranslation_Language_LanguageId",
                    column: x => x.LanguageId,
                    principalTable: "Language",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ProductFileTranslation_ProductFile_ProductFileId",
                    column: x => x.ProductFileId,
                    principalTable: "ProductFile",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ProductTranslation_LanguageId_Slug",
            table: "ProductTranslation",
            columns: new[] { "LanguageId", "Slug" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ProductTranslation_ProductId_LanguageId",
            table: "ProductTranslation",
            columns: new[] { "ProductId", "LanguageId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ProductFileTranslation_ProductFileId_LanguageId",
            table: "ProductFileTranslation",
            columns: new[] { "ProductFileId", "LanguageId" },
            unique: true);

        migrationBuilder.Sql(
            """
            DECLARE @DefaultLanguageId INT = (SELECT TOP 1 Id FROM Language WHERE IsDefault = 1);
            IF @DefaultLanguageId IS NULL
                SET @DefaultLanguageId = (SELECT TOP 1 Id FROM Language ORDER BY Id);

            IF @DefaultLanguageId IS NOT NULL
            BEGIN
                INSERT INTO ProductTranslation (ProductId, LanguageId, Title, Slug, Description)
                SELECT p.Id, @DefaultLanguageId, p.Title, p.Slug, p.Description
                FROM Product p;

                INSERT INTO ProductFileTranslation (ProductFileId, LanguageId, Title)
                SELECT pf.Id, @DefaultLanguageId, pf.Title
                FROM ProductFile pf;
            END
            """);

        migrationBuilder.AddColumn<int>(
            name: "LanguageId",
            table: "ProductDescription",
            type: "int",
            nullable: true);

        migrationBuilder.Sql(
            """
            DECLARE @DefaultLanguageId INT = (SELECT TOP 1 Id FROM Language WHERE IsDefault = 1);
            IF @DefaultLanguageId IS NULL
                SET @DefaultLanguageId = (SELECT TOP 1 Id FROM Language ORDER BY Id);

            IF @DefaultLanguageId IS NOT NULL
            BEGIN
                UPDATE ProductDescription SET LanguageId = @DefaultLanguageId WHERE LanguageId IS NULL;

                ;WITH Duplicates AS (
                    SELECT Id,
                           ROW_NUMBER() OVER (PARTITION BY ProductId, LanguageId ORDER BY Id) AS RowNum
                    FROM ProductDescription
                )
                DELETE FROM ProductDescription
                WHERE Id IN (SELECT Id FROM Duplicates WHERE RowNum > 1);
            END
            """);

        migrationBuilder.AlterColumn<int>(
            name: "LanguageId",
            table: "ProductDescription",
            type: "int",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_ProductDescription_LanguageId",
            table: "ProductDescription",
            column: "LanguageId");

        migrationBuilder.CreateIndex(
            name: "IX_ProductDescription_ProductId_LanguageId",
            table: "ProductDescription",
            columns: new[] { "ProductId", "LanguageId" },
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_ProductDescription_Language_LanguageId",
            table: "ProductDescription",
            column: "LanguageId",
            principalTable: "Language",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.DropIndex(
            name: "IX_Product_Title",
            table: "Product");

        migrationBuilder.DropIndex(
            name: "IX_Product_Slug",
            table: "Product");

        migrationBuilder.DropColumn(
            name: "Description",
            table: "Product");

        migrationBuilder.DropColumn(
            name: "Slug",
            table: "Product");

        migrationBuilder.DropColumn(
            name: "Title",
            table: "Product");

        migrationBuilder.DropColumn(
            name: "Title",
            table: "ProductFile");

        migrationBuilder.DropIndex(
            name: "IX_ProductDescription_ProductId",
            table: "ProductDescription");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Title",
            table: "ProductFile",
            type: "NVARCHAR(30)",
            maxLength: 30,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Description",
            table: "Product",
            type: "NVARCHAR(400)",
            maxLength: 400,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Slug",
            table: "Product",
            type: "VARCHAR(150)",
            unicode: false,
            maxLength: 150,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Title",
            table: "Product",
            type: "NVARCHAR(150)",
            maxLength: 150,
            nullable: false,
            defaultValue: "");

        migrationBuilder.Sql(
            """
            DECLARE @DefaultLanguageId INT = (SELECT TOP 1 Id FROM Language WHERE IsDefault = 1);
            IF @DefaultLanguageId IS NULL
                SET @DefaultLanguageId = (SELECT TOP 1 Id FROM Language ORDER BY Id);

            IF @DefaultLanguageId IS NOT NULL
            BEGIN
                UPDATE p
                SET p.Title = pt.Title,
                    p.Slug = pt.Slug,
                    p.Description = pt.Description
                FROM Product p
                INNER JOIN ProductTranslation pt ON pt.ProductId = p.Id AND pt.LanguageId = @DefaultLanguageId;

                UPDATE pf
                SET pf.Title = pft.Title
                FROM ProductFile pf
                INNER JOIN ProductFileTranslation pft ON pft.ProductFileId = pf.Id AND pft.LanguageId = @DefaultLanguageId;
            END
            """);

        migrationBuilder.DropForeignKey(
            name: "FK_ProductDescription_Language_LanguageId",
            table: "ProductDescription");

        migrationBuilder.DropIndex(
            name: "IX_ProductDescription_ProductId_LanguageId",
            table: "ProductDescription");

        migrationBuilder.DropIndex(
            name: "IX_ProductDescription_LanguageId",
            table: "ProductDescription");

        migrationBuilder.DropColumn(
            name: "LanguageId",
            table: "ProductDescription");

        migrationBuilder.CreateIndex(
            name: "IX_ProductDescription_ProductId",
            table: "ProductDescription",
            column: "ProductId");

        migrationBuilder.DropTable(
            name: "ProductFileTranslation");

        migrationBuilder.DropTable(
            name: "ProductTranslation");

        migrationBuilder.CreateIndex(
            name: "IX_Product_Slug",
            table: "Product",
            column: "Slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Product_Title",
            table: "Product",
            column: "Title",
            unique: true);
    }
}
