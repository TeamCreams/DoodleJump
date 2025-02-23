using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class CreateTblUserMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblUserMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserAccountId = table.Column<int>(type: "int", nullable: false, comment: "TblUserAccount FK"),
                    Message = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageSentTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReceiverUserId = table.Column<int>(type: "int", nullable: false, comment: "귓속말 시 사용할 메세지 수신 유저 아이디")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUserMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblUserMessage_TblUserAccount_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "TblUserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Message 정보")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TblUserMessage_UserAccountId",
                table: "TblUserMessage",
                column: "UserAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblUserMessage");
        }
    }
}
