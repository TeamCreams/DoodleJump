using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class TblUserAccountEditnowtoUTC_TIMESTAMP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LatelyEnergy",
                table: "TblUserAccount",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "UTC_TIMESTAMP()",
                comment: "마지막으로 에너지를 얻은 시간",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "now()",
                oldComment: "마지막으로 에너지를 얻은 시간");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LatelyEnergy",
                table: "TblUserAccount",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "now()",
                comment: "마지막으로 에너지를 얻은 시간",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "UTC_TIMESTAMP()",
                oldComment: "마지막으로 에너지를 얻은 시간");
        }
    }
}
