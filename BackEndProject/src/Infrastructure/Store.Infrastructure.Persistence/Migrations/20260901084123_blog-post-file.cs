using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class blogpostfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogPostFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogPostId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "VARCHAR(70)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPostFile_BlogPost_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "BlogPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostFileTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogPostFileId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostFileTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPostFileTranslation_BlogPostFile_BlogPostFileId",
                        column: x => x.BlogPostFileId,
                        principalTable: "BlogPostFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPostFileTranslation_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostFile_BlogPostId",
                table: "BlogPostFile",
                column: "BlogPostId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostFileTranslation_BlogPostFileId_LanguageId",
                table: "BlogPostFileTranslation",
                columns: new[] { "BlogPostFileId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostFileTranslation_LanguageId",
                table: "BlogPostFileTranslation",
                column: "LanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPostFileTranslation");

            migrationBuilder.DropTable(
                name: "BlogPostFile");
        }
    }
}
