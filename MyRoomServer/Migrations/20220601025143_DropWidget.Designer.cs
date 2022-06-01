﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyRoomServer.Entities.Contexts;

#nullable disable

namespace MyRoomServer.Migrations
{
    [DbContext(typeof(MyRoomDbContext))]
    [Migration("20220601025143_DropWidget")]
    partial class DropWidget
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");

            modelBuilder.Entity("MyRoomServer.Entities.AgentHouse", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint(20) unsigned")
                        .HasColumnName("id")
                        .HasComment("房源 id");

                    b.Property<uint?>("BuildingType")
                        .HasColumnType("int(10) unsigned")
                        .HasColumnName("building_type")
                        .HasComment("楼型，TODO");

                    b.Property<string>("BuiltYear")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("built_year")
                        .HasComment("建造年代");

                    b.Property<string>("CityCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("city_code");

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("city_name");

                    b.Property<ulong>("CrawlId")
                        .HasColumnType("bigint(20) unsigned")
                        .HasColumnName("crawl_id")
                        .HasComment("抓取id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .HasComment("创建时间");

                    b.Property<long>("DataSourceId")
                        .HasColumnType("bigint(20)")
                        .HasColumnName("data_source_id")
                        .HasComment("1：诸葛；2：安居客");

                    b.Property<uint?>("DecorationType")
                        .HasColumnType("int(10) unsigned")
                        .HasColumnName("decoration_type")
                        .HasComment("装修程度，1:简装 2:豪装");

                    b.Property<long?>("DictHouseId")
                        .HasColumnType("bigint(20)")
                        .HasColumnName("dict_house_id");

                    b.Property<ulong?>("Downpayment")
                        .HasColumnType("bigint(20) unsigned")
                        .HasColumnName("downpayment")
                        .HasComment("首付，单位分");

                    b.Property<int?>("Elevator")
                        .HasColumnType("int(11)")
                        .HasColumnName("elevator")
                        .HasComment("是否有电梯，0： 没有；1：有；null：未知");

                    b.Property<uint?>("FacingType")
                        .HasColumnType("int(10) unsigned")
                        .HasColumnName("facing_type")
                        .HasComment("朝向分类，1：南北");

                    b.Property<DateTime>("FirstUploadAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasColumnName("first_upload_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .HasComment("房源上架时间");

                    b.Property<uint?>("Floor")
                        .HasColumnType("int(10) unsigned")
                        .HasColumnName("floor")
                        .HasComment("楼层");

                    b.Property<uint?>("FloorLevel")
                        .HasColumnType("int(10) unsigned")
                        .HasColumnName("floor_level")
                        .HasComment("楼层位置：1: 高 2:中 3:低");

                    b.Property<uint?>("FloorPlanBath")
                        .HasColumnType("int(10) unsigned")
                        .HasColumnName("floor_plan_bath")
                        .HasComment("厕所个数");

                    b.Property<uint?>("FloorPlanHall")
                        .HasColumnType("int(10) unsigned")
                        .HasColumnName("floor_plan_hall")
                        .HasComment("厅个数");

                    b.Property<uint?>("FloorPlanKitchen")
                        .HasColumnType("int(10) unsigned")
                        .HasColumnName("floor_plan_kitchen")
                        .HasComment("厨房个数");

                    b.Property<uint?>("FloorPlanRoom")
                        .HasColumnType("int(10) unsigned")
                        .HasColumnName("floor_plan_room")
                        .HasComment("卧室个数");

                    b.Property<int?>("HeatingType")
                        .HasColumnType("int(11)")
                        .HasColumnName("heating_type");

                    b.Property<string>("HouseCard")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("house_card")
                        .HasDefaultValueSql("''");

                    b.Property<int?>("HouseDuration")
                        .HasColumnType("int(11)")
                        .HasColumnName("house_duration");

                    b.Property<int>("HouseStatus")
                        .HasColumnType("int(11)")
                        .HasColumnName("house_status")
                        .HasComment("房源状态 0：正常；1：下架");

                    b.Property<uint?>("HouseType")
                        .HasColumnType("int(10) unsigned")
                        .HasColumnName("house_type")
                        .HasComment("房源类型，1: 新房 2: 二手房 3:租房");

                    b.Property<int?>("LadderRation")
                        .HasColumnType("int(11)")
                        .HasColumnName("ladder_ration");

                    b.Property<DateTime>("LastPublishTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasColumnName("last_publish_time")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .HasComment("房源更新时间");

                    b.Property<int>("LastVersion")
                        .HasColumnType("int(11)")
                        .HasColumnName("last_version");

                    b.Property<int?>("LayoutType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("layout_type")
                        .HasDefaultValueSql("'0'")
                        .HasComment("户型类型");

                    b.Property<string>("ListingName")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("listing_name")
                        .HasComment("房源 title");

                    b.Property<int?>("Mortgage")
                        .HasColumnType("int(11)")
                        .HasColumnName("mortgage");

                    b.Property<string>("NeighborhoodName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("neighborhood_name");

                    b.Property<string>("NeighborhoodSourceCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("neighborhood_source_code")
                        .HasComment("小区 id，neighborhood 主键");

                    b.Property<string>("OfflineCode")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("offline_code")
                        .HasDefaultValueSql("'0'")
                        .HasComment("抓取房源 id");

                    b.Property<int>("OnlineAreaId")
                        .HasColumnType("int(11)")
                        .HasColumnName("online_area_id");

                    b.Property<int>("OnlineCityId")
                        .HasColumnType("int(11)")
                        .HasColumnName("online_city_id");

                    b.Property<int>("OnlineDistrictId")
                        .HasColumnType("int(11)")
                        .HasColumnName("online_district_id");

                    b.Property<int?>("OnlineHouseStatus")
                        .HasColumnType("int(11)")
                        .HasColumnName("online_house_status");

                    b.Property<long>("OnlineNeighborhoodId")
                        .HasColumnType("bigint(20)")
                        .HasColumnName("online_neighborhood_id");

                    b.Property<int?>("Ownership")
                        .HasColumnType("int(11)")
                        .HasColumnName("ownership")
                        .HasComment("交易权属: 1.商品房， 2. 公房，3.央产房，4.军产房，5.校产房， 6.私产，7. 经济适用房， 8.永久产权，9.空置房，10.使用权房，99.其他");

                    b.Property<ulong>("Pricing")
                        .HasColumnType("bigint(20) unsigned")
                        .HasColumnName("pricing")
                        .HasComment("报价，单位分");

                    b.Property<int?>("PropertyCertificatePeriod")
                        .HasColumnType("int(11)")
                        .HasColumnName("property_certificate_period")
                        .HasComment("房本年限: 0.不满二，1.满二，2.满五，99.其他");

                    b.Property<int?>("PropertyManagementType")
                        .HasColumnType("int(11)")
                        .HasColumnName("property_management_type")
                        .HasComment("房屋类型 ，房屋类型: 1.普通住宅，2.别墅，3.写字楼， 4.商铺，5.商住两用，6.公寓，7.工业厂房，8.车库，9.经济适用房，99. 其他");

                    b.Property<int?>("PropertyOnly")
                        .HasColumnType("int(11)")
                        .HasColumnName("property_only")
                        .HasComment("唯一住房:1.是，0.否");

                    b.Property<int?>("PropertyRight")
                        .HasColumnType("int(11)")
                        .HasColumnName("property_right")
                        .HasComment("共有，非共有");

                    b.Property<string>("RightProperty")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("right_property")
                        .HasDefaultValueSql("''")
                        .HasComment("产权年限，多种以/分隔，如70年/40年/50年");

                    b.Property<int?>("RoomStructure")
                        .HasColumnType("int(11)")
                        .HasColumnName("room_structure");

                    b.Property<string>("SourceCode")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)")
                        .HasColumnName("source_code")
                        .HasComment("离线房源编号");

                    b.Property<int>("Squaremeter")
                        .HasColumnType("int(11)")
                        .HasColumnName("squaremeter")
                        .HasComment("建筑面积，单位平方米（需要除以 100）");

                    b.Property<int>("StartVersion")
                        .HasColumnType("int(11)")
                        .HasColumnName("start_version");

                    b.Property<ulong?>("TaskId")
                        .HasColumnType("bigint(20) unsigned")
                        .HasColumnName("task_id")
                        .HasComment("taskid");

                    b.Property<int?>("TotalFloor")
                        .HasColumnType("int(11)")
                        .HasColumnName("total_floor")
                        .HasComment("总楼层数");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .HasComment("更新时间");

                    b.Property<int?>("UsageArea")
                        .HasColumnType("int(11)")
                        .HasColumnName("usage_area");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "CityName" }, "idx_city_name");

                    b.HasIndex(new[] { "CrawlId" }, "idx_crawl_id");

                    b.HasIndex(new[] { "CreatedAt" }, "idx_created_at");

                    b.HasIndex(new[] { "DataSourceId" }, "idx_data_source_id");

                    b.HasIndex(new[] { "DictHouseId" }, "idx_dict_house_id");

                    b.HasIndex(new[] { "HouseCard" }, "idx_house_card");

                    b.HasIndex(new[] { "NeighborhoodSourceCode", "LastVersion" }, "idx_neighborhood_code_lt_ver");

                    b.HasIndex(new[] { "OfflineCode", "DataSourceId" }, "idx_offline_code_data_source_id");

                    b.HasIndex(new[] { "OnlineCityId" }, "idx_online_city_id");

                    b.HasIndex(new[] { "OnlineHouseStatus" }, "idx_online_house_status");

                    b.HasIndex(new[] { "OnlineNeighborhoodId" }, "idx_online_neighborhood_id");

                    b.HasIndex(new[] { "UpdatedAt" }, "idx_updated_at");

                    b.HasIndex(new[] { "SourceCode", "StartVersion", "LastVersion" }, "uniq_source_code_st_ver_lt_ver")
                        .IsUnique();

                    b.ToTable("AgentHouses");
                });

            modelBuilder.Entity("MyRoomServer.Entities.Media", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Medias");
                });

            modelBuilder.Entity("MyRoomServer.Entities.Project", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Data")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("MyRoomServer.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("char(64)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyRoomServer.Entities.UserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("UsersClaims");
                });

            modelBuilder.Entity("MyRoomServer.Entities.UserOwn", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("HouseId")
                        .HasColumnType("bigint(20) unsigned");

                    b.Property<ulong?>("ProjectId")
                        .HasColumnType("bigint unsigned");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("HouseId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("UserOwns");
                });

            modelBuilder.Entity("MyRoomServer.Entities.UserOwn", b =>
                {
                    b.HasOne("MyRoomServer.Entities.AgentHouse", "House")
                        .WithMany()
                        .HasForeignKey("HouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyRoomServer.Entities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");

                    b.HasOne("MyRoomServer.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("House");

                    b.Navigation("Project");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}