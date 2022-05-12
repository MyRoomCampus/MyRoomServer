using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MyRoomServer.Entities;
using MyRoomServer.Models;
using MyRoomServer.Services;
using System.ComponentModel.DataAnnotations;

namespace MyRoomServer.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly MyRoomDbContext dbContext;
        private readonly TokenFactory tokenFactory;

        public AuthController(MyRoomDbContext dbContext, TokenFactory tokenFactory)
        {
            this.dbContext = dbContext;
            this.tokenFactory = tokenFactory;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody, Required, EmailAddress] string email,
                                   [FromBody, Required] string password)
        {
            // TODO º”√‹√‹¬Î
            var user = (from item in dbContext.Users
                        where item.Email == email
                        where item.Password == password
                        select item).SingleOrDefault();

            if (user == null)
            {
                return BadRequest("user not found.");
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
        public async Task<IActionResult> SignUpAsync([FromBody, Required, EmailAddress] string email,
                                                     [FromBody, Required] string password)
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
            return Ok(new { Message = "◊¢≤·≥…π¶" });
        }
    }
}