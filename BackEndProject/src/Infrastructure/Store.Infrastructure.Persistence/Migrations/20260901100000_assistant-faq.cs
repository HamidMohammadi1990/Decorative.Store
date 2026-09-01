using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class assistant_faq : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AssistantFaq",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                LanguageId = table.Column<int>(type: "int", nullable: false),
                Question = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                Answer = table.Column<string>(type: "NVARCHAR(2000)", nullable: false),
                Priority = table.Column<int>(type: "int", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AssistantFaq", x => x.Id);
                table.ForeignKey(
                    name: "FK_AssistantFaq_Language_LanguageId",
                    column: x => x.LanguageId,
                    principalTable: "Language",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AssistantFaq_LanguageId_Priority",
            table: "AssistantFaq",
            columns: new[] { "LanguageId", "Priority" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "AssistantFaq");
    }
}
