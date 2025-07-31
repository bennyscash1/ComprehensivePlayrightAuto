using ComprehensiveAutomation.Test.PageObject;
using ComprehensiveAutomation.Test.UiTest.MobileTest.MobileFlows;
using ComprehensivePlayrightAuto.Infra.AiService;
using ComprehensivePlayrightAuto.Infra.AiService.AiAgent;
using ComprehensivePlayrightAuto.Infra.AiService.AiAssitenceService;
using ComprehensivePlayrightAuto.Infra.AiService.SystemAiService;
using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static ComprehensivePlayrightAuto.Infra.OpenAi.AiSystemPrompts;

namespace ComprehensivePlayrightAuto.WebTest.PageObject.WebAiPages
{
    public class WebAiTaskPages : BasePages
    {
        //This class is to exstract the dom or part of the dom- send to ai and return xpath.
        public IPage pDriver;
        public WebAiTaskPages(IPage _page) : base(_page)
        {
            pDriver = _page;
        }
        public async Task<int> GetXpathFromDomAccordingToUserTaks(string userRequest)
        {

            string fullPageDom = await pDriver
                .EvaluateAsync<string>("() => document.documentElement.outerHTML");
            int typResponceTypeNumber = -1;
            while (typResponceTypeNumber!= (int)aiResponceTypeEnumWeb.MissionComplete )
            {
                OpenAiAssistantService aiAssitenceService = new OpenAiAssistantService();
                string userFullRequest = $"This is the UserGoal task request: \n " +
                    $"'{userRequest}'\n\n" +
                    $"The full dom html is {fullPageDom}\n\n" +
                    $"PLESE RETURN ONLY TYPE JSON SAME AS SYSTEM PROMPT!";

                string aiResponceJson = await aiAssitenceService
                    .SendMessageToAssistant(userFullRequest);
                string xpathLocator = "";
                string inputValue = "";
                (typResponceTypeNumber, xpathLocator, inputValue) = ExtractSelectorFromJson(aiResponceJson); 

                if (typResponceTypeNumber != (int)aiResponceTypeEnumWeb.MissionComplete ||
                     typResponceTypeNumber != (int)aiResponceTypeEnumWeb.AiStuckOrUnsure)
                {
                    //Check if the ai return a locator or input -------------
                    WebAiActionPages webAiActionPages = new WebAiActionPages(pDriver);

                    if (typResponceTypeNumber == (int)aiResponceTypeEnumWeb.ButtonLocator)
                    {
                        await webAiActionPages.AiLocatorClickOnButton(xpathLocator);
                    }
                    if (typResponceTypeNumber == (int)aiResponceTypeEnumWeb.InputLocator)
                    {
                        await webAiActionPages.AiLocatorInputAction(xpathLocator, inputValue);

                    }
                    if (typResponceTypeNumber == (int)aiResponceTypeEnumWeb.AiStuckOrUnsure)
                    {
                        break;
                    }
               }
      
            }

            return typResponceTypeNumber;
        }

        public enum aiResponceTypeEnumWeb
        {
            ButtonLocator = 1,
            InputLocator = 2,
            MissionComplete = 3,
            AiStuckOrUnsure = 0
        }

        #region get data from json
        public (int selectorType, string selector, string userInputValue) ExtractSelectorFromJson(string json)
        {
            string cleanJson = AiCommonLogicService.CleanJsonMarkdown(json);
            bool isValidJson = AiCommonLogicService.IsAiReturnValidJson(json);
            string selector = null;
            string value = null;
            int type = -1;

            if (isValidJson)
            {
                using var doc = JsonDocument.Parse(cleanJson);
                var root = doc.RootElement;


                if (root.TryGetProperty("xpath", out JsonElement selectorElement))
                {
                    selector = selectorElement.GetString();
                }

                if (root.TryGetProperty("value", out JsonElement valueElement))
                {
                    value = valueElement.GetString();
                }

                if (root.TryGetProperty("type", out JsonElement typeElement))
                {
                    // בטוח יותר להשתמש בזה כדי להמיר ל-int
                    if (typeElement.TryGetInt32(out int parsedType))
                    {
                        type = parsedType;
                    }
                }
                return (type, selector, value);
            }

            else
            {
                Assert.That(isValidJson, "AI return invalid json");
                return (type, selector, value);
            }
        }

        #endregion
    }
}
