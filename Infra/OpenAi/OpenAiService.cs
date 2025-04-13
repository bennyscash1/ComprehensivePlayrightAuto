using Newtonsoft.Json;
using OpenAI.Chat;

namespace SafeCash.Test.ApiTest.Integration.OpenAi
{
    public class OpenAiService
    {
        public enum AiRequestType
        {
            ApiRequest,
            MobileRequest,
            ImagesCompare
        }

        string mobilePrePrompt = "You are an automation expert.\n" +
                "I will give you a mobile XML element dump.\n" +
                "Return only the XPath locator for the button matching the given target.\n\n" +
                "Rules:\n" +
                "- If exactly one match is found: return the XPath only (no explanation).\n" +
                "- If multiple matches are found: return the number of matches.\n" +
                "- If no match is found, but the target is a known alias (e.g., 'x' or '*' for 'multiply'), use the real label and return the matching XPath.\n" +
                "- If nothing matches at all: return 0.\n\n" +
                "Example XML:\n<android.widget.ImageButton content-desc=\"5\" />\n" +
                "Target: 5\n" +
                "Expected: //android.widget.ImageButton[@content-desc=\"5\"]\n";


        public async Task<string> OpenAiServiceRequest(string userPrompts, AiRequestType aiRequest)
        {
            OpenAiData openAiData = new OpenAiData();
            string model = OpenAiData.model;
            string prePrompt = GetStockPrePromptPrompts(aiRequest);

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

        public string GetStockPrePromptPrompts(AiRequestType aiRequest)
        {
            string prePrompt;
            switch (aiRequest)
            {

                case AiRequestType.MobileRequest:
                    prePrompt = mobilePrePrompt;
                    break;
/*                case AiPrePromptType.DataBaseAnalyst:
                    prePrompt = DataBaseAnalyst;
                    break;
                case AiPrePromptType.GetStockCompanysPrompts:
                    prePrompt = GetStockCompanysPrompts;
                    break;*/
                default:
                    prePrompt = "You are an AI assistant."; // Default fallback
                    break;
            }
            return prePrompt;
        }


    }
}
