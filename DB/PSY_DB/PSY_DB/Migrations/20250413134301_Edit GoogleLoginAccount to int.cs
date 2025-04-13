using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class EditGoogleLoginAccounttoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GoogleAccount",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "0000",
                comment: "구글 계정");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleAccount",
                table: "TblUserAccount");
        }
    }
}
