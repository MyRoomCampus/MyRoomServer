using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRoomServer.Entities.Contexts;
using MyRoomServer.Extentions;
using MyRoomServer.Models;

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
        public async Task<IActionResult> Get([FromQuery] int page, [FromQuery] int perpage, [FromQuery] string? query)
        {
            var sqlQuery = (from item in dbContext.AgentHouses
                            select item);

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
            var res = await dbContext.AgentHouses.FindAsync(id);

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
            var userOwnHouses = await dbContext.UserOwns
                .Where(x => x.UserId == Guid.Parse(uid))
                .Select(x => x.HouseId)
                .ToListAsync();

            return Ok(new ApiRes("获取成功", userOwnHouses));
        }
    }
}
