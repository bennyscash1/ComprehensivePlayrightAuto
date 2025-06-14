using ComprehensiveAutomation.Test.PageObject;
using ComprehensivePlayrightAuto.Infra.OpenAi;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.WebTest.PageObject.WebAiPages
{
    public class WebAiPages : BasePages
    {
        public IPage pDriver;
        public WebAiPages(IPage _page) : base(_page)
        {
            pDriver = _page;
        }
        public async Task<string> GetXpathFromFullDom(string userAction)
        {
            string fullPageDom = await pDriver
                .EvaluateAsync<string>("() => document.documentElement.outerHTML");
            bool isLocatorFound = false;
            int partLength = fullPageDom.Length / 4;
            var domParts = new[]
            {
                    fullPageDom.Substring(0, partLength),
                    fullPageDom.Substring(partLength, partLength),
                    fullPageDom.Substring(partLength * 2, partLength),
                    fullPageDom.Substring(partLength * 3)
            };
            foreach (var part in domParts)
            {
                if (IsGoalInDom(part, userAction))
                {
                    string result = await new WebAiService().GetWebSingleLocatorFromUrl(part, userAction);
                    if (IsAiReturnElement(result))
                        return result;
                }
            }

            return "part1";
        }
        public async Task<bool> isLocatorFoundForDom(string domParElemnt, string userRequest)
        {
            WebAiService webAiService = new WebAiService();
            string aiResponce = "";
            if (IsGoalInDom(domParElemnt, userRequest))
            {
                aiResponce =
                    await webAiService.GetWebSingleLocatorFromUrl(domParElemnt, userRequest);
            }

            return IsAiReturnElement(aiResponce);
        }

        public static bool IsGoalInDom(string dom, string goal)
        {
            // הוצא רק מילים "חשובות" מהבקשה של המשתמש
            string[] keywords = goal
                .Replace("'", "")
                .Replace("\"", "")
                .Replace(".", "")
                .Replace(",", "")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // בדוק אם אחת מהמילים מופיעה ב־DOM
            foreach (string word in keywords)
            {
                if (dom.Contains(word, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }


        public static bool IsAiReturnElement(string json)
        {
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("elementFound").GetBoolean();
        }
        public  async Task AiLocatorClickOnButton(string aiXpath)
        {
            await ClickAsync(aiXpath);

        }
    }
}
