using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class TblUserAccountAddEnergy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Energy",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "10",
                comment: "마지막으로 에너지를 얻은 시간");

            migrationBuilder.AddColumn<DateTime>(
                name: "LatelyEnergy",
                table: "TblUserAccount",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "now()",
                comment: "마지막으로 에너지를 얻은 시간");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Energy",
                table: "TblUserAccount");

            migrationBuilder.DropColumn(
                name: "LatelyEnergy",
                table: "TblUserAccount");
        }
    }
}
