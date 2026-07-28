using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class expand_user_password_hash : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "PasswordHash",
            table: "User",
            type: "VARCHAR(256)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "VARCHAR(50)");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "PasswordHash",
            table: "User",
            type: "VARCHAR(50)",
            maxLength: 50,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "VARCHAR(256)");
    }
}
