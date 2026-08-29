using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removeisuniqueinProductDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductDescription_ProductId_LanguageId",
                table: "ProductDescription");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDescription_ProductId_LanguageId",
                table: "ProductDescription",
                columns: new[] { "ProductId", "LanguageId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductDescription_ProductId_LanguageId",
                table: "ProductDescription");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDescription_ProductId_LanguageId",
                table: "ProductDescription",
                columns: new[] { "ProductId", "LanguageId" },
                unique: true);
        }
    }
}
