using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRoomServer.Entities;
using MyRoomServer.Entities.Contexts;
using MyRoomServer.Extentions;
using MyRoomServer.Models;

namespace MyRoomServer.Controllers
{
    [Route("widget")]
    [ApiController]
    public class WidgetController : ControllerBase
    {
        private readonly MyRoomDbContext dbContext;

        public WidgetController(MyRoomDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// 获取一个组件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var widget = await dbContext.Widgets.FindAsync(id);
            if (widget == null)
            {
                return NotFound();
            }
            return Ok(new ApiRes("获取成功", widget.TransferData));
        }

        /// <summary>
        /// 添加一个组件
        /// </summary>
        /// <param name="widget"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> Post([FromBody] Widget widget)
        {
            // TODO 此处优化 缓存用户所拥有的 ProjectId 当后边修改的时候不用反复查表
            var uid = this.GetUserId();
            var hasProject = (from item in dbContext.Projects
                              where item.Id == widget.ProjectId
                              where item.UserId == Guid.Parse(uid)
                              select new { })
                              .AsNoTracking()
                              .Any();

            if (!hasProject)
            {
                return BadRequest(new ApiRes("项目不存在"));
            }

            await dbContext.Widgets.AddAsync(widget);
            await dbContext.SaveChangesAsync();
            return Ok(new ApiRes("添加成功"));
        }

        /// <summary>
        /// 修改一个组件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="widget"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] Widget widget)
        {
            var uid = this.GetUserId();
            var hasProject = (from item in dbContext.Projects
                              where item.Id == widget.ProjectId
                              where item.UserId == Guid.Parse(uid)
                              select new { })
                              .AsNoTracking()
                              .Any();
            if (!hasProject)
            {
                return BadRequest(new ApiRes("项目不存在"));
            }
            dbContext.Widgets.Update(widget);
            await dbContext.SaveChangesAsync();
            return Ok(new ApiRes("修改成功"));
        }

        /// <summary>
        /// 删除一个组件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var widget = await (from item in dbContext.Widgets
                                where item.Id == id
                                select item).SingleOrDefaultAsync();

            if (widget == null)
            {
                return BadRequest(new ApiRes("组件不存在"));
            }

            var uid = this.GetUserId();
            var hasProject = (from item in dbContext.Projects
                              where item.Id == widget.ProjectId
                              where item.UserId == Guid.Parse(uid)
                              select new { })
                              .AsNoTracking()
                              .Any();

            if (!hasProject)
            {
                return BadRequest(new ApiRes("项目不存在"));
            }

            dbContext.Widgets.Remove(widget);
            await dbContext.SaveChangesAsync();
            return Ok(new ApiRes("组件删除成功"));
        }
    }
}
