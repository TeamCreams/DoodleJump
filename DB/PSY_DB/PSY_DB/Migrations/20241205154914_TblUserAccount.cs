using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class TblUserAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CharacterStyle",
                table: "TblUserAccount",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Evolution",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Gold",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharacterStyle",
                table: "TblUserAccount");

            migrationBuilder.DropColumn(
                name: "Evolution",
                table: "TblUserAccount");

            migrationBuilder.DropColumn(
                name: "Gold",
                table: "TblUserAccount");
        }
    }
}
