using Lockshot.Bot.API.Data.Interfaces;
using Lockshot.Bot.API.Data.Services;


namespace Lockshot.Bot.API.Core.Interfaces
{
    public interface IChatBotService
    {
        Task<string> GenerateTextAsync(string userMessage);

    }
}
