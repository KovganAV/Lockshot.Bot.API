using Lockshot.Bot.API.Data.Interfaces;
using Lockshot.Bot.API.Data.Services;


namespace Lockshot.Bot.API.Data.Interfaces
{
    public interface IChatService
    {
        Task<string> GenerateResponseAsync(string input);
    }
}
