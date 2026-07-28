using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edition.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class increasetitlelengthinsubandcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SubCategory",
                type: "NVARCHAR(60)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(30)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Category",
                type: "NVARCHAR(60)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(30)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SubCategory",
                type: "NVARCHAR(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(60)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Category",
                type: "NVARCHAR(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(60)");
        }
    }
}
