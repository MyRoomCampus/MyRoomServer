using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRoomServer.Entities;
using MyRoomServer.Models;
using MyRoomServer.Extentions;
using Microsoft.EntityFrameworkCore;
using MyRoomServer.Entities.Contexts;
using System.ComponentModel.DataAnnotations;

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
        public async Task<IActionResult> Get([FromQuery, Required] int page, [FromQuery, Required] int perpage)
        {
            var uid = this.GetUserId();

            var query = (from own in dbContext.UserOwns
                         join project in dbContext.Projects
                         on own.ProjectId equals project.Id
                         select new
                         {
                             own.HouseId,
                             project.Name,
                             project.IsPublished,
                             project.CreatedAt,
                         });

            var data = await query.Skip((page - 1) * perpage)
                                         .Take(perpage)
                                         .AsNoTracking()
                                         .ToListAsync();

            var count = await query.CountAsync();

            // TODO 也许需要修改一下 返回格式 但如何优雅呢？

            return Ok(new ApiRes("获取成功", new { data, count }));
        }

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="id">房产信息Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOne([FromRoute] ulong id)
        {
            var ownInfo = await (from item in dbContext.UserOwns
                                 where item.HouseId == id
                                 select new
                                 {
                                     item.ProjectId,
                                     item.UserId
                                 }).SingleOrDefaultAsync();

            // 若此项目不属于该用户 则用户查询不到未公开的项目
            if (ownInfo == null)
            {
                return NotFound(new ApiRes("项目不存在"));
            }

            if (ownInfo.ProjectId == null)
            {
                return NotFound(new ApiRes("项目不存在"));
            }

            var project = await dbContext.Projects.FindAsync(ownInfo.ProjectId);

            var uid = this.GetUserId();

            if (project == null || ((uid == null || ownInfo.UserId != Guid.Parse(uid)) && project.IsPublished == false))
            {
                return NotFound(new ApiRes("项目不存在"));
            }

            return Ok(new ApiRes("获取成功", new
            {
                HouseId = id,
                Name = project.Name,
                CreatedAt = project.CreatedAt,
                IsPublished = project.IsPublished,
                Data = project.Data,
            }));
        }

        /// <summary>
        /// 创建一个项目信息
        /// </summary>
        /// <param name="project">项目信息(其中项目Id 要求与房产Id 一致且属于同一个用户)</param>
        /// <returns></returns>
        /// <response code="200">创建成功</response>
        /// <response code="400">此房产信息已创建过项目</response>
        /// <response code="401">此房产信息不属于该用户</response>
        [HttpPost]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> PostAsync([FromBody, Required] TransferProject project)
        {
            var uid = Guid.Parse(this.GetUserId());

            var ownInfo = (from item in dbContext.UserOwns
                           where item.UserId == uid && item.HouseId == project.HouseId
                           select item).SingleOrDefault();

            if (ownInfo == null)
            {
                return BadRequest(new ApiRes("此房产信息不属于该用户"));
            }

            if (ownInfo.Project != null)
            {
                return BadRequest(new ApiRes("此房产信息已有项目"));
            }

            ownInfo.Project = new Project
            {
                Name = project.Name,
                Data = project.Data,
            };

            await dbContext.SaveChangesAsync();
            return Ok(new ApiRes("创建成功"));
        }

        /// <summary>
        /// 更新项目信息
        /// </summary>
        /// <param name="transferProject">项目信息</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> PutAsync([FromBody, Required] TransferProject transferProject)
        {
            var uid = Guid.Parse(this.GetUserId());

            var ownInfo = (from item in dbContext.UserOwns
                           where item.UserId == uid && item.HouseId == transferProject.HouseId
                           select item).SingleOrDefault();

            if (ownInfo == null)
            {
                return BadRequest(new ApiRes("此房产信息不属于该用户"));
            }

            if (ownInfo.ProjectId == null)
            {
                return BadRequest(new ApiRes("此房产信息尚未创建项目"));
            }

            var project = await dbContext.Projects.FindAsync(ownInfo.ProjectId);

            if (project == null)
            {
                throw new NullReferenceException("Project shouldn't be null.");
            }

            project.Name = transferProject.Name;
            project.Data = transferProject.Data;

            await dbContext.SaveChangesAsync();
            return Ok(new ApiRes("修改成功"));
        }

        /// <summary>
        /// 更新项目是否发布
        /// </summary>
        /// <param name="id">房产id</param>
        /// <param name="isPublish">是否发布</param>
        /// <returns></returns>
        [HttpPut("publish/{id}")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> PutIsPublish([FromRoute] ulong id, [FromForm, Required] bool isPublish)
        {
            var uid = Guid.Parse(this.GetUserId());

            var project = await (from own in dbContext.UserOwns
                                 where own.HouseId == id
                                 where own.UserId == uid
                                 select own.Project).SingleOrDefaultAsync();

            if (project == null)
            {
                return NotFound("用户不存在此项目");
            }

            project.IsPublished = isPublish;
            await dbContext.SaveChangesAsync();

            return Ok(new ApiRes("修改成功"));
        }

        /// <summary>
        /// 删除一个项目
        /// </summary>
        /// <param name="id">房产信息Id</param>
        /// <returns></returns>
        /// <response code="200">删除成功</response>
        /// <response code="401">不是你的项目</response>
        /// <response code="404">项目不存在或已被删除</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> DeleteAsync([FromRoute] ulong id)
        {
            var uid = Guid.Parse(this.GetUserId());

            var ownInfo = (from item in dbContext.UserOwns
                           where item.UserId == uid && item.HouseId == id
                           select item).SingleOrDefault();

            if (ownInfo == null)
            {
                return Unauthorized(new ApiRes("项目不属于该用户"));
            }

            if (ownInfo.ProjectId == null)
            {
                return NotFound(new ApiRes("项目不存在"));
            }

            var project = await dbContext.Projects.FindAsync(ownInfo.ProjectId);
            dbContext.Projects.Remove(project!);
            ownInfo.ProjectId = null;

            await dbContext.SaveChangesAsync();
            return Ok(new ApiRes("删除成功"));
        }
    }
}
