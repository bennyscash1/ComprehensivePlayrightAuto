using ComprehensivePlayrightAuto.Infra.OpenAi;
using Newtonsoft.Json;
using OpenAI.Chat;
using Refit;
using static ComprehensivePlayrightAuto.Infra.OpenAi.AiSystemPrompts;

namespace ComprehensivePlayrightAuto.Infra.AiService.AiAgent
{

    public class AiServiceAgent
    {
        #region OpenAiServiceRequest
        AiSystemPrompts aiSystemPrompts = new AiSystemPrompts();
        private static Dictionary<string, List<dynamic>> sessionHistories = new();
        SessionMemoryManager sessionManager = new SessionMemoryManager();


        public async Task<string> OpenAiServiceAgentRequest(string userPrompts, SystemPromptTypeEnum systemPrompt)
        {
            string model = openAiModel;
            string prePrompt = aiSystemPrompts.GetSystemPrompt(systemPrompt);
            string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new InvalidOperationException("API key is missing from environment variables.");
            string apiResponse = "An error occurred or no response was returned.";

            try
            {
                ChatClient client = new ChatClient(model, apiKey);

                // טען את ההיסטוריה של הסשן הפעיל
                var history = sessionManager.GetCurrentSessionHistory();

                // אם זו הודעה ראשונה, נכניס את ההנחיה של המערכת
                if (!history.Any(m => m.role == "system"))
                {
                    sessionManager.AppendToCurrent("system", prePrompt);
                }
                // הוסף את ההודעה של המשתמש
                sessionManager.AppendToCurrent("user", userPrompts);

                // צור בקשה ל־OpenAI
                var openAiRequest = new
                {
                    model = model,
                    messages = history
                };

                string jsonBody = JsonConvert.SerializeObject(openAiRequest, Formatting.Indented);
                UserChatMessage message = new UserChatMessage(jsonBody);
                ChatCompletion completion = await client.CompleteChatAsync(message);

                if (completion?.Content != null && completion.Content.Count > 0)
                {
                    string assistantResponse = completion.Content[0].Text;
                    sessionManager.AppendToCurrent("assistant", assistantResponse.Trim());
                    apiResponse = assistantResponse;
                }
                else
                {
                    apiResponse = "No valid content found in the response.";
                }

                Console.WriteLine("after AI");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return apiResponse;
        }

    }
    #endregion

}
