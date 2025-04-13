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
            "Given a mobile XML element dump, return only the XPath of the button or element best matching the target.\n\n" +
            "Rules:\n" +
            "- Match by content-desc, text, or resource-id.\n" +
            "- One match → return XPath only. Multiple → return count. No match → return 0.\n" +
            "- Allow partial matches (substring, typo, etc.).\n" +
            "- Recognize semantic terms (e.g., '3 dots', 'plus icon', 'person icon'):\n" +
            "   • 'plus icon' → content-desc with 'Add'/'Create' or resource-id with 'fab'\n" +
            "   • 'search field' → EditText or 'Search'\n" +
            "   • 'person icon' → profile/avatar/user\n" +
            "   • 'menu button' or '3 dots' → 'More options' or 'Customize and control'\n" +
            "- Layouts (FrameLayout, LinearLayout) are valid if clickable and labeled.\n\n" +
            "Examples:\n" +
            "<android.widget.ImageButton content-desc=\"5\" />\n" +
            "Target: 5\n" +
            "Expected: //android.widget.ImageButton[@content-desc=\"5\"]\n\n" +
            "<android.widget.ImageButton content-desc=\"Customize and control Google Chrome\" />\n" +
            "Target: 3 dots\n" +
            "Expected: //android.widget.ImageButton[@content-desc=\"Customize and control Google Chrome\"]\n\n" +
            "<android.widget.ImageView resource-id=\"com.app:id/profile_icon\" />\n" +
            "Target: person icon\n" +
            "Expected: //android.widget.ImageView[@resource-id=\"com.app:id/profile_icon\"]\n\n" +
            "<android.widget.Button content-desc=\"Add city\" resource-id=\"com.google.android.deskclock:id/fab\" />\n" +
            "Target: plus icon\n" +
            "Expected: //android.widget.Button[@content-desc=\"Add city\"]\n\n" +
            "<android.widget.FrameLayout content-desc=\"Alarm\" resource-id=\"com.google.android.deskclock:id/tab_menu_alarm\" />\n" +
            "Target: Alarm\n" +
            "Expected: //android.widget.FrameLayout[@content-desc=\"Alarm\"]\n\n" +
            "Now analyze the following:";







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
