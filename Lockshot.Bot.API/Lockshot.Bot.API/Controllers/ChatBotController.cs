using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Lockshot.Bot.API.Core.Interfaces;
using Lockshot.Bot.API.Models;
using Lockshot.Bot.API.Data.Interfaces;

namespace Lockshot.Bot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            {
                return BadRequest("Message cannot be empty.");
            }

            request.Message = $"{AssistantPrefix}{request.Message}";

            var response = await _chatService.GenerateResponseAsync(request.Message);

            if (string.IsNullOrEmpty(response))
            {
                return StatusCode(500, "Failed to generate a response from the chatbot.");
            }

            return Ok(response);
        }

        [HttpPost("generateforprofile")]
        public async Task<IActionResult> GenerateAdviceForProfile([FromBody] ChatRequest request)
        {
            string text = "Imagine you're a coach of shooting as a sport. Give advice to beginners.";
            var response = await _chatService.GenerateResponseAsync(text);
            if (string.IsNullOrEmpty(response))
            {
                return NoContent();
            }
            return Ok(response);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateAdvice([FromBody] ChatRequest request)
        {
            string text = "Answer like a virtual assistant. ";
            request.Message = text + request.Message;
            var response = await _chatService.GenerateResponseAsync(request.Message);
            if (string.IsNullOrEmpty(response))
            {
                return NoContent();
            }
            return Ok(response);
        }

    }
}
