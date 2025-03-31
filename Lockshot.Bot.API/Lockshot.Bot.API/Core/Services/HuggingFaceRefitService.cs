using Lockshot.Bot.API.Core.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class HuggingFaceRefitService : IHuggingFaceRefit
{
    private readonly HttpClient _httpClient;

    public HuggingFaceRefitService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> PostQuestion(object requestData, string model)
    {
        return await _httpClient.PostAsJsonAsync($"https://api.huggingface.co/models/mistralai/Mistral-Nemo-Instruct-2407", requestData);
    }
}
