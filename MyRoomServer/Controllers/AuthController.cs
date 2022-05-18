using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRoomServer.Entities;
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

        public AuthController(MyRoomDbContext dbContext, ITokenFactory tokenFactory)
        {
            this.dbContext = dbContext;
            this.tokenFactory = tokenFactory;
        }

        /// <summary>
        /// �û�ע��
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="password">����</param>
        /// <response code="200">ע��ɹ�</response>
        /// <response code="400">���û����ѱ�ע��</response>
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
        /// �û���¼
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="password">����</param>
        /// <response code="200">��¼�ɹ�</response>
        /// <response code="400">���������û�������</response>
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

        [HttpGet("test")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public IActionResult Test()
        {
            return Ok();
        }

        /// <summary>d
        /// ˢ�� AccessToken �� RefreshToken
        /// </summary>
        /// <returns>�µ� AccessToken���� RefreshToken �������ڣ���ͬʱˢ�� AccessToken �� RefreshToken</returns>
        /// <response code="200">�ɹ�</response>
        /// <response code="400">�û�������</response>
        /// <response code="401">userId Ϊ null</response>
        [HttpGet("refresh")]
        [Authorize(Policy = IdentityPolicyNames.RefreshTokenOnly)]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Single().Value;
            if(userId == null)
            {
                return Unauthorized("userId is null.");
            }
            var user = await dbContext.Users.FindAsync(Guid.Parse(userId));
            if(user == null)
            {
                return BadRequest("user not exist.");
            }

            var accessToken = tokenFactory.CreateAccessToken(user);

            // ����RefreshTokenʣ����Ч��
            var expireTime = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Exp)
                                         .Select(c => Convert.ToInt32(c.Value).ToDateTime())
                                         .Single();
            var remainMinutes = (int)(expireTime - DateTime.Now).TotalMinutes;

            // RefreshToken�������ڣ���ͬʱˢ��AccessToken��RefreshToken
            if (remainMinutes < tokenFactory.RefreshTokenExpireBefore)
            {
                return Ok(new
                {
                    AccessToken = accessToken,
                    RefreshToken = tokenFactory.CreateRefreshToken(user)
                });
            }
            // ֻˢ��AccessToken
            else
            {
                return Ok(new
                {
                    AccessToken = accessToken
                });
            }
        }
    }
}