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
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> Get([FromQuery] int page, [FromQuery] int perpage)
        {
            var uid = this.GetUserId();
            // TODO 验证在判断符号两边进行运算的影响
            var res = await (from item in dbContext.Projects
                             where item.UserId == Guid.Parse(uid)
                             select item).Skip((page - 1) * perpage)
                                         .Take(perpage)
                                         .AsNoTracking()
                                         .ToListAsync();

            var count = (from item in dbContext.Projects
                         where item.UserId == Guid.Parse(uid)
                         select item).CountAsync();

            return Ok(new ApiRes("获取成功", new { res, count }));
        }

        ///// <summary>
        ///// 获取项目信息
        ///// </summary>
        ///// <param name="id">项目Id</param>
        ///// <returns></returns>
        //[HttpGet("{id}")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetAsync([FromRoute] int id)
        //{
        //    var project = await dbContext.Projects.FindAsync(id);
        //    if (project == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(new ApiRes("获取成功", project.TransferData));
        //}

        /// <summary>
        /// 创建一个项目信息
        /// </summary>
        /// <param name="projectPost">项目信息</param>
        /// <returns></returns>
        /// <response code="200">创建成功</response>
        [HttpPost]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> PostAsync([FromBody] Project projectPost)
        {
            var uid = Guid.Parse(this.GetUserId());
            var project = new Project
            {
                Name = projectPost.Name,
                UserId = uid
            };
            await dbContext.Projects.AddAsync(project);
            await dbContext.SaveChangesAsync();
            return Ok(new ApiRes("创建成功"));
        }

        /// <summary>
        /// 更新项目信息
        /// </summary>
        /// <param name="id">项目Id</param>
        /// <param name="projectPut">项目信息</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> PutAsync([FromRoute] long id, [FromBody] Project projectPut)
        {
            var uid = this.GetUserId();
            var project = await dbContext.Projects
                .Where(p => p.Id == id)
                .Where(p => p.UserId.ToString() == uid)
                .FirstOrDefaultAsync();
            if (project == null)
            {
                return NotFound(new ApiRes("项目不存在"));
            }
            project.Name = projectPut.Name;

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
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var project = await dbContext.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound(new ApiRes("项目不存在"));
            }
            var uid = this.GetUserId();
            if (project.UserId.ToString() != uid)
            {
                return Unauthorized(new ApiRes("此项目并不属于该用户"));
            }
            dbContext.Projects.Remove(project);
            await dbContext.SaveChangesAsync();
            return Ok(new ApiRes("删除成功"));
        }
    }
}
