using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edition.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addtitlelengthproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Product",
                type: "NVARCHAR(150)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(80)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Product",
                type: "NVARCHAR(80)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(150)");
        }
    }
}
