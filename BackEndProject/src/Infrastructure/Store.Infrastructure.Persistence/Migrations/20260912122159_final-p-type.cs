using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class finalptype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Kind was never added — product-file-type migration was empty.
            // Add FileTypeId directly instead of renaming a missing column.
            migrationBuilder.AddColumn<byte>(
                name: "FileTypeId",
                table: "ProductFile",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileTypeId",
                table: "ProductFile");
        }
    }
}
