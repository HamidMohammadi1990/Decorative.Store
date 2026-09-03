using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class room_types : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "RoomType",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Code = table.Column<string>(type: "VARCHAR(32)", nullable: false),
                ImageFileName = table.Column<string>(type: "NVARCHAR(120)", nullable: false),
                Priority = table.Column<int>(type: "int", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoomType", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "RoomTypeTranslation",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoomTypeId = table.Column<int>(type: "int", nullable: false),
                LanguageId = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "NVARCHAR(100)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoomTypeTranslation", x => x.Id);
                table.ForeignKey(
                    name: "FK_RoomTypeTranslation_Language_LanguageId",
                    column: x => x.LanguageId,
                    principalTable: "Language",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_RoomTypeTranslation_RoomType_RoomTypeId",
                    column: x => x.RoomTypeId,
                    principalTable: "RoomType",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_RoomType_Code",
            table: "RoomType",
            column: "Code",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_RoomType_Priority",
            table: "RoomType",
            column: "Priority");

        migrationBuilder.CreateIndex(
            name: "IX_RoomTypeTranslation_LanguageId",
            table: "RoomTypeTranslation",
            column: "LanguageId");

        migrationBuilder.CreateIndex(
            name: "IX_RoomTypeTranslation_RoomTypeId_LanguageId",
            table: "RoomTypeTranslation",
            columns: new[] { "RoomTypeId", "LanguageId" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "RoomTypeTranslation");
        migrationBuilder.DropTable(name: "RoomType");
    }
}
