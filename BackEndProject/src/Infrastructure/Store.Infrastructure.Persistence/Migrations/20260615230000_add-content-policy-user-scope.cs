using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class add_content_policy_user_scope : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy");

        migrationBuilder.AlterColumn<int>(
            name: "RoleId",
            table: "ContentPolicy",
            type: "int",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddColumn<int>(
            name: "UserId",
            table: "ContentPolicy",
            type: "int",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy",
            columns: new[] { "RoleId", "EntityType", "QueryAction", "IsActive" });

        migrationBuilder.CreateIndex(
            name: "IX_ContentPolicy_UserId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy",
            columns: new[] { "UserId", "EntityType", "QueryAction", "IsActive" });

        migrationBuilder.AddForeignKey(
            name: "FK_ContentPolicy_User_UserId",
            table: "ContentPolicy",
            column: "UserId",
            principalTable: "User",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddCheckConstraint(
            name: "CK_ContentPolicy_Scope",
            table: "ContentPolicy",
            sql: "([RoleId] IS NOT NULL AND [UserId] IS NULL) OR ([RoleId] IS NULL AND [UserId] IS NOT NULL)");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ContentPolicy_User_UserId",
            table: "ContentPolicy");

        migrationBuilder.DropCheckConstraint(
            name: "CK_ContentPolicy_Scope",
            table: "ContentPolicy");

        migrationBuilder.DropIndex(
            name: "IX_ContentPolicy_UserId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy");

        migrationBuilder.DropIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy");

        migrationBuilder.DropColumn(
            name: "UserId",
            table: "ContentPolicy");

        migrationBuilder.AlterColumn<int>(
            name: "RoleId",
            table: "ContentPolicy",
            type: "int",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_ContentPolicy_RoleId_EntityType_QueryAction_IsActive",
            table: "ContentPolicy",
            columns: new[] { "RoleId", "EntityType", "QueryAction", "IsActive" });
    }
}
