using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseCategory",
                columns: table => new
                {
                    ExpenseCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseCategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategory_ExpenseCategoryId", x => x.ExpenseCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseType",
                columns: table => new
                {
                    ExpenseTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseType_ExpenseTypeId", x => x.ExpenseTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Family",
                columns: table => new
                {
                    FamilyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamilyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Family_FamilyId", x => x.FamilyId);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "Guest"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AdObjId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FamilyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile_UserId", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserProfile_Family",
                        column: x => x.FamilyId,
                        principalTable: "Family",
                        principalColumn: "FamilyId");
                });

            migrationBuilder.CreateTable(
                name: "CreditCard",
                columns: table => new
                {
                    CreditCardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardLastFourDigit = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    CreditCardName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCard_CreditCardId", x => x.CreditCardId);
                    table.ForeignKey(
                        name: "FK_CreditCard_UserProfile",
                        column: x => x.UserId,
                        principalTable: "UserProfile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Expense",
                columns: table => new
                {
                    ExpenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ExpenseAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpenseCategoryId = table.Column<int>(type: "int", nullable: true),
                    ExpenseTypeId = table.Column<int>(type: "int", nullable: true),
                    CreditCardId = table.Column<int>(type: "int", nullable: true),
                    ExpenseDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExpenseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense_ExpenseId", x => x.ExpenseId);
                    table.ForeignKey(
                        name: "FK_Expense_Category",
                        column: x => x.ExpenseCategoryId,
                        principalTable: "ExpenseCategory",
                        principalColumn: "ExpenseCategoryId");
                    table.ForeignKey(
                        name: "FK_Expense_CreditCard",
                        column: x => x.CreditCardId,
                        principalTable: "CreditCard",
                        principalColumn: "CreditCardId");
                    table.ForeignKey(
                        name: "FK_Expense_Type",
                        column: x => x.ExpenseTypeId,
                        principalTable: "ExpenseType",
                        principalColumn: "ExpenseTypeId");
                    table.ForeignKey(
                        name: "FK_Expense_User",
                        column: x => x.UserId,
                        principalTable: "UserProfile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditCard_UserId",
                table: "CreditCard",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IDX_Expense_UserId",
                table: "Expense",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_CreditCardId",
                table: "Expense",
                column: "CreditCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_ExpenseCategoryId",
                table: "Expense",
                column: "ExpenseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_ExpenseTypeId",
                table: "Expense",
                column: "ExpenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IDX_Expense_FamilyId",
                table: "UserProfile",
                column: "FamilyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expense");

            migrationBuilder.DropTable(
                name: "ExpenseCategory");

            migrationBuilder.DropTable(
                name: "CreditCard");

            migrationBuilder.DropTable(
                name: "ExpenseType");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "Family");
        }
    }
}
