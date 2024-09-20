using Lockshot.Bot.API.Core.Interfaces;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lockshot.Bot.API.Data.Interfaces;
using Lockshot.Bot.API.Data.Services;


namespace Lockshot.Bot.API.Core.Services
{


        public class HuggingFaceService : IChatBotService
        {
            private readonly HttpClient _httpClient;
            private const string ApiUrl = "https://api-inference.huggingface.co/models/mistralai/Mistral-Nemo-Instruct-2407";
            private readonly string _apiKey;

            public HuggingFaceService(string apiKey)
            {
                _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                _apiKey = apiKey;
            }

            public async Task<string> GenerateTextAsync(string userMessage)
            {
                string accumulatedResponse = string.Empty;
                string currentInput = userMessage;

                for (int i = 0; i < 3; i++)
                {
                    var requestData = new
                    {
                        inputs = currentInput,
                        parameters = new
                        {
                            max_length = 1000,
                            temperature = 0.7
                        }
                    };

                    var response = await _httpClient.PostAsJsonAsync(ApiUrl, requestData);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error calling API: {response.StatusCode}, Error content: {errorContent}");
                        throw new HttpRequestException($"Error calling API: {response.StatusCode}, Error content: {errorContent}");
                    }

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API response: {jsonResponse}");

                    using (JsonDocument document = JsonDocument.Parse(jsonResponse))
                    {
                        if (document.RootElement.ValueKind == JsonValueKind.Array && document.RootElement.GetArrayLength() > 0)
                        {
                            var firstElement = document.RootElement[0];
                            if (firstElement.TryGetProperty("generated_text", out JsonElement generatedTextElement))
                            {
                                var generatedText = generatedTextElement.GetString();
                                accumulatedResponse += generatedText;
                                currentInput = generatedText;
                            }
                        }
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
                public string GeneratedText { get; set; }
            }
        }
    }

