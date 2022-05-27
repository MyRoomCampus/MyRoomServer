using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRoomServer.Entities;
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
    }
}
