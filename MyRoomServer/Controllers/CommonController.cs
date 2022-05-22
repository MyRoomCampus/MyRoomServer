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
    [Route("common")]
    public class CommonController : ControllerBase
    {
        /// <summary>
        /// 项目存活测试
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("pong");
        }
    }
}