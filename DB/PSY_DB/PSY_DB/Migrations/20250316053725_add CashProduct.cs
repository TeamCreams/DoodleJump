using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class addCashProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblCashProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<string>(type: "longtext", nullable: false, comment: "Store Product Id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<float>(type: "float", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false, comment: "0 : KRW, 1 : USD"),
                    ItemType = table.Column<int>(type: "int", nullable: false, comment: "0 : Cash, 1 : Subscription"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    IsConsumable = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "구독 중인지"),
                    RegisterDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblCashProduct", x => x.Id);
                },
                comment: "Cash Product 정보 Update 금지.")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TblUserCashProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "TblCashProduct PK"),
                    UserAccountId = table.Column<int>(type: "int", nullable: false, comment: "User Account Id"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUserCashProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblUserCashProduct_TblCashProduct_ProductId",
                        column: x => x.ProductId,
                        principalTable: "TblCashProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblUserCashProduct_TblUserAccount_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "TblUserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "User Cash Product 정보")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TblUserCashProduct_ProductId",
                table: "TblUserCashProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TblUserCashProduct_UserAccountId",
                table: "TblUserCashProduct",
                column: "UserAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblUserCashProduct");

            migrationBuilder.DropTable(
                name: "TblCashProduct");
        }
    }
}
