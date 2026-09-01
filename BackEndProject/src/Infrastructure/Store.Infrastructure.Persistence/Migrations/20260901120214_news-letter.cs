using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class newsletter : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "NewsletterSubscriber",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Email = table.Column<string>(type: "VARCHAR(256)", nullable: false),
                LanguageId = table.Column<int>(type: "int", nullable: false),
                SubscribedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_NewsletterSubscriber", x => x.Id);
                table.ForeignKey(
                    name: "FK_NewsletterSubscriber_Language_LanguageId",
                    column: x => x.LanguageId,
                    principalTable: "Language",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_NewsletterSubscriber_Email",
            table: "NewsletterSubscriber",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_NewsletterSubscriber_LanguageId_SubscribedAtUtc",
            table: "NewsletterSubscriber",
            columns: new[] { "LanguageId", "SubscribedAtUtc" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "NewsletterSubscriber");
    }
}
