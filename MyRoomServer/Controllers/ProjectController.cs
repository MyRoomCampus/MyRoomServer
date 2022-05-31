using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRoomServer.Entities;
using MyRoomServer.Models;
using MyRoomServer.Extentions;
using Microsoft.EntityFrameworkCore;
using MyRoomServer.Entities.Contexts;

namespace MyRoomServer.Controllers
{
    [Route("project")]
    [ApiController]
    public partial class ProjectController : ControllerBase
    {
        private readonly MyRoomDbContext dbContext;

        public ProjectController(MyRoomDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// 获取该用户的所有项目信息
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="perpage">每页的数据数</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> Get([FromQuery] int page, [FromQuery] int perpage)
        {
            var uid = this.GetUserId();
            // TODO 验证在判断符号两边进行运算的影响
            var query = (from owns in dbContext.UserOwns
                         join project in dbContext.Projects
                         on owns.ProjectId equals project.Id
                         where owns.UserId == Guid.Parse(uid)
                         select new
                         {
                             project.Id,
                             project.Name,
                             project.CreatedAt,
                         });

            var data = await query.Skip((page - 1) * perpage)
                                         .Take(perpage)
                                         .AsNoTracking()
                                         .ToListAsync();

            var count = await query.CountAsync();

            return Ok(new ApiRes("获取成功", new { data, count }));
        }

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="id">项目Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOne([FromRoute] ulong id)
        {
            var project = await dbContext.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(new ApiRes("获取成功", project));
        }

        /// <summary>
        /// 创建一个项目信息
        /// </summary>
        /// <param name="project">项目信息(其中项目Id 要求与房产Id 一致且属于同一个用户)</param>
        /// <returns></returns>
        /// <response code="200">创建成功</response>
        /// <response code="400">此房产信息已创建过项目</response>
        /// <response code="401">项目Id与用户的房产信息不匹配</response>
        [HttpPost]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> PostAsync([FromBody] Project project)
        {
            var uid = Guid.Parse(this.GetUserId());

            var hasHouse = (from item in dbContext.UserOwns
                            where item.UserId == uid && item.HouseId == project.Id
                            select item).Any();

            if (!hasHouse)
            {
                return Unauthorized(new ApiRes("项目Id与用户房产信息不匹配"));
            }

            try
            {
                await dbContext.Projects.AddAsync(project);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest(new ApiRes("此房产信息已创建项目"));
            }
            return Ok(new ApiRes("创建成功"));
        }

        /// <summary>
        /// 更新项目信息
        /// </summary>
        /// <param name="project">项目信息</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> PutAsync([FromBody] Project project)
        {
            var hasProject = (from item in dbContext.UserOwns
                              where item.Id == project.Id
                              select item).AnyAsync();

            var uid = this.GetUserId();

            if (hasProject == null)
            {
                return NotFound(new ApiRes("项目不存在"));
            }

            dbContext.Projects.Update(project);

            await dbContext.SaveChangesAsync();
            return Ok(new ApiRes("更新成功"));
        }

        /// <summary>
        /// 删除一个项目
        /// </summary>
        /// <param name="id">项目Id</param>
        /// <returns></returns>
        /// <response code="200">删除成功</response>
        /// <response code="401">不是你的项目</response>
        /// <response code="404">项目不存在或已被删除</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> DeleteAsync([FromRoute] ulong id)
        {
            var project = await dbContext.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound(new ApiRes("项目不存在"));
            }

            var uid = this.GetUserId();
            var isUserHasProject = await (from item in dbContext.UserOwns
                                          where item.UserId == Guid.Parse(uid)
                                          where item.HouseId == id
                                          select item).AnyAsync();
            if (!isUserHasProject)
            {
                return Unauthorized(new ApiRes("此项目并不属于该用户"));
            }
            dbContext.Projects.Remove(project);
            await dbContext.SaveChangesAsync();
            return Ok(new ApiRes("删除成功"));
        }

        /// <summary>
        /// 从房产信息Id查找项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("from-house/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFromHouseId([FromRoute] ulong id)
        {
            var project = await (from owns in dbContext.UserOwns
                           where owns.HouseId == id
                           select owns.Project).AsNoTracking().SingleOrDefaultAsync();

            if (project == null)
            {
                return NotFound("不存在此项目");
            }

            return Ok(new ApiRes("获取成功", project));
        }
    }
}
