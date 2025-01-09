using Microsoft.AspNetCore.Mvc;
using SonarBot.Models;
using System.Text.Json;

namespace SonarBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : Controller
    {
        private readonly ILogger<WebhookController> _logger;

        private readonly JsonSerializerOptions serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public WebhookController(ILogger<WebhookController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> Handle([FromBody] JsonElement payload)
        {
            var webhookPayload = JsonSerializer.Deserialize<GithubWebhook>(payload.GetRawText(), serializerOptions);

            return Ok();
        }
    }
}
