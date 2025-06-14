using SafeCash.Test.ApiTest.Integration.OpenAi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.Infra.OpenAi
{
    public class WebAiService
    {
        public async Task<string> GetWebSingleLocatorFromUrl(
            string urlWeb, string userRequest)
        {
            OpenAiService openAiService = new OpenAiService();
            string responseLocatorFromAi = await openAiService.OpenAiServiceRequest(
                $"Here is the full url: {urlWeb}\n\n" +
                $"I need to find the Xpath selector for the element that matches: '{userRequest}'\n\n" +
                $"Please return only the XPATH selector without any other text",
                OpenAiService.SystemPromptTypeEnum.WebSystemPrompt);
            return responseLocatorFromAi;
        }

    }
}
