using Newtonsoft.Json;
using OpenAI.Chat;

namespace SafeCash.Test.ApiTest.Integration.OpenAi
{
    public class OpenAiService
    {
        public static string openAiModel = "gpt-4o-mini";
        public enum AiRequestType
        {
            ApiRequest,
            MobileTextInpueRequest,
            MobileXyCordinateRequest,
            ImagesCompare
        }
        string mobilePrePrompt = "You are an automation expert.\n" +
            "Given a mobile XML element dump, return only the XPath of the button or element best matching the target.\n\n" +
            "Rules:\n" +
            "- Match by content-desc, text, or resource-id.\n" +
            "- If provided, also consider color or visual clues (e.g., \"Blue icon\") when content-desc, text, or resource-id are insufficient.\n" +
            "- One match → return XPath only. Multiple → return count. No match → return 0.\n" +
            "- Allow partial matches (substring, typo, etc.).\n" +
                "- Recognize semantic and visual terms (e.g., '3 dots', 'plus icon', 'person icon', color descriptions):\n" +
            "   • 'plus icon' → content-desc with 'Add'/'Create' or resource-id with 'fab'\n" +
            "   • 'search field' → EditText or 'Search'\n" +
            "   • 'person icon' → profile/avatar/user\n" +
            "   • 'menu button' or '3 dots' → 'More options' or 'Customize and control'\n" +
            "   • Color terms (e.g., \"Blue icon\") → prioritize clickable buttons (ImageButton, Button, ImageView) without text\n" +
            "     especially when stated clearly like FAB buttons or visually distinct elements\n" +
            "   • Also infer visual roles or icon types from user terms (e.g., 'record icon', 'dialpad icon', '10 dots') even if no exact match in content-desc/text\n" +
            "- Layouts (FrameLayout, LinearLayout) are valid if clickable and labeled.\n\n" +
            "Examples:\n" +
            "<android.widget.ImageButton content-desc=\"5\" />\n" +
            "Target: 5\n" +
            "Expected: //android.widget.ImageButton[@content-desc=\"5\"]\n\n" +
            "<android.widget.ImageView resource-id=\"com.app:id/profile_icon\" />\n" +
            "Target: person icon\n" +
            "Expected: //android.widget.ImageView[@resource-id=\"com.app:id/profile_icon\"]\n\n" +
            "<android.widget.Button content-desc=\"Add city\" resource-id=\"com.google.android.deskclock:id/fab\" />\n" +
            "Target: plus icon\n" +
            "Expected: //android.widget.Button[@content-desc=\"Add city\"]\n\n" +
            "<android.widget.ImageButton content-desc=\"key pad\" resource-id=\"com.google.android.dialer:id/dialpad_fab\" clickable=\"true\" />\n" +
            "Target: Blue icon with 10 dots\n" +
            "Expected: //android.widget.ImageButton[@resource-id=\"com.google.android.dialer:id/dialpad_fab\"]\n\n" +
            "Important: Return ONLY the raw XPath string (no markdown, no quotes, no formatting).\n" +
            "Now analyze the following:";

        string mobilePrePromptCordinateXy = "You are an automation expert.\n" +
            "Your task is:\n" +
            "Given:\n" +
            "- A full XML dump of a mobile screen hierarchy.\n" +
            "- The screen width and height.\n" +
            "- A pair of coordinates (X and Y).\n\n" +
            "Rules:\n" +
            "1. Identify the single UI element in the XML whose bounds ([left,top][right,bottom]) fully contain the given X and Y coordinates.\n" +
            "2. If multiple elements overlap, select the deepest (most nested) element.\n" +
            "3. Return only the XPath expression to locate that specific element.\n" +
            "4. XPath must be precise and use resource-id or content-desc if available.\n" +
            "   - Prefer resource-id when present.\n" +
            "   - If resource-id is missing, use content-desc.\n" +
            "   - If both are missing, use full tag and index path.\n" +
            "5. If no matching element is found, return exactly the number 0 (zero) and nothing else.\n" +
            "6. Never explain, apologize, or add any extra text.\n\n" +
            "Examples of valid responses:\n" +
            "- //android.widget.ImageButton[@resource-id=\"com.example:id/button\"]\n" +
            "- //android.widget.TextView[@content-desc=\"Submit\"]\n" +
            "- 0\n\n" +
            "Important:\n" +
            "- Screen coordinates are based on the screen size.\n" +
            "- Bounds are given as [left,top][right,bottom] relative to the screen.\n" +
            "- Coordinates on the exact border (equal to left or top but less than right or bottom) are considered inside the bounds.\n" +
            "- Always return only one XPath or 0.\n" +
            "7. Coordinates must be strictly inside the bounds:\n   " +
            "- X must be >= left and < right.\n  " +
            " - Y must be >= top and < bottom.\n   " +
            "- No partial matches are allowed.\n   " +
            "- Ignore elements where X and Y are not fully contained inside bounds.\n";

        public async Task<string> OpenAiServiceRequest(string userPrompts, AiRequestType aiRequest)
        {
            string model = openAiModel;
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

                case AiRequestType.MobileTextInpueRequest:
                    prePrompt = mobilePrePrompt;
                    break;
                case AiRequestType.MobileXyCordinateRequest:
                    prePrompt = mobilePrePromptCordinateXy;
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
