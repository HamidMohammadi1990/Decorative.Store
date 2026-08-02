using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Infrastructure.Persistence.Migrations;

public partial class remove_company : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_BankTransaction_Company_CompanyId",
            table: "BankTransaction");

        migrationBuilder.DropForeignKey(
            name: "FK_ChequeTransaction_Company_CompanyId",
            table: "ChequeTransaction");

        migrationBuilder.DropForeignKey(
            name: "FK_Expense_Company_CompanyId",
            table: "Expense");

        migrationBuilder.DropForeignKey(
            name: "FK_FinancialYear_Company_CompanyId",
            table: "FinancialYear");

        migrationBuilder.DropForeignKey(
            name: "FK_OrderCommission_Company_CompanyId",
            table: "OrderCommission");

        migrationBuilder.DropForeignKey(
            name: "FK_ProductComment_Company_CompanyId",
            table: "ProductComment");

        migrationBuilder.DropForeignKey(
            name: "FK_ProductPropertyPrice_Company_CompanyId",
            table: "ProductPropertyPrice");

        migrationBuilder.DropForeignKey(
            name: "FK_PropertyItemPrice_Company_CompanyId",
            table: "PropertyItemPrice");

        migrationBuilder.DropForeignKey(
            name: "FK_Wallet_Company_CompanyId",
            table: "Wallet");

        migrationBuilder.DropForeignKey(
            name: "FK_WalletTransaction_Company_CompanyId",
            table: "WalletTransaction");

        migrationBuilder.DropTable(name: "PosTransaction");
        migrationBuilder.DropTable(name: "CompanyStoryLike");
        migrationBuilder.DropTable(name: "CompanyStoryComment");
        migrationBuilder.DropTable(name: "CompanyStoryItem");
        migrationBuilder.DropTable(name: "CompanyComment");
        migrationBuilder.DropTable(name: "CompanyPosDevice");
        migrationBuilder.DropTable(name: "CompanyStory");
        migrationBuilder.DropTable(name: "Company");

        migrationBuilder.DropIndex(name: "IX_WalletTransaction_CompanyId", table: "WalletTransaction");
        migrationBuilder.DropIndex(name: "IX_Wallet_CompanyId", table: "Wallet");
        migrationBuilder.DropIndex(name: "IX_PropertyItemPrice_CompanyId", table: "PropertyItemPrice");
        migrationBuilder.DropIndex(name: "IX_ProductPropertyPrice_CompanyId", table: "ProductPropertyPrice");
        migrationBuilder.DropIndex(name: "IX_ProductComment_CompanyId", table: "ProductComment");
        migrationBuilder.DropIndex(name: "IX_OrderCommission_CompanyId", table: "OrderCommission");
        migrationBuilder.DropIndex(name: "IX_FinancialYear_CompanyId", table: "FinancialYear");
        migrationBuilder.DropIndex(name: "IX_Expense_CompanyId", table: "Expense");
        migrationBuilder.DropIndex(name: "IX_ChequeTransaction_CompanyId", table: "ChequeTransaction");
        migrationBuilder.DropIndex(name: "IX_BankTransaction_CompanyId", table: "BankTransaction");

        migrationBuilder.DropColumn(name: "CompanyId", table: "WalletTransaction");
        migrationBuilder.DropColumn(name: "CompanyId", table: "Wallet");
        migrationBuilder.DropColumn(name: "CompanyId", table: "PropertyItemPrice");
        migrationBuilder.DropColumn(name: "CompanyId", table: "ProductPropertyPrice");
        migrationBuilder.DropColumn(name: "CompanyId", table: "ProductComment");
        migrationBuilder.DropColumn(name: "CompanyId", table: "OrderCommission");
        migrationBuilder.DropColumn(name: "CompanyId", table: "FinancialYear");
        migrationBuilder.DropColumn(name: "CompanyId", table: "Expense");
        migrationBuilder.DropColumn(name: "CompanyId", table: "ChequeTransaction");
        migrationBuilder.DropColumn(name: "CompanyId", table: "BankTransaction");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "CompanyId",
            table: "WalletTransaction",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "CompanyId",
            table: "Wallet",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "CompanyId",
            table: "PropertyItemPrice",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "CompanyId",
            table: "ProductPropertyPrice",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "CompanyId",
            table: "ProductComment",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "CompanyId",
            table: "OrderCommission",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "CompanyId",
            table: "FinancialYear",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "CompanyId",
            table: "Expense",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "CompanyId",
            table: "ChequeTransaction",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "CompanyId",
            table: "BankTransaction",
            type: "int",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Company",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<int>(type: "int", nullable: false),
                CityId = table.Column<int>(type: "int", nullable: false),
                Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                Code = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                PhoneNumber = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                Email = table.Column<string>(type: "varchar(35)", unicode: false, maxLength: 35, nullable: true),
                PostalCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                Address = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                Latitude = table.Column<float>(type: "real", nullable: false),
                Longitude = table.Column<float>(type: "real", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Company", x => x.Id);
                table.ForeignKey(
                    name: "FK_Company_City_CityId",
                    column: x => x.CityId,
                    principalTable: "City",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Company_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });
    }
}
