using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class user_story_comments_likes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "UserStoryComment",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<int>(type: "int", nullable: false),
                UserStoryId = table.Column<int>(type: "int", nullable: false),
                Content = table.Column<string>(type: "NVARCHAR(500)", nullable: false),
                ApprovedByUserId = table.Column<int>(type: "int", nullable: true),
                CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                ApprovedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsApproved = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserStoryComment", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserStoryComment_User_ApprovedByUserId",
                    column: x => x.ApprovedByUserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_UserStoryComment_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_UserStoryComment_UserStory_UserStoryId",
                    column: x => x.UserStoryId,
                    principalTable: "UserStory",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "UserStoryLike",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<int>(type: "int", nullable: false),
                UserStoryId = table.Column<int>(type: "int", nullable: false),
                CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserStoryLike", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserStoryLike_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_UserStoryLike_UserStory_UserStoryId",
                    column: x => x.UserStoryId,
                    principalTable: "UserStory",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_UserStoryComment_ApprovedByUserId",
            table: "UserStoryComment",
            column: "ApprovedByUserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserStoryComment_IsApproved",
            table: "UserStoryComment",
            column: "IsApproved");

        migrationBuilder.CreateIndex(
            name: "IX_UserStoryComment_UserId",
            table: "UserStoryComment",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserStoryComment_UserStoryId",
            table: "UserStoryComment",
            column: "UserStoryId");

        migrationBuilder.CreateIndex(
            name: "IX_UserStoryLike_UserStoryId",
            table: "UserStoryLike",
            column: "UserStoryId");

        migrationBuilder.CreateIndex(
            name: "IX_UserStoryLike_UserId_UserStoryId",
            table: "UserStoryLike",
            columns: new[] { "UserId", "UserStoryId" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UserStoryLike");

        migrationBuilder.DropTable(
            name: "UserStoryComment");
    }
}
