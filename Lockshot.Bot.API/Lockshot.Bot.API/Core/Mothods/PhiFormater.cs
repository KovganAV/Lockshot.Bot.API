using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;

namespace Lockshot.Bot.API.Core.Mothods
{
    public static class PhiFormater
    {

        private readonly static string systemPrompt = "You are an AI assistant that helps people find information. Answer questions using a direct style. Do not share more information that the requested by the users.";

        private readonly static string continuePromt = "Continue your answer, but keep it midle context and without repeating your text. If you have finished your answer, send a blank line.";

        public static StringContent GeneratePromt(int flag, string content, string genText)
        {

            var sysPromt = flag == 0 ? systemPrompt : continuePromt;

            string fullPromt = $"<|system|>{sysPromt}<|end|><|user|>{content}<|end|><|assistant|>{genText}";

            var requestBody = new
            {
                role = "user",
                inputs = fullPromt,
                parameters = new
                {
                    max_tokens = 1000,
                    stream = true,
                    max_length = 10000,
                    temperature = 0.5,
                    return_full_text = true,
                    do_sample = true,
                }
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            return jsonContent;

        }

        public async static Task<string> GetStringResponse(HttpResponseMessage response, int poss)
        {


            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var messageArray = JArray.Parse(line);
                    foreach (var message in messageArray)
                    {
                        var generatedText = message["generated_text"]?.ToString();
                        if (!string.IsNullOrEmpty(generatedText))
                        {
                            string firstAnswer = ExtractAssistantResponse(generatedText);
                            string result = firstAnswer.Substring(poss, firstAnswer.Length - poss);
                            return firstAnswer;
                        }
                    }
                }
            }

            return "";

        }

        private static string ExtractAssistantResponse(string text)
        {

            // Регулярное выражение для поиска текста после <|assistant|>
            string pattern = @"<\|assistant\|>(.*?)(<\|end\|>|$)";
            Match match = Regex.Match(text, pattern, RegexOptions.Singleline);

            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            return string.Empty;
        }

    }
}
