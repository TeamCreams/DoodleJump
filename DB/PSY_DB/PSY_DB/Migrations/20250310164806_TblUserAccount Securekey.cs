using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class TblUserAccountSecurekey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecureKey",
                table: "TblUserAccount",
                type: "longtext",
                nullable: false,
                comment: "보안 키")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecureKey",
                table: "TblUserAccount");
        }
    }
}
