using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRoomServer.Entities;
using MyRoomServer.Entities.Contexts;
using MyRoomServer.Extentions;
using MyRoomServer.Models;

namespace MyRoomServer.Controllers
{
    [ApiController]
    [Route("resource")]
    public class MediaController : ControllerBase
    {
        private readonly MyRoomDbContext dbContext;

        public MediaController(MyRoomDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="id">资源的Id</param>
        /// <param name="ifModifiedSince">用于缓存协商</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetResource([FromRoute] Guid id, [FromHeader(Name = "If-Modified-Since")] DateTime? ifModifiedSince)
        {
            var queryImage = async delegate (Guid id)
            {
                return await dbContext.Medias
                    .Where(x => x.Id == id)
                    .Select(x => x.Content)
                    .AsNoTracking()
                    .SingleAsync();
            };

            var query = await dbContext.Medias
                    .Where(x => x.Id == id)
                    .Select(x => new { x.UpdatedAt })
                    .SingleOrDefaultAsync();

            if (query == null)
            {
                return NotFound();
            }

            if (ifModifiedSince != null && query.UpdatedAt < ifModifiedSince)
            {
                return await Task.FromResult(StatusCode(304));
            }

            var res = await dbContext.Medias
                .Where(x => x.Id == id)
                .Select(x => x)
                .AsNoTracking()
                .SingleAsync();

            Response.AddLastModified(res.UpdatedAt);

            return File(res.Content, res.Type);
        }

        /// <summary>
        /// 上传资源，最大 10MB
        /// </summary>
        /// <param name="resource">资源数据</param>
        /// <param name="type">资源数据的格式</param>
        /// <returns></returns>
        /// <response code="200">成功</response>
        /// <response code="413">资源过大</response>
        [HttpPost]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> OnPost([FromForm] IFormFile resource, [FromForm] string type)
        {
            if (resource.Length > 1024 * 1024 * 10) return await Task.FromResult(StatusCode(413));
            var buffer = new byte[resource.Length];

            using var avatarStream = resource.OpenReadStream();
            await avatarStream.ReadAsync(buffer);

            var newResource = new Media
            {
                Size = buffer.Length,
                Content = buffer,
                Type = type
            };

            dbContext.Medias.Add(newResource);

            await dbContext.SaveChangesAsync();

            return Ok(newResource.Id);
        }
    }
}
