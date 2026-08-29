using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class finaldbchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "ProductComment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "ProductComment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductCommentReaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCommentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsHelpful = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCommentReaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCommentReaction_ProductComment_ProductCommentId",
                        column: x => x.ProductCommentId,
                        principalTable: "ProductComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCommentReaction_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductComment_ParentId",
                table: "ProductComment",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCommentReaction_ProductCommentId",
                table: "ProductCommentReaction",
                column: "ProductCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCommentReaction_UserId_ProductCommentId",
                table: "ProductCommentReaction",
                columns: new[] { "UserId", "ProductCommentId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComment_ProductComment_ParentId",
                table: "ProductComment",
                column: "ParentId",
                principalTable: "ProductComment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComment_ProductComment_ParentId",
                table: "ProductComment");

            migrationBuilder.DropTable(
                name: "ProductCommentReaction");

            migrationBuilder.DropIndex(
                name: "IX_ProductComment_ParentId",
                table: "ProductComment");

            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "ProductComment");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ProductComment");
        }
    }
}
