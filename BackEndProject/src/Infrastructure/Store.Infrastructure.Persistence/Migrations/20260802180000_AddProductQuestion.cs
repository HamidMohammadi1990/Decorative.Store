using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddProductQuestion : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ProductQuestion",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProductId = table.Column<int>(type: "int", nullable: false),
                UserId = table.Column<int>(type: "int", nullable: false),
                Question = table.Column<string>(type: "NVARCHAR(500)", nullable: false),
                Answer = table.Column<string>(type: "NVARCHAR(2500)", nullable: true),
                AnsweredByUserId = table.Column<int>(type: "int", nullable: true),
                CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductQuestion", x => x.Id);
                table.ForeignKey(
                    name: "FK_ProductQuestion_Product_ProductId",
                    column: x => x.ProductId,
                    principalTable: "Product",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ProductQuestion_User_AnsweredByUserId",
                    column: x => x.AnsweredByUserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ProductQuestion_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ProductQuestion_AnsweredByUserId",
            table: "ProductQuestion",
            column: "AnsweredByUserId");

        migrationBuilder.CreateIndex(
            name: "IX_ProductQuestion_ProductId",
            table: "ProductQuestion",
            column: "ProductId");

        migrationBuilder.CreateIndex(
            name: "IX_ProductQuestion_UserId",
            table: "ProductQuestion",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ProductQuestion");
    }
}
