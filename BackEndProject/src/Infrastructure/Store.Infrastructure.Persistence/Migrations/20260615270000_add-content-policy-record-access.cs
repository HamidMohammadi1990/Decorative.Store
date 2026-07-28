using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class add_content_policy_record_access : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ContentPolicyRecordAccess",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PolicyId = table.Column<int>(type: "int", nullable: false),
                EntityId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ContentPolicyRecordAccess", x => x.Id);
                table.ForeignKey(
                    name: "FK_ContentPolicyRecordAccess_ContentPolicy_PolicyId",
                    column: x => x.PolicyId,
                    principalTable: "ContentPolicy",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ContentPolicyRecordAccess_PolicyId",
            table: "ContentPolicyRecordAccess",
            column: "PolicyId");

        migrationBuilder.CreateIndex(
            name: "IX_ContentPolicyRecordAccess_PolicyId_EntityId",
            table: "ContentPolicyRecordAccess",
            columns: new[] { "PolicyId", "EntityId" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ContentPolicyRecordAccess");
    }
}
