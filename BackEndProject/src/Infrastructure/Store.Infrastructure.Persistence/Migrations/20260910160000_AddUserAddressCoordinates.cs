using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddUserAddressCoordinates : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>(
            name: "Latitude",
            table: "UserAddress",
            type: "decimal(10,7)",
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "Longitude",
            table: "UserAddress",
            type: "decimal(10,7)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Latitude",
            table: "UserAddress");

        migrationBuilder.DropColumn(
            name: "Longitude",
            table: "UserAddress");
    }
}
