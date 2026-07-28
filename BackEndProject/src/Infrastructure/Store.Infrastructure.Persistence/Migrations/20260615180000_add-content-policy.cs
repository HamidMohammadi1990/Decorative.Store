using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class add_content_policy : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "BypassContentPolicy",
            table: "Role",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.CreateTable(
            name: "ContentPolicy",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<int>(type: "int", nullable: false),
                EntityType = table.Column<int>(type: "int", nullable: false),
                Name = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                Priority = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ContentPolicy", x => x.Id);
                table.ForeignKey(
                    name: "FK_ContentPolicy_Role_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Role",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "ContentPolicyRule",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PolicyId = table.Column<int>(type: "int", nullable: false),
                FieldPath = table.Column<string>(type: "VARCHAR(150)", unicode: false, maxLength: 150, nullable: false),
                Operator = table.Column<int>(type: "int", nullable: false),
                ValueType = table.Column<int>(type: "int", nullable: false),
                Value = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                SortOrder = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ContentPolicyRule", x => x.Id);
                table.ForeignKey(
                    name: "FK_ContentPolicyRule_ContentPolicy_PolicyId",
                    column: x => x.PolicyId,
                    principalTable: "ContentPolicy",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_IsActive",
            table: "ContentPolicy",
            columns: new[] { "RoleId", "EntityType", "IsActive" });

        migrationBuilder.CreateIndex(
            name: "IX_ContentPolicyRule_PolicyId",
            table: "ContentPolicyRule",
            column: "PolicyId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ContentPolicyRule");

        migrationBuilder.DropTable(
            name: "ContentPolicy");

        migrationBuilder.DropColumn(
            name: "BypassContentPolicy",
            table: "Role");
    }
}
