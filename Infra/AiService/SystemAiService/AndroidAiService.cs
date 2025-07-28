using ComprehensivePlayrightAuto.Infra.OpenAi;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using SafeCash.Test.ApiTest.Integration.OpenAi;
using System.Text.Json;
using System.Text.RegularExpressions;
namespace ComprehensivePlayrightAuto.Infra.AiService.SystemAiService
{
    public class AndroidAiService
    {
        #region Get Locator from AI for click locator
        public async Task<string> GetAndroidLocatorFromUserTextInput(
            string fullPageSource, string userInputView)
        {
            AiSystemService openAiService = new AiSystemService();
            string responceLocatorFromAi = await openAiService.OpenAiServiceRequest(
                $"Here is the full app XML source:," +
                $"{fullPageSource}\n\n" +
                $" I need to find the XPath locator for the button or input field for the next line>>: '\n" +
                $"{userInputView}'\n\n" +
                $"Please return only xpath without any other text",
                AiSystemPrompts.SystemPromptTypeEnum.MobileTextInpueRequest);
            bool isLocatorValid = AndroidAiService.isLocatorValid(responceLocatorFromAi);
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
        #endregion
        public async Task<string> GetAndroidLocatorFromUserXyCordinate(
            string fullPageSource, int x, int y, string screenSize)
        {
            AiSystemService openAiService = new AiSystemService();
            string responceLocatorFromAi = await openAiService.GrokRequestService(
                $"Here is the full app XML source:," +
                $"{fullPageSource}\n\n" +
                $" I need to find the XPath locator for the button or input field according to the X and Y cordinate>>: '\n" +
                $"the X cordinate:{x}', the Y cordinate: {y}\n\n" +
                $"Please return only xpath without any other text",
                AiSystemPrompts.SystemPromptTypeEnum.MobileXyCordinateRequest);
            bool isLocatorValid = AndroidAiService.isLocatorValid(responceLocatorFromAi);
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
        public static bool isLocatorValid(string locator)
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
            AiSystemService openAiService = new AiSystemService();
            string responseOpenAi = await openAiService.OpenAiServiceRequest(
                $"image 1: {expectedImagePath} " +
                $"image 2: {actualImagePath} ",
                AiSystemPrompts.SystemPromptTypeEnum.ImagesCompare);

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
           string fullPageSource, string userEndGoalMission, string userUpdateOnFailedScenario ="")
        {
            AiSystemService openAiService = new AiSystemService();
            string responceLocatorFromAi = await openAiService.GrokRequestService(
                $"XML:\n{fullPageSource}\n\n" +
                $"The user Goal:\n" +

                $"{userEndGoalMission}\n\n" +

                $"{userUpdateOnFailedScenario}",
                 AiSystemPrompts.SystemPromptTypeEnum.MobileSystemPromptMissionTask);
            
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
        #endregion

    }
}


