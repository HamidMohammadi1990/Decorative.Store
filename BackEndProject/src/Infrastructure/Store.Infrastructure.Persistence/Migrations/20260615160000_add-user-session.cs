using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class addusersession : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "UserSession",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<int>(type: "int", nullable: false),
                CurrentJwtId = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                IpAddress = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                UserAgent = table.Column<string>(type: "varchar(512)", unicode: false, maxLength: 512, nullable: true),
                DeviceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastSeenOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                ExpiresOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                RevokedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                RevokedReason = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserSession", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserSession_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.AddColumn<Guid>(
            name: "UserSessionId",
            table: "RefreshToken",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_RefreshToken_UserSessionId",
            table: "RefreshToken",
            column: "UserSessionId");

        migrationBuilder.CreateIndex(
            name: "IX_UserSession_UserId",
            table: "UserSession",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserSession_UserId_IsRevoked",
            table: "UserSession",
            columns: new[] { "UserId", "IsRevoked" });

        migrationBuilder.AddForeignKey(
            name: "FK_RefreshToken_UserSession_UserSessionId",
            table: "RefreshToken",
            column: "UserSessionId",
            principalTable: "UserSession",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_RefreshToken_UserSession_UserSessionId",
            table: "RefreshToken");

        migrationBuilder.DropIndex(
            name: "IX_RefreshToken_UserSessionId",
            table: "RefreshToken");

        migrationBuilder.DropColumn(
            name: "UserSessionId",
            table: "RefreshToken");

        migrationBuilder.DropTable(
            name: "UserSession");
    }
}
