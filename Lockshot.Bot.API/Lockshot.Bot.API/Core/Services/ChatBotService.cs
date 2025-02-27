using Lockshot.Bot.API.Core.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Lockshot.Bot.API.Core.Services
{
    public class HuggingFaceService : IChatBotService
    {
        private readonly HttpClient _httpClient;

        public HuggingFaceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GenerateTextAsync(string userMessage)
        {
            string accumulatedResponse = string.Empty;
            string currentInput = userMessage;

                var requestData = new
                {
                    inputs = currentInput,
                    parameters = new
                    {
                        max_length = 1000,
                        temperature = 0.7
                    }
                };

                var response = await _httpClient.PostAsJsonAsync(string.Empty, requestData);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error calling API: {response.StatusCode}, Error content: {errorContent}");
                    return $"API Error: {response.StatusCode}";
                }

                var huggingFaceResponses = await response.Content.ReadFromJsonAsync<List<HuggingFaceResponse>>();

                if (huggingFaceResponses != null && huggingFaceResponses.Count > 0)
                {
                    var generatedText = huggingFaceResponses[0]?.GeneratedText;
                    if (!string.IsNullOrEmpty(generatedText))
                    {
                        accumulatedResponse += generatedText;
                        currentInput = generatedText;
                    }
                }

            var formattedResponse = FormatResponse(accumulatedResponse);
            formattedResponse = RemoveFirstTwoSentences(formattedResponse);

            return formattedResponse;
        }

        private string FormatResponse(string response)
        {
            response = response.Replace("\n", "<br>");
            response = Regex.Replace(response, @"(\d+\.\s)", "<br>$1");
            response = Regex.Replace(response, @"\*\*(.*?)\*\*", "<strong>$1</strong>");
            response = response.Trim();
            return response;
        }

        private string RemoveFirstTwoSentences(string text)
        {
            var sentences = Regex.Split(text, @"(?<=[.!?])\s+");

            if (sentences.Length > 2)
            {
                return string.Join(" ", sentences.Skip(2));
            }

            return text;
        }

        private class HuggingFaceResponse
        {
            [JsonPropertyName("generated_text")]
            public string GeneratedText { get; set; }
        }
    }
}
