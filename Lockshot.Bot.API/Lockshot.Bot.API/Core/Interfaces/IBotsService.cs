using Microsoft.AspNetCore.Http.HttpResults;
using Lockshot.Bot.Models.Request;
using Lockshot.Bot.Models.Response;

namespace Lockshot.Bot.API.Core.Interfaces
{
    public interface IBotsService
    {

        Task<BotResponse> GetAnswer(BotRequest request);

    }    
}
