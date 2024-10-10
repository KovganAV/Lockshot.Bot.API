using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Lockshot.Bot.Models.Response;
using Lockshot.Bot.Models.Request;
using Lockshot.Bot.API.Core.Interfaces;

namespace Lockshot.Bot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BotsController : ControllerBase
    {

        private IBotsService _botsService;

        public BotsController(IBotsService botsService)
        {
            _botsService = botsService;

        }

        [HttpPost("/Message")]
        public async Task<ActionResult<BotResponse>> GetMessage(BotRequest request)
        {

            try
            {

                var result = await _botsService.GetAnswer(request);

                return Ok(result);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

    }
}
