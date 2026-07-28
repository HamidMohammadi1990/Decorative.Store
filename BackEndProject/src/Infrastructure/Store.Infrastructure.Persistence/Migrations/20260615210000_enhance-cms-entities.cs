using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class enhancecmsentities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "MetaDescription",
            table: "Page",
            type: "NVARCHAR(300)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "MetaTitle",
            table: "Page",
            type: "NVARCHAR(120)",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "Section",
            type: "bit",
            nullable: false,
            defaultValue: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "SectionItem",
            type: "bit",
            nullable: false,
            defaultValue: true);

        migrationBuilder.CreateIndex(
            name: "IX_Page_Slug",
            table: "Page",
            column: "Slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PageSection_PageId_SectionId",
            table: "PageSection",
            columns: new[] { "PageId", "SectionId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_SectionType_Name",
            table: "SectionType",
            column: "Name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_SectionType_Name",
            table: "SectionType");

        migrationBuilder.DropIndex(
            name: "IX_PageSection_PageId_SectionId",
            table: "PageSection");

        migrationBuilder.DropIndex(
            name: "IX_Page_Slug",
            table: "Page");

        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "SectionItem");

        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "Section");

        migrationBuilder.DropColumn(
            name: "MetaTitle",
            table: "Page");

        migrationBuilder.DropColumn(
            name: "MetaDescription",
            table: "Page");
    }
}
