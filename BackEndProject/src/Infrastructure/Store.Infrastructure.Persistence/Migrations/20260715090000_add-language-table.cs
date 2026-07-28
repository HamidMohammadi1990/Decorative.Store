using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class add_language_table : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Language",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Code = table.Column<string>(type: "VARCHAR(10)", unicode: false, maxLength: 10, nullable: false),
                Name = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                IsDefault = table.Column<bool>(type: "bit", nullable: false),
                DisplayOrder = table.Column<int>(type: "int", nullable: false),
                IsRtl = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Language", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Language_Code",
            table: "Language",
            column: "Code",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Language_IsDefault",
            table: "Language",
            column: "IsDefault",
            unique: true,
            filter: "[IsDefault] = 1");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Language");
    }
}
