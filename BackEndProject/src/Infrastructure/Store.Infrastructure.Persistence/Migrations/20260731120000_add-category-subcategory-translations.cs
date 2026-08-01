using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class add_category_subcategory_translations : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "CategoryTranslation",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CategoryId = table.Column<int>(type: "int", nullable: false),
                LanguageId = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "NVARCHAR(60)", maxLength: 60, nullable: false),
                Slug = table.Column<string>(type: "VARCHAR(150)", unicode: false, maxLength: 150, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CategoryTranslation", x => x.Id);
                table.ForeignKey(
                    name: "FK_CategoryTranslation_Category_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "Category",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_CategoryTranslation_Language_LanguageId",
                    column: x => x.LanguageId,
                    principalTable: "Language",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "SubCategoryTranslation",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                SubCategoryId = table.Column<int>(type: "int", nullable: false),
                LanguageId = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "NVARCHAR(60)", maxLength: 60, nullable: false),
                Slug = table.Column<string>(type: "VARCHAR(150)", unicode: false, maxLength: 150, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SubCategoryTranslation", x => x.Id);
                table.ForeignKey(
                    name: "FK_SubCategoryTranslation_Language_LanguageId",
                    column: x => x.LanguageId,
                    principalTable: "Language",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_SubCategoryTranslation_SubCategory_SubCategoryId",
                    column: x => x.SubCategoryId,
                    principalTable: "SubCategory",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_CategoryTranslation_CategoryId_LanguageId",
            table: "CategoryTranslation",
            columns: new[] { "CategoryId", "LanguageId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_CategoryTranslation_LanguageId_Slug",
            table: "CategoryTranslation",
            columns: new[] { "LanguageId", "Slug" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_SubCategoryTranslation_LanguageId_Slug",
            table: "SubCategoryTranslation",
            columns: new[] { "LanguageId", "Slug" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_SubCategoryTranslation_SubCategoryId_LanguageId",
            table: "SubCategoryTranslation",
            columns: new[] { "SubCategoryId", "LanguageId" },
            unique: true);

        migrationBuilder.Sql(
            """
            DECLARE @DefaultLanguageId INT = (SELECT TOP 1 Id FROM Language WHERE IsDefault = 1);
            IF @DefaultLanguageId IS NULL
                SET @DefaultLanguageId = (SELECT TOP 1 Id FROM Language ORDER BY Id);

            IF @DefaultLanguageId IS NOT NULL
            BEGIN
                INSERT INTO CategoryTranslation (CategoryId, LanguageId, Title, Slug)
                SELECT c.Id, @DefaultLanguageId, c.Title, c.Slug
                FROM Category c;

                INSERT INTO SubCategoryTranslation (SubCategoryId, LanguageId, Title, Slug)
                SELECT s.Id, @DefaultLanguageId, s.Title, s.Slug
                FROM SubCategory s;
            END
            """);

        migrationBuilder.DropIndex(
            name: "IX_Category_Slug",
            table: "Category");

        migrationBuilder.DropIndex(
            name: "IX_SubCategory_Slug",
            table: "SubCategory");

        migrationBuilder.DropColumn(
            name: "Slug",
            table: "Category");

        migrationBuilder.DropColumn(
            name: "Title",
            table: "Category");

        migrationBuilder.DropColumn(
            name: "Slug",
            table: "SubCategory");

        migrationBuilder.DropColumn(
            name: "Title",
            table: "SubCategory");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Slug",
            table: "SubCategory",
            type: "VARCHAR(150)",
            unicode: false,
            maxLength: 150,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Title",
            table: "SubCategory",
            type: "NVARCHAR(60)",
            maxLength: 60,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Slug",
            table: "Category",
            type: "VARCHAR(150)",
            unicode: false,
            maxLength: 150,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Title",
            table: "Category",
            type: "NVARCHAR(60)",
            maxLength: 60,
            nullable: false,
            defaultValue: "");

        migrationBuilder.Sql(
            """
            DECLARE @DefaultLanguageId INT = (SELECT TOP 1 Id FROM Language WHERE IsDefault = 1);
            IF @DefaultLanguageId IS NULL
                SET @DefaultLanguageId = (SELECT TOP 1 Id FROM Language ORDER BY Id);

            IF @DefaultLanguageId IS NOT NULL
            BEGIN
                UPDATE c
                SET c.Title = ct.Title,
                    c.Slug = ct.Slug
                FROM Category c
                INNER JOIN CategoryTranslation ct ON ct.CategoryId = c.Id AND ct.LanguageId = @DefaultLanguageId;

                UPDATE s
                SET s.Title = st.Title,
                    s.Slug = st.Slug
                FROM SubCategory s
                INNER JOIN SubCategoryTranslation st ON st.SubCategoryId = s.Id AND st.LanguageId = @DefaultLanguageId;
            END
            """);

        migrationBuilder.DropTable(
            name: "SubCategoryTranslation");

        migrationBuilder.DropTable(
            name: "CategoryTranslation");

        migrationBuilder.CreateIndex(
            name: "IX_SubCategory_Slug",
            table: "SubCategory",
            column: "Slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Category_Slug",
            table: "Category",
            column: "Slug",
            unique: true);
    }
}
