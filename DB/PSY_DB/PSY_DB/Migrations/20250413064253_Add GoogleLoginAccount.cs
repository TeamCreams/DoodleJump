using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class AddGoogleLoginAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "TblUserAccount",
                type: "longtext",
                nullable: true,
                defaultValueSql: "NewAccount",
                comment: "계정 이름",
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
                defaultValueSql: "0000",
                comment: "계정 비밀번호",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldDefaultValueSql: "0000",
                oldComment: "계정 비밀번호")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "GoogleAccount",
                table: "TblUserAccount",
                type: "longtext",
                nullable: true,
                defaultValueSql: "GoogleAccount",
                comment: "구글 계정")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleAccount",
                table: "TblUserAccount");

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
                oldNullable: true,
                oldDefaultValueSql: "NewAccount",
                oldComment: "계정 이름")
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
                oldNullable: true,
                oldDefaultValueSql: "0000",
                oldComment: "계정 비밀번호")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
