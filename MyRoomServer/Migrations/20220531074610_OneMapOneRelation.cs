using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyRoomServer.Migrations
{
    public partial class OneMapOneRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ag_house_HouseId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Widgets_Projects_ProjectId",
                table: "Widgets");

            migrationBuilder.DropIndex(
                name: "IX_Widgets_ProjectId",
                table: "Widgets");

            migrationBuilder.DropIndex(
                name: "IX_Projects_HouseId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "HouseId",
                table: "Projects");

            migrationBuilder.AddColumn<ulong>(
                name: "ProjectId1",
                table: "Widgets",
                type: "bigint(20) unsigned",
                nullable: true);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "Projects",
                type: "bigint(20) unsigned",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Widgets_ProjectId1",
                table: "Widgets",
                column: "ProjectId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ag_house_Id",
                table: "Projects",
                column: "Id",
                principalTable: "ag_house",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Widgets_Projects_ProjectId1",
                table: "Widgets",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ag_house_Id",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Widgets_Projects_ProjectId1",
                table: "Widgets");

            migrationBuilder.DropIndex(
                name: "IX_Widgets_ProjectId1",
                table: "Widgets");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "Widgets");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Projects",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "bigint(20) unsigned")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<ulong>(
                name: "HouseId",
                table: "Projects",
                type: "bigint(20) unsigned",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.CreateIndex(
                name: "IX_Widgets_ProjectId",
                table: "Widgets",
                column: "ProjectId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Widgets_Projects_ProjectId",
                table: "Widgets",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
