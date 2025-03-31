using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lockshot.Bot.API.Core.Interfaces;
using Lockshot.Bot.API.Models;
using Lockshot.Bot.API.Data.Interfaces;

namespace Lockshot.Bot.API.Controllers
{
    [ApiController]
    [Route("api/chatbot")]
    public class ChatBotController : ControllerBase
    {
        private readonly IChatService _chatService;
        private const string AssistantPrefix = "Answer like a virtual assistant. ";

        public ChatBotController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Message cannot be empty.");

            var response = await _chatService.GenerateResponseAsync(AssistantPrefix + request.Message);
            return string.IsNullOrEmpty(response) ? StatusCode(500, "Failed to generate response.") : Ok(response);
        }

        [HttpPost("generate-advice")]
        public async Task<IActionResult> GenerateAdviceWithShots([FromBody] List<ShootingData> shots)
        {
            if (shots == null || !shots.Any())
                return BadRequest("No shooting data provided.");

            string shootingData = string.Join("\n", shots.Select(s =>
                $"Weapon: {s.WeaponType}, Score: {s.Score}, Distance: {s.Distance}m, Metrics: {s.Metrics}, Time: {s.Timestamp}"));

            string aiRequest = $"Imagine you're a professional shooting coach. Analyze the following shooting history and provide specific advice for improvement:\n\n{shootingData}";

            var response = await _chatService.GenerateResponseAsync(aiRequest);
            return string.IsNullOrEmpty(response) ? StatusCode(500, "Failed to generate advice.") : Ok(new { Advice = response });
        }
    }
}
