using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyRoomServer.Migrations
{
    public partial class ForeignKeyProjectAgentHouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "HouseId",
                table: "Projects",
                type: "bigint(20) unsigned",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_HouseId",
                table: "Projects",
                column: "HouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ag_house_HouseId",
                table: "Projects",
                column: "HouseId",
                principalTable: "ag_house",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ag_house_HouseId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_HouseId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "HouseId",
                table: "Projects");
        }
    }
}
