using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyRoomServer.Migrations
{
    public partial class UpdateWidget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abscissa",
                table: "Widgets");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Widgets");

            migrationBuilder.DropColumn(
                name: "Ordinate",
                table: "Widgets");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Widgets");

            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "Widgets",
                type: "varchar(256)",
                maxLength: 256,
                nullable: false,
                collation: "utf8mb4_general_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.AddColumn<ulong>(
                name: "CurrentId",
                table: "Widgets",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Widgets",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Style",
                table: "Widgets",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Widgets",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Widgets_ProjectId",
                table: "Widgets",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Widgets_Projects_ProjectId",
                table: "Widgets",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Widgets_Projects_ProjectId",
                table: "Widgets");

            migrationBuilder.DropIndex(
                name: "IX_Widgets_ProjectId",
                table: "Widgets");

            migrationBuilder.DropColumn(
                name: "CurrentId",
                table: "Widgets");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Widgets");

            migrationBuilder.DropColumn(
                name: "Style",
                table: "Widgets");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Widgets");

            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "Widgets",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.AddColumn<long>(
                name: "Abscissa",
                table: "Widgets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Length",
                table: "Widgets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Ordinate",
                table: "Widgets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Width",
                table: "Widgets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
