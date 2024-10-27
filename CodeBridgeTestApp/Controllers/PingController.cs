using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CodeBridgeTestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("fixed")]
    public class PingController : ControllerBase
    {
        [HttpGet("ping")]
        public IActionResult Ping() => Ok("Dogshouseservice.Version1.0.1");
    }
}
