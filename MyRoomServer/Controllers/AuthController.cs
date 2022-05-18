using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRoomServer.Entities;
using MyRoomServer.Services;
using System.ComponentModel.DataAnnotations;

namespace MyRoomServer.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly MyRoomDbContext dbContext;
        private readonly ITokenFactory tokenFactory;

        public AuthController(MyRoomDbContext dbContext, ITokenFactory tokenFactory)
        {
            this.dbContext = dbContext;
            this.tokenFactory = tokenFactory;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <response code="200">注册成功</response>
        /// <response code="400">该用户名已被注册</response>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm, Required, MinLength(6)] string username, [FromForm, Required, MinLength(6)] string password)
        {
            var hasUser = (from item in dbContext.Users
                        where item.UserName == username
                        select item).AsNoTracking().Any();
            if (hasUser)
            {
                return BadRequest();
            }

            await dbContext.Users.AddAsync(new User
            {
                UserName = username,
                Password = password,
            });

            await dbContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <response code="200">登录成功</response>
        /// <response code="400">密码错误或用户不存在</response>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromForm, Required] string username,
                                   [FromForm, Required] string password)
        {
            var user = (from item in dbContext.Users
                        where item.UserName == username
                        where item.Password == password
                        select item).AsNoTracking().SingleOrDefault();

            if (user is null)
            {
                return BadRequest();
            }

            var accessToken = tokenFactory.CreateAccessToken(user);
            var refreshToken = tokenFactory.CreateRefreshToken(user);
            return Ok(new
            {
                accessToken,
                refreshToken,
            });
        }
    }
}