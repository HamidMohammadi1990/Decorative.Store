using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class add_content_policy_query_action : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_IsActive",
            table: "ContentPolicy");

        migrationBuilder.AddColumn<string>(
            name: "QueryAction",
            table: "ContentPolicy",
            type: "VARCHAR(100)",
            unicode: false,
            maxLength: 100,
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy",
            columns: new[] { "RoleId", "EntityType", "QueryAction", "IsActive" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy");

        migrationBuilder.DropColumn(
            name: "QueryAction",
            table: "ContentPolicy");

        migrationBuilder.CreateIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_IsActive",
            table: "ContentPolicy",
            columns: new[] { "RoleId", "EntityType", "IsActive" });
    }
}
