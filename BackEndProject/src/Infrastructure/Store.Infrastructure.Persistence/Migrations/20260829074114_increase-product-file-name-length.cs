using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class increaseproductfilenamelength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "ProductFile",
                type: "VARCHAR(70)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(35)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "ProductFile",
                type: "VARCHAR(35)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(70)");
        }
    }
}
