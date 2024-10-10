using Lockshot.Bot.API.Core.Interfaces;
using Lockshot.Bot.API.Core.Mothods;
using Microsoft.AspNetCore.Http.HttpResults;
using Lockshot.Bot.Models.Request;
using Lockshot.Bot.Models.Response;
using Discord.Common.Bots;


namespace Lockshot.Bot.API.Core.Services
{
    public class BotsService : IBotsService
    {

        private GeneratorAnswers _generator;

        Dictionary<BotType, GeneratorAnswers.GetAnswer> operations;

        public BotsService(GeneratorAnswers generator)
        {

            this._generator = generator;

            operations = new Dictionary<BotType, GeneratorAnswers.GetAnswer>
            {

                {BotType.Phi, _generator.PhiGeneration },

                {BotType.Mistral, _generator.MistralGeneration },

                {BotType.LM, _generator.ZephyrGeneration },

            };

        }

        public async Task<BotResponse> GetAnswer(BotRequest request)
        {

            try
            {

                if (operations.TryGetValue(request.BotType, out GeneratorAnswers.GetAnswer answer))
                {

                    string genText = await answer(request);

                    var result = new BotResponse()
                    {
                        Answer = genText,
                        BotType = request.  BotType,
                    };

                    return result;

                }
                else
                {

                    throw new Exception("Invalid type");

                }

            }
            catch (Exception ex)
            {

                var result = new BotResponse()
                {
                    Answer = ex.Message,
                    BotType = request.BotType,
                };

                return result;

            }
        }


    }
}
