using Lockshot.Bot.API.Core.Interfaces;
using Lockshot.Bot.API.Data.Interfaces;


namespace Lockshot.Bot.API.Data.Services
{
        public class ChatService : IChatService
        {
            private readonly IChatBotService _chatBotService;

            public ChatService(IChatBotService chatBotService)
            {
                _chatBotService = chatBotService;
            }
        public async Task<string> GenerateResponseAsync(string input)
            {
                var response = await _chatBotService.GenerateTextAsync(input);
                if (string.IsNullOrEmpty(response))
                {
                    return null;
                }
                return response;
            }

        }
}
