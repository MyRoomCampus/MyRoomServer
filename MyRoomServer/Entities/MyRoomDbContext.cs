using Microsoft.EntityFrameworkCore;

namespace MyRoomServer.Entities
{
    public class MyRoomDbContext : DbContext
    {
        public MyRoomDbContext(DbContextOptions<MyRoomDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserClaim> UsersClaims { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Widget> Widgets { get; set; } = null!;
        public DbSet<AgentHouse> AgHouses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<AgentHouse>(entity =>
            {
                entity.ToTable("ag_house");

                entity.HasIndex(e => e.CityName, "idx_city_name");

                entity.HasIndex(e => e.CrawlId, "idx_crawl_id");

                entity.HasIndex(e => e.CreatedAt, "idx_created_at");

                entity.HasIndex(e => e.DataSourceId, "idx_data_source_id");

                entity.HasIndex(e => e.DictHouseId, "idx_dict_house_id");

                entity.HasIndex(e => e.HouseCard, "idx_house_card");

                entity.HasIndex(e => new { e.NeighborhoodSourceCode, e.LastVersion }, "idx_neighborhood_code_lt_ver");

                entity.HasIndex(e => new { e.OfflineCode, e.DataSourceId }, "idx_offline_code_data_source_id");

                entity.HasIndex(e => e.OnlineCityId, "idx_online_city_id");

                entity.HasIndex(e => e.OnlineHouseStatus, "idx_online_house_status");

                entity.HasIndex(e => e.OnlineNeighborhoodId, "idx_online_neighborhood_id");

                entity.HasIndex(e => e.UpdatedAt, "idx_updated_at");

                entity.HasIndex(e => new { e.SourceCode, e.StartVersion, e.LastVersion }, "uniq_source_code_st_ver_lt_ver")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("id")
                    .HasComment("房源 id");

                entity.Property(e => e.BuildingType)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("building_type")
                    .HasComment("楼型，TODO");

                entity.Property(e => e.BuiltYear)
                    .HasMaxLength(64)
                    .HasColumnName("built_year")
                    .HasComment("建造年代");

                entity.Property(e => e.CityCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("city_code");

                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("city_name");

                entity.Property(e => e.CrawlId)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("crawl_id")
                    .HasComment("抓取id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("创建时间");

                entity.Property(e => e.DataSourceId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("data_source_id")
                    .HasComment("1：诸葛；2：安居客");

                entity.Property(e => e.DecorationType)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("decoration_type")
                    .HasComment("装修程度，1:简装 2:豪装");

                entity.Property(e => e.DictHouseId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("dict_house_id");

                entity.Property(e => e.Downpayment)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("downpayment")
                    .HasComment("首付，单位分");

                entity.Property(e => e.Elevator)
                    .HasColumnType("int(11)")
                    .HasColumnName("elevator")
                    .HasComment("是否有电梯，0： 没有；1：有；null：未知");

                entity.Property(e => e.FacingType)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("facing_type")
                    .HasComment("朝向分类，1：南北");

                entity.Property(e => e.FirstUploadAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("first_upload_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("房源上架时间");

                entity.Property(e => e.Floor)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("floor")
                    .HasComment("楼层");

                entity.Property(e => e.FloorLevel)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("floor_level")
                    .HasComment("楼层位置：1: 高 2:中 3:低");

                entity.Property(e => e.FloorPlanBath)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("floor_plan_bath")
                    .HasComment("厕所个数");

                entity.Property(e => e.FloorPlanHall)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("floor_plan_hall")
                    .HasComment("厅个数");

                entity.Property(e => e.FloorPlanKitchen)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("floor_plan_kitchen")
                    .HasComment("厨房个数");

                entity.Property(e => e.FloorPlanRoom)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("floor_plan_room")
                    .HasComment("卧室个数");

                entity.Property(e => e.HeatingType)
                    .HasColumnType("int(11)")
                    .HasColumnName("heating_type");

                entity.Property(e => e.HouseCard)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("house_card")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.HouseDuration)
                    .HasColumnType("int(11)")
                    .HasColumnName("house_duration");

                entity.Property(e => e.HouseStatus)
                    .HasColumnType("int(11)")
                    .HasColumnName("house_status")
                    .HasComment("房源状态 0：正常；1：下架");

                entity.Property(e => e.HouseType)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("house_type")
                    .HasComment("房源类型，1: 新房 2: 二手房 3:租房");

                entity.Property(e => e.LadderRation)
                    .HasColumnType("int(11)")
                    .HasColumnName("ladder_ration");

                entity.Property(e => e.LastPublishTime)
                    .HasColumnType("timestamp")
                    .HasColumnName("last_publish_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("房源更新时间");

                entity.Property(e => e.LastVersion)
                    .HasColumnType("int(11)")
                    .HasColumnName("last_version");

                entity.Property(e => e.LayoutType)
                    .HasColumnType("int(11)")
                    .HasColumnName("layout_type")
                    .HasDefaultValueSql("'0'")
                    .HasComment("户型类型");

                entity.Property(e => e.ListingName)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .HasColumnName("listing_name")
                    .HasComment("房源 title");

                entity.Property(e => e.Mortgage)
                    .HasColumnType("int(11)")
                    .HasColumnName("mortgage");

                entity.Property(e => e.NeighborhoodName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("neighborhood_name");

                entity.Property(e => e.NeighborhoodSourceCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("neighborhood_source_code")
                    .HasComment("小区 id，neighborhood 主键");

                entity.Property(e => e.OfflineCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("offline_code")
                    .HasDefaultValueSql("'0'")
                    .HasComment("抓取房源 id");

                entity.Property(e => e.OnlineAreaId)
                    .HasColumnType("int(11)")
                    .HasColumnName("online_area_id");

                entity.Property(e => e.OnlineCityId)
                    .HasColumnType("int(11)")
                    .HasColumnName("online_city_id");

                entity.Property(e => e.OnlineDistrictId)
                    .HasColumnType("int(11)")
                    .HasColumnName("online_district_id");

                entity.Property(e => e.OnlineHouseStatus)
                    .HasColumnType("int(11)")
                    .HasColumnName("online_house_status");

                entity.Property(e => e.OnlineNeighborhoodId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("online_neighborhood_id");

                entity.Property(e => e.Ownership)
                    .HasColumnType("int(11)")
                    .HasColumnName("ownership")
                    .HasComment("交易权属: 1.商品房， 2. 公房，3.央产房，4.军产房，5.校产房， 6.私产，7. 经济适用房， 8.永久产权，9.空置房，10.使用权房，99.其他");

                entity.Property(e => e.Pricing)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("pricing")
                    .HasComment("报价，单位分");

                entity.Property(e => e.PropertyCertificatePeriod)
                    .HasColumnType("int(11)")
                    .HasColumnName("property_certificate_period")
                    .HasComment("房本年限: 0.不满二，1.满二，2.满五，99.其他");

                entity.Property(e => e.PropertyManagementType)
                    .HasColumnType("int(11)")
                    .HasColumnName("property_management_type")
                    .HasComment("房屋类型 ，房屋类型: 1.普通住宅，2.别墅，3.写字楼， 4.商铺，5.商住两用，6.公寓，7.工业厂房，8.车库，9.经济适用房，99. 其他");

                entity.Property(e => e.PropertyOnly)
                    .HasColumnType("int(11)")
                    .HasColumnName("property_only")
                    .HasComment("唯一住房:1.是，0.否");

                entity.Property(e => e.PropertyRight)
                    .HasColumnType("int(11)")
                    .HasColumnName("property_right")
                    .HasComment("共有，非共有");

                entity.Property(e => e.RightProperty)
                    .HasMaxLength(255)
                    .HasColumnName("right_property")
                    .HasDefaultValueSql("''")
                    .HasComment("产权年限，多种以/分隔，如70年/40年/50年");

                entity.Property(e => e.RoomStructure)
                    .HasColumnType("int(11)")
                    .HasColumnName("room_structure");

                entity.Property(e => e.SourceCode)
                    .HasMaxLength(128)
                    .HasColumnName("source_code")
                    .HasComment("离线房源编号");

                entity.Property(e => e.Squaremeter)
                    .HasColumnType("int(11)")
                    .HasColumnName("squaremeter")
                    .HasComment("建筑面积，单位平方米（需要除以 100）");

                entity.Property(e => e.StartVersion)
                    .HasColumnType("int(11)")
                    .HasColumnName("start_version");

                entity.Property(e => e.TaskId)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("task_id")
                    .HasComment("taskid");

                entity.Property(e => e.TotalFloor)
                    .HasColumnType("int(11)")
                    .HasColumnName("total_floor")
                    .HasComment("总楼层数");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("更新时间");

                entity.Property(e => e.UsageArea)
                    .HasColumnType("int(11)")
                    .HasColumnName("usage_area");
            });
        }
    }
}
