using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyRoomServer.Migrations
{
    public partial class RenameModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseMapUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "UserOwns",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HouseId = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProjectId = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOwns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOwns_ag_house_HouseId",
                        column: x => x.HouseId,
                        principalTable: "ag_house",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOwns_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserOwns_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_UserOwns_HouseId",
                table: "UserOwns",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOwns_ProjectId",
                table: "UserOwns",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOwns_UserId",
                table: "UserOwns",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOwns");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Projects",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "HouseMapUsers",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HouseId = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseMapUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseMapUsers_ag_house_HouseId",
                        column: x => x.HouseId,
                        principalTable: "ag_house",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseMapUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_HouseMapUsers_HouseId",
                table: "HouseMapUsers",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseMapUsers_UserId",
                table: "HouseMapUsers",
                column: "UserId");
        }
    }
}
