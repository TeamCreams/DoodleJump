using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class DeleteTblUserAccountSecureKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "TblUserScore");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "TblUserScore");

            migrationBuilder.DropColumn(
                name: "SecureKey",
                table: "TblUserAccount");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterDate",
                table: "TblUserMission",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "TblUserMission",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisterDate",
                table: "TblUserMission");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "TblUserMission");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "TblUserScore",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "TblUserScore",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SecureKey",
                table: "TblUserAccount",
                type: "longtext",
                nullable: false,
                comment: "보안 키")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
