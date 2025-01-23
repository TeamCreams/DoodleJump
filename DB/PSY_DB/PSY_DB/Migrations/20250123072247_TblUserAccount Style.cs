using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class TblUserAccountStyle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HairStyle",
                table: "TblUserAccount",
                type: "longtext",
                nullable: true,
                defaultValueSql: "Afro",
                comment: "디자인",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0",
                oldComment: "디자인")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "EyesStyle",
                table: "TblUserAccount",
                type: "longtext",
                nullable: true,
                defaultValueSql: "Annoyed",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "EyebrowStyle",
                table: "TblUserAccount",
                type: "longtext",
                nullable: true,
                defaultValueSql: "AnnoyedEyebrows",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "0")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "HairStyle",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                comment: "디자인",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldDefaultValueSql: "Afro",
                oldComment: "디자인")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "EyesStyle",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldDefaultValueSql: "Annoyed")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "EyebrowStyle",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldDefaultValueSql: "AnnoyedEyebrows")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
