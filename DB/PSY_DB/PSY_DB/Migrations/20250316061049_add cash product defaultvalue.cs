using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class addcashproductdefaultvalue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "TblUserCashProduct",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "UTC_TIMESTAMP()",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDate",
                table: "TblUserCashProduct",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "UTC_TIMESTAMP()",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDate",
                table: "TblCashProduct",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "UTC_TIMESTAMP()",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "TblUserCashProduct",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "UTC_TIMESTAMP()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDate",
                table: "TblUserCashProduct",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "UTC_TIMESTAMP()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDate",
                table: "TblCashProduct",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "UTC_TIMESTAMP()");
        }
    }
}
