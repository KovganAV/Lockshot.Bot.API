using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Lockshot.Bot.API.Core.Services;
using Lockshot.Bot.API.Core.Interfaces;
using Lockshot.Bot.API.Models;
using Lockshot.Bot.API.Data.Services;
using Lockshot.Bot.API.Data.Interfaces;


namespace Lockshot.Bot.API.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class chatBotController : ControllerBase
        {
            private readonly IChatService _chatService;

            public chatBotController(IChatService chatService)
            {
                _chatService = chatService;
            }

            [HttpPost("generate")]
            public async Task<IActionResult> Generate([FromBody] ChatRequest request)
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
