using System.Threading.Tasks;

namespace Lockshot.Bot.API.Core.Interfaces
{
    public interface IChatBotService
    {
        Task<string> GenerateTextAsync(string userMessage);
    }
}
