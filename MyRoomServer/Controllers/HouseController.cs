using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRoomServer.Entities.Contexts;
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
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] int page, [FromQuery] int perpage)
        {
            var res = await (from item in dbContext.AgHouses
                             select item).Skip((page - 1) * perpage)
                                         .Take(perpage)
                                         .AsNoTracking()
                                         .ToListAsync();

            return Ok(new ApiRes("获取成功", res));
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
            var res = await dbContext.AgHouses.FindAsync(id);

            if(res == null)
            {
                return NotFound();
            }

            return Ok(new ApiRes("获取成功", res));
        }
    }
}
