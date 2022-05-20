using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRoomServer.Entities;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var widget = await dbContext.Widgets.FindAsync(id);
            if (widget == null)
            {
                return NotFound();
            }
            return Ok(widget.TransferData);
        }

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
                return BadRequest();
            }

            await dbContext.Widgets.AddAsync(widget);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

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
                return BadRequest();
            }
            dbContext.Widgets.Update(widget);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var widget = await (from item in dbContext.Widgets
                                where item.Id == id
                                select item).SingleOrDefaultAsync();
            // TODO 以0判断并不健壮
            if (widget == null)
            {
                return BadRequest();
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
                return BadRequest();
            }

            dbContext.Widgets.Remove(widget);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
