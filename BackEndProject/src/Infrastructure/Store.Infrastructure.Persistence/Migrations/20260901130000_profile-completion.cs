using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class profile_completion : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ProfileCompletionSetting",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ConfigJson = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProfileCompletionSetting", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProfileCompletionUserState",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<int>(type: "int", nullable: false),
                AnswersJson = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                RewardClaimedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProfileCompletionUserState", x => x.Id);
                table.ForeignKey(
                    name: "FK_ProfileCompletionUserState_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ProfileCompletionUserState_UserId",
            table: "ProfileCompletionUserState",
            column: "UserId",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "ProfileCompletionUserState");
        migrationBuilder.DropTable(name: "ProfileCompletionSetting");
    }
}
