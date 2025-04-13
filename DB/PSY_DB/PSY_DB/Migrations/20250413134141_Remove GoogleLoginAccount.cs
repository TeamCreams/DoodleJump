using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGoogleLoginAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleAccount",
                table: "TblUserAccount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleAccount",
                table: "TblUserAccount",
                type: "longtext",
                nullable: true,
                defaultValueSql: "GoogleAccount",
                comment: "구글 계정")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
