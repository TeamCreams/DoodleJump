using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class TblUserAccountMissionaddRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TblUserAccount",
                keyColumn: "UserName",
                keyValue: null,
                column: "UserName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "TblUserAccount",
                type: "longtext",
                nullable: false,
                defaultValueSql: "NewAccount",
                comment: "계정 이름",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TblUserAccount",
                keyColumn: "Password",
                keyValue: null,
                column: "Password",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "TblUserAccount",
                type: "longtext",
                nullable: false,
                defaultValueSql: "0000",
                comment: "계정 비밀번호",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TblUserAccount",
                keyColumn: "Nickname",
                keyValue: null,
                column: "Nickname",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "TblUserAccount",
                type: "longtext",
                nullable: false,
                defaultValueSql: "Empty",
                comment: "계정 닉네임",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Evolution",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "140003",
                comment: "업데이트 스택",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0",
                oldComment: "업데이트 스택");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "TblUserAccount",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldDefaultValueSql: "NewAccount",
                oldComment: "계정 이름")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "TblUserAccount",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldDefaultValueSql: "0000",
                oldComment: "계정 비밀번호")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "TblUserAccount",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldDefaultValueSql: "Empty",
                oldComment: "계정 닉네임")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Evolution",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                comment: "업데이트 스택",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "140003",
                oldComment: "업데이트 스택");
        }
    }
}
