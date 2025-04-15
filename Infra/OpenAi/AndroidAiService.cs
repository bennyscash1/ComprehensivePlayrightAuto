using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using SafeCash.Test.ApiTest.Integration.OpenAi;
using System.Text.RegularExpressions;
namespace SafeCash.Test.ApiTest.InternalApiTest.Buyer
{
    public class AndroidAiService 
    {
         public async Task <string> GetLocatorFromAndroidSourcePage(
             string fullPageSource, string buttonName)
        {          
            OpenAiService openAiService = new OpenAiService();
            string responceLocatorFromAi = await openAiService.OpenAiServiceRequest(
                $"Here is the full app XML source:," +
                $"{fullPageSource}\n\n" +
                $" I need to find the XPath locator for the button or input field for>>: '{buttonName}'\n\n" +
                $"Please return only xpath without any other text",
                OpenAiService.AiRequestType.MobileRequest);
            bool isLocatorValid = IsLocatorIsVald(responceLocatorFromAi);
            if (isLocatorValid)
            {
                return responceLocatorFromAi;
            }
            else
            {
                // If the locator is not valid, you can handle it here
                Console.WriteLine($"Invalid XPath locator: {responceLocatorFromAi}");
                return string.Empty; // or throw an exception, or return a default value
            }   
            //string pattern = @"By\.XPath\(\""(.*?)\""\)";
            //string extractedXPath = "No XPath found in the response";
            //Match match = Regex.Match(respnceOpenAi, pattern);
            //if (match.Success)
            //{
            //    extractedXPath = match.Groups[1].Value;
            //    Console.WriteLine($"Extracted XPath: {extractedXPath}");
            //}
            //else
            //{
            //    Console.WriteLine("No XPath found in the response.");
            //}
        }
        public static bool IsLocatorIsVald(string locator)
        {
            if (string.IsNullOrWhiteSpace(locator))
                return false;

            // Basic check: should start with "/" or "(" for XPath
            if (!locator.TrimStart().StartsWith("/"))
                return false;

            // Try to load it as an XPath using XmlDocument and XPathNavigator
            try
            {
                var dummyXml = "<root><android.widget.ImageButton content-desc=\"5\" /></root>";
                var xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(dummyXml);
                var nav = xmlDoc.CreateNavigator();
                var nodes = nav.Select(locator);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsImagesAreCompareUseAi(string expectedImagePath, string actualImagePath)
        {
            OpenAiService openAiService = new OpenAiService();
            string responseOpenAi = await openAiService.OpenAiServiceRequest(
                $"image 1: {expectedImagePath} " +
                $"image 2: {actualImagePath} " ,
                OpenAiService.AiRequestType.ImagesCompare);

            if (responseOpenAi.Contains("true", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

