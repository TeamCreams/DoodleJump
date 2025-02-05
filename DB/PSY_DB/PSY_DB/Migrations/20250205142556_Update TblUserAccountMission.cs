using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTblUserAccountMission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccumulatedStone",
                table: "TblUserScore",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Scoreboard",
                table: "TblUserScore",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccumulatedStone",
                table: "TblUserScore");

            migrationBuilder.DropColumn(
                name: "Scoreboard",
                table: "TblUserScore");
        }
    }
}
