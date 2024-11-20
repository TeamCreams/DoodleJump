using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSY_DB.Migrations
{
    /// <inheritdoc />
    public partial class addtabletblusermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblUserMission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserAccountId = table.Column<int>(type: "int", nullable: false, comment: "TblUserAccount FK"),
                    MissionId = table.Column<int>(type: "int", nullable: false),
                    MissionStatus = table.Column<int>(type: "int", nullable: false),
                    Param1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUserMission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblUserMission_TblUserAccount_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "TblUserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "UserScore 정보")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TblUserMission_UserAccountId",
                table: "TblUserMission",
                column: "UserAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblUserMission");
        }
    }
}
