using ComprehensiveAutomation.Test.UiTest.MobileTest.MobilePageObject;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using SafeCash.Test.ApiTest.InternalApiTest.Buyer;
using System.Text.Json;

namespace ComprehensiveAutomation.Test.UiTest.MobileTest.MobileFlows
{
    public class MobileAiTaskFlow : MobileBaseFlow
    {
        MobileLoginPage mobileDriverLocator;
        public MobileAiTaskFlow(AndroidDriver i_driver) : base(i_driver)
        {
            appiumDriver = i_driver;
            mobileDriverLocator = new MobileLoginPage(appiumDriver);
        }
        List<By> PreviosLocator;
        By? currentAiLocator;
        public async Task <int> HandleAiTaskMission(string userGoalMission)
        {
            var aiService = new AndroidAiService();
            int aiResponceType = (int)aiResponceTypeEnum.ButtonLocator;
            PreviosLocator = new();
            while (aiResponceType == (int)aiResponceTypeEnum.ButtonLocator ||
                   aiResponceType == (int)aiResponceTypeEnum.InputLocator)
            {
                 mobileDriverLocator.WaitForPageToLoad();
                string fullPageSource = GetFullPageSource();
                string jsonAiResponed = "";
                string  listThatWeClickBefore = GetPreviousLocatorsListsInfo(currentAiLocator);
                if (listThatWeClickBefore != null)
                {
                    jsonAiResponed = await aiService.GetAiResponedAsJson(
                      fullPageSource,
                      userGoalMission,
                      "Update: Here is a list of locators that have already been clicked.\n" +
                      $"{listThatWeClickBefore}\n\n" +
                      "Do not return or suggest any of them again");
                }
                else
                {
                    jsonAiResponed = await aiService.GetAiResponedAsJson(fullPageSource, userGoalMission);
                }

                aiResponceType = GetTypeFromJson(jsonAiResponed);

                if (aiResponceType != (int)aiResponceTypeEnum.ButtonLocator &&
                    aiResponceType != (int)aiResponceTypeEnum.InputLocator)
                    break;

                string? inputText = aiResponceType == (int)aiResponceTypeEnum.InputLocator
                    ? GetTextInputValuFromJson(jsonAiResponed)
                    : null;

                currentAiLocator = await RetryUntilElementFound(jsonAiResponed, fullPageSource, userGoalMission, aiService);
                
                if (currentAiLocator == null)
                     return aiResponceType = (int)aiResponceTypeEnum.AiStuckOrUnsure;
                if (aiResponceType == (int)aiResponceTypeEnum.ButtonLocator)
                {
                    mobileDriverLocator.MobileClickElement(currentAiLocator);
                }

                else
                {
                    mobileDriverLocator.MobileInputTextToField(currentAiLocator, inputText);
                }
            }
            return aiResponceType;
        }

        private async Task<By?> RetryUntilElementFound(string jsonResponse, string fullPageSource, string userGoal, AndroidAiService aiService)
        {
            By locator = GetXPathLocatorFromAiJson(jsonResponse);
            By PreviosButtonClickLocator;
            int retry = 0;
            #region Test if the element display on the page
            while (!mobileDriverLocator.IsHavyElementFount(locator) && retry < 3)
            {
                jsonResponse = await aiService.GetAiResponedAsJson(
                    fullPageSource, userGoal, $"The element '{locator}' not on the page, please check again");

                locator = GetXPathLocatorFromAiJson(jsonResponse);
                PreviosButtonClickLocator = locator;
                retry++;
            }
            #endregion
            //Do it so the ai will know the previos locator
            return mobileDriverLocator.IsHavyElementFount(locator) ? locator : null;
        }




        public enum aiResponceTypeEnum
        {
            ButtonLocator = 1,
            InputLocator = 2,
            MissionComplete = 3,
            AiStuckOrUnsure = 0
        }

        public static int GetTypeFromJson(string json)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("type", out JsonElement typeElement) && typeElement.ValueKind == JsonValueKind.Number)
                    {
                        return typeElement.GetInt32();
                    }
                }
            }
            catch (JsonException)
            {
            }

            return -1; 
        }
        public static By GetXPathLocatorFromAiJson(string json)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("xpath", out JsonElement xpathElement) && xpathElement.ValueKind == JsonValueKind.String)
                    {
                        string xpath = xpathElement.GetString();
                        if (!string.IsNullOrWhiteSpace(xpath))
                        {
                            return By.XPath(xpath);
                        }
                    }
                }
            }
            catch (JsonException)
            {
                // Optional: log error
            }

            return null; // You can also throw an exception here if you prefer strict handling
        }

        public static string GetTextInputValuFromJson(string json)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("value", out JsonElement valueElement) &&
                        valueElement.ValueKind == JsonValueKind.String)
                    {
                        return valueElement.GetString();
                    }
                }
            }
            catch (JsonException)
            {
                // Optionally log or handle the parsing error
            }

            return null; // Or throw an exception if required
        }

        public string GetPreviousLocatorsListsInfo(By? currentAiLocator)
        {
            if (currentAiLocator == null)
                return null;

            PreviosLocator.Add(currentAiLocator);

            var lines = PreviosLocator
                .Select((locator, index) => $"Locator {index + 1}: {locator}");

            return string.Join("\n", lines);
        }
    }
}
