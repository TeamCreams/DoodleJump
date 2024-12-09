using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class TblUserAccount2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharacterStyle",
                table: "TblUserAccount");

            migrationBuilder.AlterColumn<int>(
                name: "Gold",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Evolution",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                comment: "업데이트 스택",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EyebrowStyle",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "0");

            migrationBuilder.AddColumn<int>(
                name: "EyesStyle",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "0");

            migrationBuilder.AddColumn<int>(
                name: "HairStyle",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                comment: "디자인");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EyebrowStyle",
                table: "TblUserAccount");

            migrationBuilder.DropColumn(
                name: "EyesStyle",
                table: "TblUserAccount");

            migrationBuilder.DropColumn(
                name: "HairStyle",
                table: "TblUserAccount");

            migrationBuilder.AlterColumn<int>(
                name: "Gold",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "Evolution",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0",
                oldComment: "업데이트 스택");

            migrationBuilder.AddColumn<string>(
                name: "CharacterStyle",
                table: "TblUserAccount",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
