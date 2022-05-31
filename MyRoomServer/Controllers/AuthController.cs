using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRoomServer.Entities;
using MyRoomServer.Entities.Contexts;
using MyRoomServer.Extentions;
using MyRoomServer.Models;
using MyRoomServer.Services;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyRoomServer.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly MyRoomDbContext dbContext;
        private readonly ITokenFactory tokenFactory;
        private readonly string salt;

        public AuthController(MyRoomDbContext dbContext, ITokenFactory tokenFactory, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.tokenFactory = tokenFactory;
            this.salt = configuration.GetValue<string>("PasswordSalt");
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="username">用户名(长度为6-20)</param>
        /// <param name="password">密码(长度为6-20)</param>
        /// <response code="200">注册成功</response>
        /// <response code="400">该用户名已被注册</response>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiRes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiRes), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync(
            [FromForm, Required, MinLength(6), MaxLength(20)] string username,
            [FromForm, Required, MinLength(6), MaxLength(20)] string password)
        {
            var hasUser = (from item in dbContext.Users
                           where item.UserName == username
                           select item).AsNoTracking().Any();
            if (hasUser)
            {
                return BadRequest(new ApiRes("该用户名已注册"));
            }

            await dbContext.Users.AddAsync(new User
            {
                UserName = username,
                Password = password.Sha256(salt),
            });

            await dbContext.SaveChangesAsync();

            return Ok(new ApiRes("用户注册成功"));
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户名(长度为6-20)</param>
        /// <param name="password">密码(长度为6-20)</param>
        /// <response code="200">登录成功</response>
        /// <response code="400">密码错误或用户不存在</response>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesErrorResponseType(typeof(ApiRes))]
        public IActionResult Login([FromForm, Required, MinLength(6), MaxLength(20)] string username,
                                   [FromForm, Required, MinLength(6), MaxLength(20)] string password)
        {
            var user = (from item in dbContext.Users
                        where item.UserName == username
                        where item.Password == password.Sha256(salt)
                        select item).AsNoTracking().SingleOrDefault();

            if (user is null)
            {
                return BadRequest(new ApiRes("密码错误或用户不存在"));
            }

            var accessToken = tokenFactory.CreateAccessToken(user);
            var refreshToken = tokenFactory.CreateRefreshToken(user);
            return Ok(new
            {
                accessToken,
                refreshToken,
            });
        }

        /// <summary>
        /// 用户权限测试
        /// </summary>
        /// <returns></returns>
        [HttpGet("test")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public IActionResult Test()
        {
            return Ok(new ApiRes("鉴权成功"));
        }

        /// <summary>
        /// 刷新 AccessToken 及 RefreshToken
        /// </summary>
        /// <returns>新的 AccessToken，若 RefreshToken 即将过期，则同时刷新 AccessToken 和 RefreshToken</returns>
        /// <response code="200">成功</response>
        /// <response code="400">用户不存在</response>
        /// <response code="401">userId 为 null，RefreshToken过期</response>
        [HttpGet("refresh")]
        [Authorize(Policy = IdentityPolicyNames.RefreshTokenOnly)]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            var userId = this.GetUserId();
            var user = await dbContext.Users.FindAsync(Guid.Parse(userId));
            if (user == null)
            {
                return BadRequest(new ApiRes("用户不存在"));
            }

            var accessToken = tokenFactory.CreateAccessToken(user);

            // 计算RefreshToken剩余有效期
            var expireTime = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Exp)
                                        .Select(c => Convert.ToInt32(c.Value).ToDateTime())
                                        .Single();

            var remainMinutes = (int)(expireTime - DateTime.Now).TotalMinutes;

            // RefreshToken即将过期，则同时刷新AccessToken和RefreshToken
            if (remainMinutes < tokenFactory.RefreshTokenExpireBefore)
            {
                return Ok(new
                {
                    AccessToken = accessToken,
                    RefreshToken = tokenFactory.CreateRefreshToken(user)
                });
            }

            // 只刷新AccessToken
            else
            {
                return Ok(new
                {
                    AccessToken = accessToken
                });
            }
        }

        /// <summary>
        /// 更新用户验证信息
        /// </summary>
        /// <param name="username">用户名（非必填）</param>
        /// <param name="password">密码（非必填）</param>
        /// <returns></returns>
        /// <response code="200">成功</response>
        /// <response code="400">请求更改用户验证信息的用户不存在或用户名已被占用</response>
        [HttpPut("validate-info")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> UpdateUserValidateInfoAsync(
            [MinLength(6), MaxLength(20)] string? username,
            [MinLength(6), MaxLength(20)] string? password)
        {
            var uid = this.GetUserId();
            var user = await dbContext.Users.FindAsync(Guid.Parse(uid));
            if (user == null)
            {
                return BadRequest(new ApiRes("用户不存在"));
            }
            if (username != null)
            {
                user.UserName = username;
            }
            if (password != null)
            {
                user.Password = password.Sha256(salt);
            }
            dbContext.Users.Update(user);

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest(new ApiRes("该用户名已被使用"));
            }
            return Ok(new ApiRes("更改成功"));
        }

        /// <summary>
        /// 检查此用户名是否已被使用
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        /// <response code="200">用户名可用</response>
        /// <response code="400">用户名不可用</response>
        [HttpGet("check-username/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUserValidateInfoAsync(
            [MinLength(6), MaxLength(20)] string username)
        {
            var ok = await (from item in dbContext.Users
                            where item.UserName == username
                            select item).AsNoTracking().AnyAsync();
            if (!ok)
            {
                return Ok(new ApiRes("此用户名可用"));
            }
            else
            {
                return BadRequest(new ApiRes("此用户名不可用"));
            }
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <returns></returns>
        /// <response code="200">注销成功</response>
        /// <response code="400">使用了已注销的用户访问此接口</response>
        /// <response code="401">未登录</response>
        [HttpDelete("cancel")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> UserCancel()
        {
            var uid = this.GetUserId();
            var user = await dbContext.Users.FindAsync(uid);

            if (user == null)
            {
                return BadRequest(new ApiRes("用户不存在"));
            }

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
            return Ok(new ApiRes("注销成功"));
        }
    }
}