using ComprehensiveAutomation.Test.PageObject;
using ComprehensivePlayrightAuto.Infra.AiService.SystemAiService;
using HtmlAgilityPack;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace ComprehensivePlayrightAuto.WebTest.PageObject.WebAiPages
{
    public class WebAiActionPages : BasePages
    {
        public IPage pDriver;
        public WebAiActionPages(IPage _page) : base(_page)
        {
            pDriver = _page;
        }
        public async Task<string> GetXpathFromFullDom(string userAction)
        {
            string fullPageDom = await pDriver
                .EvaluateAsync<string>("() => document.documentElement.outerHTML");



            bool isLocatorFound = false;
            var domParts = SplitDomStringIntoParts(fullPageDom, 2);
            foreach (var parDom in domParts)
            {
                if (IsGoalOnthePartDom(parDom, userAction))
                {                       
                    string aiFullResolt = await new WebAiService()
                        .GetWebActionLocator(parDom, userAction);
                    if (IsAiReturnElement(aiFullResolt))
                    {
                        string xpathLocator = ExtractXPathFromJson(aiFullResolt);

                        return xpathLocator;
                    }                       
                                                                
                }
            }
            return "Ai not return any locator";
        }
        public async Task<bool> isLocatorFoundForDom(string domParElemnt, string userRequest)
        {
            WebAiService webAiService = new WebAiService();
            string aiResponce = "";
            if (IsGoalOnthePartDom(domParElemnt, userRequest))
            {
                aiResponce =
                    await webAiService.GetWebActionLocator(domParElemnt, userRequest);
            }

            return IsAiReturnElement(aiResponce);
        }

        public static bool IsGoalOnthePartDom(string dom, string goal)
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
        public static List<string> SplitDomStringIntoParts(string input, int numberOfParts)
        {
            var parts = new List<string>();
            int partLength = input.Length / numberOfParts;
            int remainder = input.Length % numberOfParts;

            int currentIndex = 0;

            for (int i = 0; i < numberOfParts; i++)
            {
                int currentPartLength = partLength + (i < remainder ? 1 : 0); // distribute remainder
                parts.Add(input.Substring(currentIndex, currentPartLength));
                currentIndex += currentPartLength;
            }

            return parts;
        }
        public static bool IsValidXPath(string xpath)
        {
            try
            {
                // Create dummy document to validate XPath
                var xmlDoc = new XmlDocument();
                var nav = xmlDoc.CreateNavigator();

                // Try compiling the XPath expression
                nav.Compile(xpath);

                return true;
            }
            catch
            {
                return false;
            }
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
        public async Task AiLocatorInputAction(string aiXpath, string input)
        {
            await FuillTextAsync(aiXpath, input);

        }
        public static string ExtractXPathFromDom(string dom, string goal)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(dom);

            // מילות מפתח
            string[] keywords = goal
                .Replace("'", "")
                .Replace("\"", "")
                .Replace(".", "")
                .Replace(",", "")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // עבור על כל האלמנטים ובדוק אם הטקסט מכיל מילת מפתח
            foreach (var node in doc.DocumentNode.Descendants().Where(n => n.NodeType == 
            HtmlNodeType.Element && !string.IsNullOrWhiteSpace(n.InnerText)))
            {
                foreach (var word in keywords)
                {
                    if (node.InnerText.Contains(word, StringComparison.OrdinalIgnoreCase))
                    {
                        return node.XPath;
                    }
                }
            }

            return "Element not being found";
        }
        public static string ExtractXPathFromJson(string json)
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("xpathElement", out JsonElement xpathElement))
            {
                return xpathElement.GetString();
            }

            return null;
        }

        public static string GetBestMatchingXPath(string dom, string goal)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(dom);

            // Step 1: Extract keywords from the goal
            string[] keywords = goal
                .Replace("'", "")
                .Replace("\"", "")
                .Replace(".", "")
                .Replace(",", "")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Step 2: Search for nodes with inner text matching most keywords
            var candidates = doc.DocumentNode
                .Descendants()
                .Where(n => n.NodeType == HtmlNodeType.Element && !string.IsNullOrWhiteSpace(n.InnerText))
                .Select(n => new
                {
                    Node = n,
                    Score = keywords.Count(k => n.InnerText.Contains(k, StringComparison.OrdinalIgnoreCase))
                })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            // Step 3: Return XPath of the best match (if any)
            return candidates?.Node.XPath;
        }
    }
}
