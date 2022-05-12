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

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromForm, Required, EmailAddress] string email,
                                   [FromForm, Required] string password)
        {
            // TODO º”√‹√‹¬Î
            var user = (from item in dbContext.Users
                        where item.Email == email
                        where item.Password == password
                        select item).AsNoTracking().SingleOrDefault();

            if (user is null)
            {
                return BadRequest(new { Message = "”√ªß≤ª¥Ê‘⁄" });
            }

            var accessToken = tokenFactory.CreateAccessToken(user);
            var refreshToken = tokenFactory.CreateRefreshToken(user);
            return Ok(new
            {
                accessToken,
                refreshToken,
            });
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromForm, Required, EmailAddress] string email,
                                                     [FromForm, Required] string password)
        {
            #region ≈–∂œ” œ‰◊¢≤·
            var userQuery = dbContext.Users.Where(u => email == u.Email);
            if (userQuery.Any())
            {
                return BadRequest(new { Message = "” œ‰“—±ª◊¢≤·" });
            }
            #endregion
            await dbContext.Users.AddAsync(new User
            {
                Email = email,
                Password = password,
            });
            await dbContext.SaveChangesAsync();
            return Ok(new { Message = "◊¢≤·≥…π¶" });
        }
    }
}