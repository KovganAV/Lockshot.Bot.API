
using Refit;

namespace Lockshot.Bot.API.Core.Interfaces
{
    public interface IHuggingFaceRefit
    {

        [Post("/{model}")]
        Task<HttpResponseMessage> PostQuestion([Body] object promt, string model);

    }
}
