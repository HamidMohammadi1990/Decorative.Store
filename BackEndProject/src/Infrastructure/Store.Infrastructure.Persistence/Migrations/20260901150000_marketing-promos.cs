using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class marketing_promos : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "MarketingPromo",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                LanguageId = table.Column<int>(type: "int", nullable: false),
                PromoType = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                Subtitle = table.Column<string>(type: "NVARCHAR(200)", nullable: true),
                LinkLabel = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                LinkHref = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                ImageFileName = table.Column<string>(type: "NVARCHAR(35)", nullable: false),
                Priority = table.Column<int>(type: "int", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MarketingPromo", x => x.Id);
                table.ForeignKey(
                    name: "FK_MarketingPromo_Language_LanguageId",
                    column: x => x.LanguageId,
                    principalTable: "Language",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "MarketingStripDisclaimer",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                LanguageId = table.Column<int>(type: "int", nullable: false),
                Disclaimer = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                DisclaimerLinkLabel = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                DisclaimerLinkHref = table.Column<string>(type: "NVARCHAR(200)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MarketingStripDisclaimer", x => x.Id);
                table.ForeignKey(
                    name: "FK_MarketingStripDisclaimer_Language_LanguageId",
                    column: x => x.LanguageId,
                    principalTable: "Language",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_MarketingPromo_LanguageId_Priority",
            table: "MarketingPromo",
            columns: new[] { "LanguageId", "Priority" });

        migrationBuilder.CreateIndex(
            name: "IX_MarketingStripDisclaimer_LanguageId",
            table: "MarketingStripDisclaimer",
            column: "LanguageId",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "MarketingPromo");
        migrationBuilder.DropTable(name: "MarketingStripDisclaimer");
    }
}
