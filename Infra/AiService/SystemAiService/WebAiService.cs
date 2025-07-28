using ComprehensivePlayrightAuto.Infra.AiService.AiAgent;
using ComprehensivePlayrightAuto.Infra.OpenAi;
using SafeCash.Test.ApiTest.Integration.OpenAi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.Infra.AiService.SystemAiService
{
    public class WebAiService
    {
        public async Task<string> GetWebActionLocator(
            string webDomString, string userRequest)
        {
            AiServiceAgent openAiService = new AiServiceAgent();

            string responseLocatorFromAi = await openAiService.OpenAiServiceAgentRequest(
                $"Here is the full url: {webDomString}\n\n" +
                $"I need to find the Xpath selector for the element that matches: '{userRequest}'\n\n" +
                $"Please return only the XPATH selector without any other text",
                AiSystemPrompts.SystemPromptTypeEnum.WebSystemActionPrompt);

            return responseLocatorFromAi;
        }

    }
}
