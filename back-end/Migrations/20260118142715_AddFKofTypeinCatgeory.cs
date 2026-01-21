using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget_Tracker_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFKofTypeinCatgeory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionTypeMasterId",
                table: "transaction_category_master",
                type: "int(11)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_transaction_category_master_TransactionTypeMasterId",
                table: "transaction_category_master",
                column: "TransactionTypeMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_category_master_transaction_type_master_Transact~",
                table: "transaction_category_master",
                column: "TransactionTypeMasterId",
                principalTable: "transaction_type_master",
                principalColumn: "transaction_type_master_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transaction_category_master_transaction_type_master_Transact~",
                table: "transaction_category_master");

            migrationBuilder.DropIndex(
                name: "IX_transaction_category_master_TransactionTypeMasterId",
                table: "transaction_category_master");

            migrationBuilder.DropColumn(
                name: "TransactionTypeMasterId",
                table: "transaction_category_master");
        }
    }
}
