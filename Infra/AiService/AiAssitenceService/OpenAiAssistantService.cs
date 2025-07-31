using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.Infra.AiService.AiAssitenceService
{

        public class OpenAiAssistantService
        {
            private readonly string _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            private readonly string _assistantId = "asst_PyPrh0ASUJpL5sIZImVkLdX6";
            private readonly HttpClient _httpClient;

            public OpenAiAssistantService()
            {
                _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
                _httpClient.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v2"); // 🔥 חובה!

        }

        // שמור את threadId במסד נתונים לפי user/session
        private static string _threadId = "";

        private string _lastUserPrompt = null;

        public async Task<string> SendMessageToAssistant(string userPrompt)
        {
            if (string.IsNullOrEmpty(_threadId))
            {
                _threadId = await CreateThread();
            }

            if (userPrompt == _lastUserPrompt)
            {
                // לא שולחים שוב אותה בקשה
                return "{\"type\": 3}";
            }

            _lastUserPrompt = userPrompt;

            await AddMessageToThread(_threadId, userPrompt);
            string runId = await RunAssistant(_threadId);

            return await WaitForRunAndGetResponse(_threadId, runId);
        }


        private async Task<string> CreateThread()
            {
                var response = await _httpClient.PostAsync("https://api.openai.com/v1/threads", new StringContent("{}", Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(content);
                return result.id;
            }

            private async Task AddMessageToThread(string threadId, string userPrompt)
            {
                var payload = new
                {
                    role = "user",
                    content = userPrompt
                };

                var json = JsonConvert.SerializeObject(payload);
                await _httpClient.PostAsync($"https://api.openai.com/v1/threads/{threadId}/messages", new StringContent(json, Encoding.UTF8, "application/json"));
            }

            private async Task<string> RunAssistant(string threadId)
            {
                var payload = new
                {
                    assistant_id = _assistantId
                };

                var json = JsonConvert.SerializeObject(payload);
                var response = await _httpClient.PostAsync($"https://api.openai.com/v1/threads/{threadId}/runs", new StringContent(json, Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(content);
                return result.id;
            }

            private async Task<string> WaitForRunAndGetResponse(string threadId, string runId)
            {
                string status = "in_progress";
                int maxTries = 20;

                while (status == "in_progress" && maxTries-- > 0)
                {
                    await Task.Delay(1000);
                    var response = await _httpClient.GetAsync($"https://api.openai.com/v1/threads/{threadId}/runs/{runId}");
                    var content = await response.Content.ReadAsStringAsync();
                    string validJson = AiCommonLogicService.CleanedJsonWithValidation(content);
                    dynamic result = JsonConvert.DeserializeObject(validJson);
                    status = result.status;
                }

                // לאחר סיום, קבל את ההודעה האחרונה של העוזר
                var messagesResponse = await _httpClient.GetAsync($"https://api.openai.com/v1/threads/{threadId}/messages");
                var messagesContent = await messagesResponse.Content.ReadAsStringAsync();
                dynamic messages = JsonConvert.DeserializeObject(messagesContent);

                // חפש את ההודעה האחרונה מהעוזר
                foreach (var message in ((IEnumerable<dynamic>)messages.data))
                {
                    if (message.role == "assistant")
                    {
                        return message.content[0].text.value.ToString();
                    }
                }

                return "No response from assistant.";
            }
        

    }
}
