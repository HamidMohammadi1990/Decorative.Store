using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAddressDefaultAndApartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Apartment",
                table: "UserAddress",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "UserAddress",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql("""
                ;WITH RankedAddresses AS (
                    SELECT Id,
                           ROW_NUMBER() OVER (PARTITION BY UserId ORDER BY Id) AS RowNum
                    FROM UserAddress
                )
                UPDATE ua
                SET IsDefault = 1
                FROM UserAddress ua
                INNER JOIN RankedAddresses r ON ua.Id = r.Id
                WHERE r.RowNum = 1;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_UserId_IsDefault",
                table: "UserAddress",
                columns: new[] { "UserId", "IsDefault" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserAddress_UserId_IsDefault",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "Apartment",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "UserAddress");
        }
    }
}
