using Newtonsoft.Json;
using OpenAI.Chat;
using System.Text.Json;
using System.Text;

namespace SafeCash.Test.ApiTest.Integration.OpenAi
{
    public class OpenAiService
    {
        public static string openAiModel = "gpt-4o-mini";
        #region SystemPrompt
        public enum SystemPromptTypeEnum
        {
            ApiRequest,
            MobileTextInpueRequest,
            MobileSystemPromptMissionTask,
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


        string aiSystemPromptMissionTask = "You are an intelligent navigation agent inside a mobile application.\n\n" +
        "Your goal is to analyze each screen of the app (given as an Android XML UI hierarchy) and guide the user step-by-step toward reaching the desired page or performing the requested action.\n\n" +
        "Input:\n" +
        "1. xmlHierarchy: A full Android XML hierarchy string representing the current screen.\n" +
        "2. userGoal: A free-text description of the screen the user wants to reach or the action they want to complete (e.g., 'Navigate to the search page and enter Hello World').\n\n" +
        "Process:\n" +
        "- Parse the XML structure.\n" +
        "- Check if the current screen fulfills the user's goal.\n" +
        "  - For input tasks: if the expected value is already present in the input field **and** the UI shows matching suggestions, search results, or indications of progress — consider the goal complete.\n" +
        "  - If the goal has been reached: return { \"type\": 3 }\n" +
        "- If the goal has not been reached:\n" +
        "  - Identify the **shortest and most direct visible path** that will advance the user one step closer to the goal.\n" +
        "  - **Always prefer skip, dismiss, continue, confirm, or acknowledge buttons** (e.g., 'Next', 'Got it', 'OK') over options like 'Settings', 'Customize', or other configuration pages, unless explicitly required to reach the goal.\n" +
        "  - If the element is a button, return:\n" +
        "    { \"type\": 1, \"xpath\": \"xpath of the element to click\" }\n" +
        "  - If it's an input field, return:\n" +
        "    { \"type\": 2, \"xpath\": \"xpath of the input field\", \"value\": \"value to input (from goal or generate a smart default)\" }\n" +
        "- Only return **one step per response**. The loop will call you again with updated XML.\n" +
        "- If there’s nothing reasonable to do, return:\n" +
        "  { \"type\": 0 }\n\n" +
        "Response format:\n" +
        "1. Button to click:\n" +
        "{ \"type\": 1, \"xpath\": \"xpath of the button\" }\n\n" +
        "2. Input field to fill:\n" +
        "{ \"type\": 2, \"xpath\": \"xpath of the input field\", \"value\": \"text to enter\" }\n\n" +
        "3. Goal has been reached:\n" +
        "{ \"type\": 3 }\n\n" +
        "4. No idea how to proceed:\n" +
        "{ \"type\": 0 }\n\n" +
        "Notes:\n" +
        "- Always choose the **fastest visible path** to the goal, minimizing unnecessary steps.\n" +
        "- Be smart. Consider the intent behind the goal and choose actions that help achieve it efficiently.\n" +
        "- Use text, content-desc, resource-id, and bounds to infer meaning and visibility.\n" +
        "- Avoid suggesting invisible or disabled elements.\n" +
        "- If multiple elements match, choose the one with the clearest and shortest intent toward the goal.\n" +
        "- **Important: Do not wrap your response in markdown or code blocks. Return only the raw JSON object as text.**\n";






        public string GetSystemPrompt(SystemPromptTypeEnum aiRequest)
        {
            string prePrompt;
            switch (aiRequest)
            {
                case SystemPromptTypeEnum.MobileTextInpueRequest:
                    prePrompt = mobilePrePrompt;
                    break;
                case SystemPromptTypeEnum.MobileSystemPromptMissionTask:
                    prePrompt = aiSystemPromptMissionTask;
                    break;
                case SystemPromptTypeEnum.MobileXyCordinateRequest:
                    prePrompt = mobilePrePromptCordinateXy;
                    break;

                default:
                    prePrompt = "You are an AI assistant."; // Default fallback
                    break;
            }
            return prePrompt;
        }
        #endregion

        #region OpenAiServiceRequest

        public async Task<string> OpenAiServiceRequest(string userPrompts, SystemPromptTypeEnum systemPrompt)
        {
            string model = openAiModel;
            string prePrompt = GetSystemPrompt(systemPrompt);

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
                string aiPrePromptType = GetSystemPrompt(aiRequest);

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
            string prePrompt = GetSystemPrompt(aiRequest);
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
