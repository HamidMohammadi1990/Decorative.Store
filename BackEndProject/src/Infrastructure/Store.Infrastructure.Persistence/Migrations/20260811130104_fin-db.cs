using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class findb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserStory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(70)", nullable: false),
                    Caption = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    MediaType = table.Column<byte>(type: "tinyint", nullable: false),
                    MediaPath = table.Column<string>(type: "NVARCHAR(260)", nullable: false),
                    MediaAlt = table.Column<string>(type: "NVARCHAR(120)", nullable: false),
                    PosterPath = table.Column<string>(type: "NVARCHAR(260)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStory_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserStory_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStory_IsActive_CreatedOnUtc",
                table: "UserStory",
                columns: new[] { "IsActive", "CreatedOnUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_UserStory_ProductId",
                table: "UserStory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStory_UserId",
                table: "UserStory",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserStory");
        }
    }
}
