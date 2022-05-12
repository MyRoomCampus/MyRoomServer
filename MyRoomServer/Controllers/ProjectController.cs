using Microsoft.AspNetCore.Mvc;

namespace MyRoomServer.Controllers
{
    [Route("project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get([FromRoute] int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put([FromRoute]int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete([FromRoute]int id)
        {
        }
    }
}
