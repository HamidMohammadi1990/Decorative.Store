using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class expand_content_policy_engine : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "RequireContentPolicy",
            table: "Role",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<int>(
            name: "Effect",
            table: "ContentPolicy",
            type: "int",
            nullable: false,
            defaultValue: 1);

        migrationBuilder.AddColumn<int>(
            name: "RuleGroup",
            table: "ContentPolicyRule",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "RequireContentPolicy",
            table: "Role");

        migrationBuilder.DropColumn(
            name: "Effect",
            table: "ContentPolicy");

        migrationBuilder.DropColumn(
            name: "RuleGroup",
            table: "ContentPolicyRule");
    }
}
