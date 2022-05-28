using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyRoomServer.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ag_house",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false, comment: "房源 id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    listing_name = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: false, comment: "房源 title", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    first_upload_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "房源上架时间"),
                    pricing = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false, comment: "报价，单位分"),
                    squaremeter = table.Column<int>(type: "int(11)", nullable: false, comment: "建筑面积，单位平方米（需要除以 100）"),
                    downpayment = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, comment: "首付，单位分"),
                    floor = table.Column<uint>(type: "int(10) unsigned", nullable: true, comment: "楼层"),
                    total_floor = table.Column<int>(type: "int(11)", nullable: true, comment: "总楼层数"),
                    dict_house_id = table.Column<long>(type: "bigint(20)", nullable: true),
                    room_structure = table.Column<int>(type: "int(11)", nullable: true),
                    ladder_ration = table.Column<int>(type: "int(11)", nullable: true),
                    heating_type = table.Column<int>(type: "int(11)", nullable: true),
                    house_duration = table.Column<int>(type: "int(11)", nullable: true),
                    property_right = table.Column<int>(type: "int(11)", nullable: true, comment: "共有，非共有"),
                    mortgage = table.Column<int>(type: "int(11)", nullable: true),
                    usage_area = table.Column<int>(type: "int(11)", nullable: true),
                    floor_level = table.Column<uint>(type: "int(10) unsigned", nullable: true, comment: "楼层位置：1: 高 2:中 3:低"),
                    facing_type = table.Column<uint>(type: "int(10) unsigned", nullable: true, comment: "朝向分类，1：南北"),
                    decoration_type = table.Column<uint>(type: "int(10) unsigned", nullable: true, comment: "装修程度，1:简装 2:豪装"),
                    building_type = table.Column<uint>(type: "int(10) unsigned", nullable: true, comment: "楼型，TODO"),
                    built_year = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "建造年代", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    city_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    city_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    neighborhood_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    neighborhood_source_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "小区 id，neighborhood 主键", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    floor_plan_room = table.Column<uint>(type: "int(10) unsigned", nullable: true, comment: "卧室个数"),
                    floor_plan_hall = table.Column<uint>(type: "int(10) unsigned", nullable: true, comment: "厅个数"),
                    floor_plan_bath = table.Column<uint>(type: "int(10) unsigned", nullable: true, comment: "厕所个数"),
                    floor_plan_kitchen = table.Column<uint>(type: "int(10) unsigned", nullable: true, comment: "厨房个数"),
                    house_type = table.Column<uint>(type: "int(10) unsigned", nullable: true, comment: "房源类型，1: 新房 2: 二手房 3:租房"),
                    layout_type = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'", comment: "户型类型"),
                    last_publish_time = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "房源更新时间"),
                    ownership = table.Column<int>(type: "int(11)", nullable: true, comment: "交易权属: 1.商品房， 2. 公房，3.央产房，4.军产房，5.校产房， 6.私产，7. 经济适用房， 8.永久产权，9.空置房，10.使用权房，99.其他"),
                    right_property = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValueSql: "''", comment: "产权年限，多种以/分隔，如70年/40年/50年", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    property_management_type = table.Column<int>(type: "int(11)", nullable: true, comment: "房屋类型 ，房屋类型: 1.普通住宅，2.别墅，3.写字楼， 4.商铺，5.商住两用，6.公寓，7.工业厂房，8.车库，9.经济适用房，99. 其他"),
                    elevator = table.Column<int>(type: "int(11)", nullable: true, comment: "是否有电梯，0： 没有；1：有；null：未知"),
                    house_status = table.Column<int>(type: "int(11)", nullable: false, comment: "房源状态 0：正常；1：下架"),
                    online_house_status = table.Column<int>(type: "int(11)", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "创建时间"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新时间"),
                    data_source_id = table.Column<long>(type: "bigint(20)", nullable: false, comment: "1：诸葛；2：安居客"),
                    offline_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValueSql: "'0'", comment: "抓取房源 id", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    source_code = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, comment: "离线房源编号", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_version = table.Column<int>(type: "int(11)", nullable: false),
                    last_version = table.Column<int>(type: "int(11)", nullable: false),
                    crawl_id = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false, comment: "抓取id"),
                    task_id = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, comment: "taskid"),
                    house_card = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValueSql: "''", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    online_neighborhood_id = table.Column<long>(type: "bigint(20)", nullable: false),
                    online_city_id = table.Column<int>(type: "int(11)", nullable: false),
                    online_district_id = table.Column<int>(type: "int(11)", nullable: false),
                    online_area_id = table.Column<int>(type: "int(11)", nullable: false),
                    property_only = table.Column<int>(type: "int(11)", nullable: true, comment: "唯一住房:1.是，0.否"),
                    property_certificate_period = table.Column<int>(type: "int(11)", nullable: true, comment: "房本年限: 0.不满二，1.满二，2.满五，99.其他")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ag_house", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "char(64)", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "UsersClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersClaims", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Widgets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    Abscissa = table.Column<long>(type: "bigint", nullable: false),
                    Ordinate = table.Column<long>(type: "bigint", nullable: false),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    Width = table.Column<long>(type: "bigint", nullable: false),
                    Data = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Widgets", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "idx_city_name",
                table: "ag_house",
                column: "city_name");

            migrationBuilder.CreateIndex(
                name: "idx_crawl_id",
                table: "ag_house",
                column: "crawl_id");

            migrationBuilder.CreateIndex(
                name: "idx_created_at",
                table: "ag_house",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "idx_data_source_id",
                table: "ag_house",
                column: "data_source_id");

            migrationBuilder.CreateIndex(
                name: "idx_dict_house_id",
                table: "ag_house",
                column: "dict_house_id");

            migrationBuilder.CreateIndex(
                name: "idx_house_card",
                table: "ag_house",
                column: "house_card");

            migrationBuilder.CreateIndex(
                name: "idx_neighborhood_code_lt_ver",
                table: "ag_house",
                columns: new[] { "neighborhood_source_code", "last_version" });

            migrationBuilder.CreateIndex(
                name: "idx_offline_code_data_source_id",
                table: "ag_house",
                columns: new[] { "offline_code", "data_source_id" });

            migrationBuilder.CreateIndex(
                name: "idx_online_city_id",
                table: "ag_house",
                column: "online_city_id");

            migrationBuilder.CreateIndex(
                name: "idx_online_house_status",
                table: "ag_house",
                column: "online_house_status");

            migrationBuilder.CreateIndex(
                name: "idx_online_neighborhood_id",
                table: "ag_house",
                column: "online_neighborhood_id");

            migrationBuilder.CreateIndex(
                name: "idx_updated_at",
                table: "ag_house",
                column: "updated_at");

            migrationBuilder.CreateIndex(
                name: "uniq_source_code_st_ver_lt_ver",
                table: "ag_house",
                columns: new[] { "source_code", "start_version", "last_version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ag_house");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UsersClaims");

            migrationBuilder.DropTable(
                name: "Widgets");
        }
    }
}
