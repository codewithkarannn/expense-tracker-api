using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget_Tracker_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedBudgetMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "BudgetMaster",
                columns: table => new
                {
                    BudgetMasterId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TransactionCategoryMasterId = table.Column<int>(type: "int(11)", nullable: true),
                    UserMasterId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetMaster", x => x.BudgetMasterId);
                    table.ForeignKey(
                        name: "FK_BudgetMaster_transaction_category_master_TransactionCategory~",
                        column: x => x.TransactionCategoryMasterId,
                        principalTable: "transaction_category_master",
                        principalColumn: "transaction_category_master_id");
                    table.ForeignKey(
                        name: "FK_BudgetMaster_user_master_UserMasterId",
                        column: x => x.UserMasterId,
                        principalTable: "user_master",
                        principalColumn: "UserMasterID");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetMaster_TransactionCategoryMasterId",
                table: "BudgetMaster",
                column: "TransactionCategoryMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetMaster_UserMasterId",
                table: "BudgetMaster",
                column: "UserMasterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetMaster");

            migrationBuilder.AddColumn<string>(
                name: "UserMastercol",
                table: "user_master",
                type: "varchar(45)",
                maxLength: 45,
                nullable: true,
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
