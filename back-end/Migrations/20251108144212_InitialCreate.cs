using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget_Tracker_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_role_master",
                columns: table => new
                {
                    UserRoleMasterID = table.Column<int>(type: "int(11)", nullable: false),
                    UserRole = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.UserRoleMasterID);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "user_master",
                columns: table => new
                {
                    UserMasterID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FirstName = table.Column<string>(type: "varchar(145)", maxLength: 145, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(145)", maxLength: 145, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserRoleID = table.Column<int>(type: "int(11)", nullable: true),
                    UserEmail = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserPassword = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserMastercol = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<sbyte>(type: "tinyint(4)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.UserMasterID);
                    table.ForeignKey(
                        name: "RoleIdMaster",
                        column: x => x.UserRoleID,
                        principalTable: "user_role_master",
                        principalColumn: "UserRoleMasterID");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "transaction_category_master",
                columns: table => new
                {
                    transaction_category_master_id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    transaction_category_name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'"),
                    user_master_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.transaction_category_master_id);
                    table.ForeignKey(
                        name: "user_master_id",
                        column: x => x.user_master_id,
                        principalTable: "user_master",
                        principalColumn: "UserMasterID");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "transaction_payment_mode",
                columns: table => new
                {
                    paymentModeId = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    paymentMode = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'"),
                    userMasterId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.paymentModeId);
                    table.ForeignKey(
                        name: "usermasterID_FK",
                        column: x => x.userMasterId,
                        principalTable: "user_master",
                        principalColumn: "UserMasterID");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "transaction_type_master",
                columns: table => new
                {
                    transaction_type_master_id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    transaction_typename = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<sbyte>(type: "tinyint(4)", nullable: true, defaultValueSql: "'1'"),
                    user_master_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.transaction_type_master_id);
                    table.ForeignKey(
                        name: "user_master_id_fk",
                        column: x => x.user_master_id,
                        principalTable: "user_master",
                        principalColumn: "UserMasterID");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "transaction_master",
                columns: table => new
                {
                    transaction_master_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    user_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    transaction_type_master_id = table.Column<int>(type: "int(11)", nullable: false),
                    transaction_category_master_id = table.Column<int>(type: "int(11)", nullable: false),
                    transaction_paymentmodeId = table.Column<int>(type: "int(11)", nullable: true),
                    transaction_description = table.Column<string>(type: "varchar(145)", maxLength: 145, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    transaction_amount = table.Column<double>(type: "double", nullable: false),
                    transaction_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    transaction_note = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    isActive = table.Column<sbyte>(type: "tinyint(4)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.transaction_master_id);
                    table.ForeignKey(
                        name: "transaction_category_master",
                        column: x => x.transaction_category_master_id,
                        principalTable: "transaction_category_master",
                        principalColumn: "transaction_category_master_id");
                    table.ForeignKey(
                        name: "transaction_paymernt_mode",
                        column: x => x.transaction_paymentmodeId,
                        principalTable: "transaction_payment_mode",
                        principalColumn: "paymentModeId");
                    table.ForeignKey(
                        name: "transaction_type_master",
                        column: x => x.transaction_type_master_id,
                        principalTable: "transaction_type_master",
                        principalColumn: "transaction_type_master_id");
                    table.ForeignKey(
                        name: "user_master",
                        column: x => x.user_id,
                        principalTable: "user_master",
                        principalColumn: "UserMasterID");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "user_master_id_idx",
                table: "transaction_category_master",
                column: "user_master_id");

            migrationBuilder.CreateIndex(
                name: "transaction_category_master_idx",
                table: "transaction_master",
                column: "transaction_category_master_id");

            migrationBuilder.CreateIndex(
                name: "transaction_paymernt_mode_idx",
                table: "transaction_master",
                column: "transaction_paymentmodeId");

            migrationBuilder.CreateIndex(
                name: "transaction_type_master_idx",
                table: "transaction_master",
                column: "transaction_type_master_id");

            migrationBuilder.CreateIndex(
                name: "user_master_idx",
                table: "transaction_master",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "usermasterID_FK_idx",
                table: "transaction_payment_mode",
                column: "userMasterId");

            migrationBuilder.CreateIndex(
                name: "user_master_id_idx1",
                table: "transaction_type_master",
                column: "user_master_id");

            migrationBuilder.CreateIndex(
                name: "RoleIdMaster_idx",
                table: "user_master",
                column: "UserRoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transaction_master");

            migrationBuilder.DropTable(
                name: "transaction_category_master");

            migrationBuilder.DropTable(
                name: "transaction_payment_mode");

            migrationBuilder.DropTable(
                name: "transaction_type_master");

            migrationBuilder.DropTable(
                name: "user_master");

            migrationBuilder.DropTable(
                name: "user_role_master");
        }
    }
}
