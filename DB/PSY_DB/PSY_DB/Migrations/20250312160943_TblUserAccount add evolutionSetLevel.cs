using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class TblUserAccountaddevolutionSetLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EvolutionSetLevel",
                table: "TblUserAccount",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "EvolutionSet Level");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EvolutionSetLevel",
                table: "TblUserAccount");
        }
    }
}
