using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRoomServer.Entities.Contexts;
using MyRoomServer.Extentions;
using MyRoomServer.Models;
using System.ComponentModel.DataAnnotations;

namespace MyRoomServer.Controllers
{
    [ApiController]
    [Route("house")]
    public class HouseController : ControllerBase
    {
        private readonly MyRoomDbContext dbContext;

        public HouseController(MyRoomDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// 获取一批房产信息
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="perpage">每页多少条数据</param>
        /// <param name="query">模糊查询（可选参数）</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery, Required] int page, [FromQuery, Required] int perpage, [FromQuery] string? query)
        {
            // TODO 因为匿名没法复用 如何优雅的服用此段代码 （太 ...长了 这部分
            var sqlQuery = from house in dbContext.AgentHouses
                           join own in dbContext.UserOwns
                           on house.Id equals own.HouseId
                           into res1
                           from own_house in res1
                           join project in dbContext.Projects
                           on own_house.ProjectId equals project.Id
                           into res2
                           from item in res2.DefaultIfEmpty()
                           select new
                           {
                               house.Id,
                               house.ListingName,
                               house.FirstUploadAt,
                               house.Pricing,
                               house.Squaremeter,
                               house.Downpayment,
                               house.Floor,
                               house.TotalFloor,
                               house.DictHouseId,
                               house.RoomStructure,
                               house.LadderRation,
                               house.HeatingType,
                               house.HouseDuration,
                               house.PropertyRight,
                               house.Mortgage,
                               house.UsageArea,
                               house.FloorLevel,
                               house.FacingType,
                               house.DecorationType,
                               house.BuildingType,
                               house.BuiltYear,
                               house.CityName,
                               house.CityCode,
                               house.NeighborhoodName,
                               house.NeighborhoodSourceCode,
                               house.FloorPlanRoom,
                               house.FloorPlanHall,
                               house.FloorPlanBath,
                               house.FloorPlanKitchen,
                               house.HouseType,
                               house.LayoutType,
                               house.LastPublishTime,
                               house.Ownership,
                               house.RightProperty,
                               house.PropertyManagementType,
                               house.Elevator,
                               house.HouseStatus,
                               house.OnlineHouseStatus,
                               house.CreatedAt,
                               house.UpdatedAt,
                               house.DataSourceId,
                               house.OfflineCode,
                               house.SourceCode,
                               house.StartVersion,
                               house.LastVersion,
                               house.CrawlId,
                               house.TaskId,
                               house.HouseCard,
                               house.OnlineNeighborhoodId,
                               house.OnlineCityId,
                               house.OnlineDistrictId,
                               house.OnlineAreaId,
                               house.PropertyOnly,
                               house.PropertyCertificatePeriod,
                               HaveProject = own_house.ProjectId != null,
                               HaveProjectPublished = item != null && item.IsPublished,
                           };

            if (query != null)
            {
                sqlQuery = sqlQuery.Where(x => x.ListingName.Contains(query)
                    || x.CityName.Contains(query)
                    || x.NeighborhoodName.Contains(query));
            }

            var res = await sqlQuery.Skip((page - 1) * perpage)
                                         .Take(perpage)
                                         .AsNoTracking()
                                         .ToListAsync();
            var cnt = await sqlQuery.CountAsync();

            return Ok(new
            {
                Count = cnt,
                Data = res,
            });
        }

        /// <summary>
        /// 根据Id获取房产信息
        /// </summary>
        /// <param name="id">房产Id</param>
        /// <returns></returns>
        /// <response code="200">获取成功</response>
        /// <response code="404">不存在此房产Id信息</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] ulong id)
        {
            var res = await (from house in dbContext.AgentHouses
                             join own in dbContext.UserOwns
                             on house.Id equals own.HouseId
                             into res1
                             from own_house in res1
                             join project in dbContext.Projects
                             on own_house.ProjectId equals project.Id
                             into res2
                             from item in res2.DefaultIfEmpty()
                             where house.Id == id
                             select new
                             {
                                 house.Id,
                                 house.ListingName,
                                 house.FirstUploadAt,
                                 house.Pricing,
                                 house.Squaremeter,
                                 house.Downpayment,
                                 house.Floor,
                                 house.TotalFloor,
                                 house.DictHouseId,
                                 house.RoomStructure,
                                 house.LadderRation,
                                 house.HeatingType,
                                 house.HouseDuration,
                                 house.PropertyRight,
                                 house.Mortgage,
                                 house.UsageArea,
                                 house.FloorLevel,
                                 house.FacingType,
                                 house.DecorationType,
                                 house.BuildingType,
                                 house.BuiltYear,
                                 house.CityName,
                                 house.CityCode,
                                 house.NeighborhoodName,
                                 house.NeighborhoodSourceCode,
                                 house.FloorPlanRoom,
                                 house.FloorPlanHall,
                                 house.FloorPlanBath,
                                 house.FloorPlanKitchen,
                                 house.HouseType,
                                 house.LayoutType,
                                 house.LastPublishTime,
                                 house.Ownership,
                                 house.RightProperty,
                                 house.PropertyManagementType,
                                 house.Elevator,
                                 house.HouseStatus,
                                 house.OnlineHouseStatus,
                                 house.CreatedAt,
                                 house.UpdatedAt,
                                 house.DataSourceId,
                                 house.OfflineCode,
                                 house.SourceCode,
                                 house.StartVersion,
                                 house.LastVersion,
                                 house.CrawlId,
                                 house.TaskId,
                                 house.HouseCard,
                                 house.OnlineNeighborhoodId,
                                 house.OnlineCityId,
                                 house.OnlineDistrictId,
                                 house.OnlineAreaId,
                                 house.PropertyOnly,
                                 house.PropertyCertificatePeriod,
                                 HaveProject = own_house.ProjectId != null,
                                 HaveProjectPublished = item != null && item.IsPublished,
                             }).AsNoTracking().SingleOrDefaultAsync();

            if (res == null)
            {
                return NotFound();
            }

            return Ok(new ApiRes("获取成功", res));
        }

        /// <summary>
        /// 查看用户发布的房产信息
        /// </summary>
        /// <returns></returns>
        /// <response code="200">用户所拥有的房产信息的Id（允许使用这些Id创建项目）</response>
        [HttpGet("own")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> GetByUserId()
        {
            var uid = this.GetUserId();
            var userOwnHouses = await (from own in dbContext.UserOwns
                                       where own.UserId == Guid.Parse(uid)
                                       join project in dbContext.Projects
                                       on own.ProjectId equals project.Id
                                       into res
                                       from item in res.DefaultIfEmpty()
                                       select new
                                       {
                                           own.HouseId,
                                           own.House.ListingName,
                                           HaveProject = own.ProjectId != null,
                                           HaveProjectPublished = item != null && item.IsPublished,
                                       }).AsNoTracking().ToListAsync();

            return Ok(new ApiRes("获取成功", userOwnHouses));
        }
    }
}
