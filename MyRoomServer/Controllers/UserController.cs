using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRoomServer.Entities.Contexts;
using MyRoomServer.Extentions;
using MyRoomServer.Extentions.Validations;
using MyRoomServer.Models;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyRoomServer.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyRoomDbContext dbContext;
        private readonly string salt;

        public UserController(MyRoomDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.salt = configuration.GetValue<string>("PasswordSalt");
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="password">新密码</param>
        /// <returns></returns>
        /// <response code="200">更改成功</response>
        /// <response code="400">用户不存在（用户在注销的情况下访问此接口）</response>
        [HttpPut("password")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> UpdatePassword([Password, Required, FromForm] string password)
        {
            var uid = this.GetUserId();
            var user = await dbContext.Users.FindAsync(Guid.Parse(uid));
            if (user == null)
            {
                return BadRequest(new ApiRes("用户不存在"));
            }
            user.Password = password.Sha256(salt);
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return Ok(new ApiRes("更改成功"));
        }
    }
}
