using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRoomServer.Models;
using Qiniu.Storage;
using Qiniu.Util;

namespace MyRoomServer.Controllers
{
    [Route("storage")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly QiniuConfiguration config;

        public StorageController(IConfiguration configuration)
        {
            config = new QiniuConfiguration();
            configuration.GetSection("Qiniu").Bind(config);
        }

        [HttpGet]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public IActionResult GetToken()
        {
            var mac = new Mac(config.AccessKey, config.SecretKey);
            var putPolicy = new PutPolicy
            {
                Scope = config.Bucket
            };

            var token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            return Ok(new ApiRes("获取 token 成功", token));
        }
    }
}
