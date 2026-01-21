using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget_Tracker_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyMasterId",
                table: "user_master",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CurrencyMasters",
                columns: table => new
                {
                    CurrencyMasterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CurrencyCode = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CurrencyName = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CurrencySymbol = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyMasters", x => x.CurrencyMasterId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_user_master_CurrencyMasterId",
                table: "user_master",
                column: "CurrencyMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_master_CurrencyMasters_CurrencyMasterId",
                table: "user_master",
                column: "CurrencyMasterId",
                principalTable: "CurrencyMasters",
                principalColumn: "CurrencyMasterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_master_CurrencyMasters_CurrencyMasterId",
                table: "user_master");

            migrationBuilder.DropTable(
                name: "CurrencyMasters");

            migrationBuilder.DropIndex(
                name: "IX_user_master_CurrencyMasterId",
                table: "user_master");

            migrationBuilder.DropColumn(
                name: "CurrencyMasterId",
                table: "user_master");
        }
    }
}
