using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace chancies.Server.Api.Controllers.Health
{
    [ApiController]
    [Route("health")]
    public class HealthController
        : ControllerBase
    {
        public HealthController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
    }
}
