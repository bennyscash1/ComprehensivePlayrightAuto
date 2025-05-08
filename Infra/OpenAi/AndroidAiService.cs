using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using SafeCash.Test.ApiTest.Integration.OpenAi;
using System.Text.Json;
using System.Text.RegularExpressions;
namespace SafeCash.Test.ApiTest.InternalApiTest.Buyer
{
    public class AndroidAiService
    {
        public async Task<string> GetAndroidLocatorFromUserTextInput(
            string fullPageSource, string userInputView)
        {
            OpenAiService openAiService = new OpenAiService();
            string responceLocatorFromAi = await openAiService.OpenAiServiceRequest(
                $"Here is the full app XML source:," +
                $"{fullPageSource}\n\n" +
                $" I need to find the XPath locator for the button or input field for the next line>>: '\n" +
                $"{userInputView}'\n\n" +
                $"Please return only xpath without any other text",
                OpenAiService.SystemPromptTypeEnum.MobileTextInpueRequest);
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
        }
        public async Task<string> GetAndroidLocatorFromUserXyCordinate(
            string fullPageSource, int x, int y, string screenSize)
        {
            OpenAiService openAiService = new OpenAiService();
            string responceLocatorFromAi = await openAiService.OpenAiServiceRequest(
                $"Here is the full app XML source:," +
                $"{fullPageSource}\n\n" +
                $" I need to find the XPath locator for the button or input field according to the X and Y cordinate>>: '\n" +
                $"the X cordinate:{x}', the Y cordinate: {y}\n\n" +
                $"Please return only xpath without any other text",
                OpenAiService.SystemPromptTypeEnum.MobileXyCordinateRequest);
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
                $"image 2: {actualImagePath} ",
                OpenAiService.SystemPromptTypeEnum.ImagesCompare);

            if (responseOpenAi.Contains("true", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Ai service for tasks 
        public async Task<string> GetAiResponedAsJson(
           string fullPageSource, string userEndGoalMission)
        {
            OpenAiService openAiService = new OpenAiService();
            string responceLocatorFromAi = await openAiService.OpenAiServiceRequest(
                $"Here is the full app XML source:," +
                $"{fullPageSource}\n\n" +
                $" I need you to promot me to the next task according to this user task>>: '\n" +
                $"{userEndGoalMission}'\n\n" +
                $"Please return me valid json accordign to the system prompt",
                OpenAiService.SystemPromptTypeEnum.MobileSystemPromptMissionTask);
            bool isLocatorValid = isAiReturnValidJson(responceLocatorFromAi);
            if (isLocatorValid)
            {
                return responceLocatorFromAi;
            }
            else
            {
                Assert.That(isLocatorValid, Is.True,
                    $"THe ai responed invalid json: {responceLocatorFromAi}");
                return string.Empty; 
            }
        }
        public static bool isAiReturnValidJson(string input)
        {
            try
            {
                using (JsonDocument.Parse(input))
                {
                    return true;
                }
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}
    #endregion


