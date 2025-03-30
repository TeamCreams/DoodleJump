using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class AddTblUserAccountPurchaseEnergy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Energy",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "10",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "10",
                oldComment: "마지막으로 에너지를 얻은 시간");

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPurchaseEnergyTime",
                table: "TblUserAccount",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "하루 중 에너지를 처음으로 구매한 시간");

            migrationBuilder.AddColumn<int>(
                name: "PurchaseEnergyCountToday",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                comment: "에너지를 구매한 횟수");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstPurchaseEnergyTime",
                table: "TblUserAccount");

            migrationBuilder.DropColumn(
                name: "PurchaseEnergyCountToday",
                table: "TblUserAccount");

            migrationBuilder.AlterColumn<int>(
                name: "Energy",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "10",
                comment: "마지막으로 에너지를 얻은 시간",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "10");
        }
    }
}
