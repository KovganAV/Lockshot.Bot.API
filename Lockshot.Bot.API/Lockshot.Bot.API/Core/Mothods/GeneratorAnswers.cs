using Lockshot.Bot.API.Core.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;
using System.Text.RegularExpressions;
using Lockshot.Bot.Models.Request;
using Lockshot.Bot.Models.Response;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace Lockshot.Bot.API.Core.Mothods
{
    public class GeneratorAnswers
    {

        IHuggingFaceRefit _huggingFaceRefit { get; set; }

        private const string MistralModel = "mistralai/Mistral-Nemo-Instruct-2407";
        private const string ZephyrModel = "HuggingFaceH4/zephyr-7b-alpha";
        

        public GeneratorAnswers(IHuggingFaceRefit huggingFaceRefit)
        {
            _huggingFaceRefit = huggingFaceRefit;

        }

        public async Task<string> MistralGeneration(BotRequest request)
        {
            string accumulatedResponse = string.Empty;
            string currentInput = request.Message;

            for (int i = 0; i < 1; i++)
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

                var response = await _huggingFaceRefit.PostQuestion(requestData, MistralModel);

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
        private string RemoveFirstTwoSentencesZephyr(string text)
        {
            var sentences = Regex.Split(text, @"(?<=[.!?])\s+").ToList();

            if (sentences.Count > 2)
            {
                sentences = sentences.Skip(2).ToList();
            }

            string result = string.Join(" ", sentences).Trim();

            var question = sentences.FirstOrDefault()?.Trim().ToLower();
            if (!string.IsNullOrEmpty(question))
            {
                result = Regex.Replace(result, Regex.Escape(question), "", RegexOptions.IgnoreCase).Trim();
            }

            return result;
        }
    }
}
