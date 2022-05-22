using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRoomServer.Entities;
using MyRoomServer.Models;
using MyRoomServer.Extentions;
using Microsoft.EntityFrameworkCore;

namespace MyRoomServer.Controllers
{
    [Route("project")]
    [ApiController]
    public class ProjectController : ControllerBase
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
            return Ok(new ApiRes("获取成功", res));
        }

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="id">项目Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            var project = await dbContext.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(new ApiRes("获取成功", project.TransferData));
        }

        [HttpPost]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public IActionResult Post([FromBody] Project project)
        {
            project.UserId = Guid.Parse(this.GetUserId());
            dbContext.Projects.Add(project);
            return Ok(new ApiRes("上传成功"));
        }

        /// <summary>
        /// 更新项目信息
        /// </summary>
        /// <param name="id">项目Id</param>
        /// <param name="newProject">项目信息</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public IActionResult Put([FromRoute] long id, [FromBody] Project newProject)
        {
            var uid = this.GetUserId();
            var project = dbContext.Projects
                .Where(p => p.Id == id)
                .Where(p => p.UserId.ToString() == uid)
                .FirstOrDefault();
            if (project == null)
            {
                return NotFound(new ApiRes("项目不存在"));
            }
            project = newProject;
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
            return Ok(new ApiRes("删除成功" ,project.TransferData));
        }
    }
}
