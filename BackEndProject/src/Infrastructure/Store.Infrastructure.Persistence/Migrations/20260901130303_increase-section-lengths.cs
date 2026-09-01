using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class increasesectionlengths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "SectionTranslation",
                type: "NVARCHAR(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(150)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SectionTranslation",
                type: "NVARCHAR(300)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(80)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SectionTranslation",
                type: "NVARCHAR(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(160)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "SectionTranslation",
                type: "NVARCHAR(150)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(200)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SectionTranslation",
                type: "NVARCHAR(80)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(300)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SectionTranslation",
                type: "NVARCHAR(160)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(500)",
                oldNullable: true);
        }
    }
}
