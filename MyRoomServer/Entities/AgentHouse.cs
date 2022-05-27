using System;
using System.Collections.Generic;

namespace MyRoomServer.Entities
{
    public class AgentHouse
    {
        /// <summary>
        /// 房源 id
        /// </summary>
        public ulong Id { get; set; }
        /// <summary>
        /// 房源 title
        /// </summary>
        public string ListingName { get; set; }
        /// <summary>
        /// 房源上架时间
        /// </summary>
        public DateTime FirstUploadAt { get; set; }
        /// <summary>
        /// 报价，单位分
        /// </summary>
        public ulong Pricing { get; set; }
        /// <summary>
        /// 建筑面积，单位平方米（需要除以 100）
        /// </summary>
        public int Squaremeter { get; set; }
        /// <summary>
        /// 首付，单位分
        /// </summary>
        public ulong? Downpayment { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public uint? Floor { get; set; }
        /// <summary>
        /// 总楼层数
        /// </summary>
        public int? TotalFloor { get; set; }
        public long? DictHouseId { get; set; }
        public int? RoomStructure { get; set; }
        public int? LadderRation { get; set; }
        public int? HeatingType { get; set; }
        public int? HouseDuration { get; set; }
        /// <summary>
        /// 共有，非共有
        /// </summary>
        public int? PropertyRight { get; set; }
        public int? Mortgage { get; set; }
        public int? UsageArea { get; set; }
        /// <summary>
        /// 楼层位置：1: 高 2:中 3:低
        /// </summary>
        public uint? FloorLevel { get; set; }
        /// <summary>
        /// 朝向分类，1：南北
        /// </summary>
        public uint? FacingType { get; set; }
        /// <summary>
        /// 装修程度，1:简装 2:豪装
        /// </summary>
        public uint? DecorationType { get; set; }
        /// <summary>
        /// 楼型，TODO
        /// </summary>
        public uint? BuildingType { get; set; }
        /// <summary>
        /// 建造年代
        /// </summary>
        public string BuiltYear { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public string NeighborhoodName { get; set; }
        /// <summary>
        /// 小区 id，neighborhood 主键
        /// </summary>
        public string NeighborhoodSourceCode { get; set; }
        /// <summary>
        /// 卧室个数
        /// </summary>
        public uint? FloorPlanRoom { get; set; }
        /// <summary>
        /// 厅个数
        /// </summary>
        public uint? FloorPlanHall { get; set; }
        /// <summary>
        /// 厕所个数
        /// </summary>
        public uint? FloorPlanBath { get; set; }
        /// <summary>
        /// 厨房个数
        /// </summary>
        public uint? FloorPlanKitchen { get; set; }
        /// <summary>
        /// 房源类型，1: 新房 2: 二手房 3:租房
        /// </summary>
        public uint? HouseType { get; set; }
        /// <summary>
        /// 户型类型
        /// </summary>
        public int? LayoutType { get; set; }
        /// <summary>
        /// 房源更新时间
        /// </summary>
        public DateTime LastPublishTime { get; set; }
        /// <summary>
        /// 交易权属: 1.商品房， 2. 公房，3.央产房，4.军产房，5.校产房， 6.私产，7. 经济适用房， 8.永久产权，9.空置房，10.使用权房，99.其他
        /// </summary>
        public int? Ownership { get; set; }
        /// <summary>
        /// 产权年限，多种以/分隔，如70年/40年/50年
        /// </summary>
        public string RightProperty { get; set; }
        /// <summary>
        /// 房屋类型 ，房屋类型: 1.普通住宅，2.别墅，3.写字楼， 4.商铺，5.商住两用，6.公寓，7.工业厂房，8.车库，9.经济适用房，99. 其他
        /// </summary>
        public int? PropertyManagementType { get; set; }
        /// <summary>
        /// 是否有电梯，0： 没有；1：有；null：未知
        /// </summary>
        public int? Elevator { get; set; }
        /// <summary>
        /// 房源状态 0：正常；1：下架
        /// </summary>
        public int HouseStatus { get; set; }
        public int? OnlineHouseStatus { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// 1：诸葛；2：安居客
        /// </summary>
        public long DataSourceId { get; set; }
        /// <summary>
        /// 抓取房源 id
        /// </summary>
        public string OfflineCode { get; set; }
        /// <summary>
        /// 离线房源编号
        /// </summary>
        public string SourceCode { get; set; }
        public int StartVersion { get; set; }
        public int LastVersion { get; set; }
        /// <summary>
        /// 抓取id
        /// </summary>
        public ulong CrawlId { get; set; }
        /// <summary>
        /// taskid
        /// </summary>
        public ulong? TaskId { get; set; }
        public string HouseCard { get; set; }
        public long OnlineNeighborhoodId { get; set; }
        public int OnlineCityId { get; set; }
        public int OnlineDistrictId { get; set; }
        public int OnlineAreaId { get; set; }
        /// <summary>
        /// 唯一住房:1.是，0.否
        /// </summary>
        public int? PropertyOnly { get; set; }
        /// <summary>
        /// 房本年限: 0.不满二，1.满二，2.满五，99.其他
        /// </summary>
        public int? PropertyCertificatePeriod { get; set; }
    }
}
