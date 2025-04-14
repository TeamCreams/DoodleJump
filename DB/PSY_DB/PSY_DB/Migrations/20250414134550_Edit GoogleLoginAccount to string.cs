using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class EditGoogleLoginAccounttostring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GoogleAccount",
                table: "TblUserAccount",
                type: "longtext",
                nullable: true,
                defaultValueSql: "0000",
                comment: "구글 계정",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0000",
                oldComment: "구글 계정")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GoogleAccount",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "0000",
                comment: "구글 계정",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldDefaultValueSql: "0000",
                oldComment: "구글 계정")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
