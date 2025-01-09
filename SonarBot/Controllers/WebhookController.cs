using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SonarBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : Controller
    {
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(ILogger<WebhookController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Handle([FromBody] JsonElement payload)
        {

            return Ok();
        }
    }
}
