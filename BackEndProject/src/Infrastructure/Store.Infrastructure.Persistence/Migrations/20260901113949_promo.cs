using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class promo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemAttachment_OrderItem_OrderItemId",
                table: "OrderItemAttachment");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemAttachment_OrderItem_OrderItemId",
                table: "OrderItemAttachment",
                column: "OrderItemId",
                principalTable: "OrderItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemAttachment_OrderItem_OrderItemId",
                table: "OrderItemAttachment");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemAttachment_OrderItem_OrderItemId",
                table: "OrderItemAttachment",
                column: "OrderItemId",
                principalTable: "OrderItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
