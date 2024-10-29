using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class SetFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "TblUserScore",
                newName: "UserAccountId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "TblUserAccount",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterDate",
                table: "TblUserAccount",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "TblUserAccount",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_TblUserScore_UserAccountId",
                table: "TblUserScore",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblUserScore_TblUserAccount_UserAccountId",
                table: "TblUserScore",
                column: "UserAccountId",
                principalTable: "TblUserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblUserScore_TblUserAccount_UserAccountId",
                table: "TblUserScore");

            migrationBuilder.DropIndex(
                name: "IX_TblUserScore_UserAccountId",
                table: "TblUserScore");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "TblUserAccount");

            migrationBuilder.DropColumn(
                name: "RegisterDate",
                table: "TblUserAccount");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "TblUserAccount");

            migrationBuilder.RenameColumn(
                name: "UserAccountId",
                table: "TblUserScore",
                newName: "PlayerId");
        }
    }
}
