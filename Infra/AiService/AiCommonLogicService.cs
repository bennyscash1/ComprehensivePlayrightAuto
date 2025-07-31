using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.Infra.AiService
{
    public class AiCommonLogicService
    {
        public static bool IsAiReturnValidJson(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            string cleaned = CleanJsonMarkdown(input);

            try
            {
                using (JsonDocument.Parse(cleaned))
                {
                    return true;
                }
            }
            catch (JsonException)
            {
                return false;
            }
        }

        /// <summary>
        /// Cleans markdown-style wrapping like ```json ... ``` from the AI response
        /// </summary>
        public static string CleanJsonMarkdown(string input)
        {
            string trimmed = input.Trim();

            if (trimmed.StartsWith("```json"))
                trimmed = trimmed.Substring(7).Trim(); // Remove ```json

            if (trimmed.StartsWith("```"))
                trimmed = trimmed.Substring(3).Trim(); // Remove starting ```

            if (trimmed.EndsWith("```"))
                trimmed = trimmed.Substring(0, trimmed.Length - 3).Trim(); // Remove ending ```
            return trimmed;
        }
        public static string CleanedJsonWithValidation (string input)
        {
            string cleanedJson = CleanJsonMarkdown(input);
            bool isValidJson = IsAiReturnValidJson(cleanedJson);
            Assert.That(isValidJson,"The json that return from the ai not valid json");
            return cleanedJson;
        }
    }
}
