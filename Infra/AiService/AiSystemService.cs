using ComprehensivePlayrightAuto.Infra.OpenAi;
using Newtonsoft.Json;
using OpenAI.Chat;
using System.Text;
using System.Text.Json;
using static ComprehensivePlayrightAuto.Infra.OpenAi.AiSystemPrompts;

namespace SafeCash.Test.ApiTest.Integration.OpenAi
{
    public class AiSystemService
    {
        #region OpenAiServiceRequest
        AiSystemPrompts aiSystemPrompts = new AiSystemPrompts();

        public async Task<string> OpenAiServiceRequest(string userPrompts, SystemPromptTypeEnum systemPrompt)
        {
            string model = AiSystemPrompts.openAiModel;
            string prePrompt = aiSystemPrompts.GetSystemPrompt(systemPrompt);

            string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new InvalidOperationException("API key is missing from environment variables.");
            string apiResponce = "An error occurred or no response was returned.";
            //string combinedPrompt = $"{prePrompt}\n\n{userPrompts}";
            try
            {
                ChatClient client = new ChatClient(model, apiKey);

                var openAiRequest = new
                {
                    model = model,
                    messages = new[]
                    {
                      new { role = "system", content = prePrompt },
                      new { role = "user", content = userPrompts }
                     }
                };
                string jsonBody = JsonConvert.SerializeObject(openAiRequest, Formatting.Indented);

                UserChatMessage message = new UserChatMessage(jsonBody);
                ChatCompletion completion = await client.CompleteChatAsync(message);

                if (completion?.Content != null && completion.Content.Count > 0)
                {
                    apiResponce = completion.Content[0].Text;

                }
                else
                {
                    apiResponce = "No valid content found in the response.";
                }
                Console.WriteLine("after ai");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return apiResponce;
        }

        #endregion

        #region Grok ai request
        public async Task<string> GrokRequestService(string userMessage, SystemPromptTypeEnum aiRequest)
        {
            string grokUrl = "https://api.x.ai/v1/chat/completions";
            string apiGrokKey = Environment.GetEnvironmentVariable("GROK_API_KEY") ?? throw new InvalidOperationException("API key is missing from environment variables.");

            using (HttpClient client = new HttpClient())
            {
                string aiPrePromptType = aiSystemPrompts.GetSystemPrompt(aiRequest);

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiGrokKey}");

                var requestBody = new
                {
                    messages = new[]
                    {
                new { role = "system", content = aiPrePromptType },
                new { role = "user", content = userMessage }
                },
                    model = "grok-3-mini-fast-latest",
                    stream = false,
                    temperature = 0
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(grokUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // ✅ Extract only the "content" field from the response
                    using JsonDocument doc = JsonDocument.Parse(jsonResponse);

                    if (doc.RootElement.TryGetProperty("choices", out JsonElement choicesArray) &&
                        choicesArray.GetArrayLength() > 0 &&
                        choicesArray[0].TryGetProperty("message", out JsonElement message) &&
                        message.TryGetProperty("content", out JsonElement contentElement))
                    {
                        return contentElement.GetString() ?? "No content available.";
                    }

                    return "Invalid response format.";
                }
                else
                {
                    return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
        }
        #endregion

        #region Deep seek ai request
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<string> DeepSeekResponceAi(string userPrompts, SystemPromptTypeEnum aiRequest)
        {
            string apiUrl = "https://api.deepseek.com/chat/completions"; // Replace with actual API URL
            string bearerToken = "sk-5706d7050b8c4bddb967ba236538d89d"; // Replace with actual token
            string prePrompt = aiSystemPrompts.GetSystemPrompt(aiRequest);
            var requestBody = new
            {
                model = "deepseek-chat",
                messages = new[]
                {
                new { role = "system", content = prePrompt },
                new { role = "user", content = userPrompts }
            },
                stream = false
            };

            string jsonRequest = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json"); // ✅ "Content-Type" set correctly here

            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = content
            };

            request.Headers.Add("Authorization", $"Bearer {bearerToken}"); // ✅ Correct place for Authorization
                                                                           // No need to add "Content-Type" again here

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(jsonResponse);

            string? contentResponce = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return contentResponce;
        }
        #endregion
    }
}
