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
        public async Task HandleAiResponce(string userGoalMission)
        {
            #region Get full page and send to AI
            var aiService = new AndroidAiService();     
            #endregion

            #region Habdle the json response according to the type
            int aiResponceType = (int)aiResponceTypeEnum.ButtonLocator;

            while (aiResponceType == (int)aiResponceTypeEnum.ButtonLocator ||
                  aiResponceType == (int)aiResponceTypeEnum.InputLocator)
            {
                mobileDriverLocator.WaitForPageToLoad();
                string fullPageSource = GetFullPageSource();
                string jsonResponce = await aiService
                    .GetAiResponedAsJson(fullPageSource, userGoalMission);
                aiResponceType = GetTypeFromJson(jsonResponce);
                if (GetTypeFromJson(jsonResponce) == (int)aiResponceTypeEnum.ButtonLocator)
                {
                    string responceLocatorFromAi = GetXPathFromAiJson(jsonResponce);
                    By aiJsonResponce = By.XPath(responceLocatorFromAi);
                    mobileDriverLocator.MobileClickElement(aiJsonResponce);
                }
                if (GetTypeFromJson(jsonResponce) == (int)aiResponceTypeEnum.InputLocator)
                {
                    string responceLocatorFromAi = GetXPathFromAiJson(jsonResponce);
                    string inputTextFromJson = GetTextInputValuFromJson(jsonResponce);
                    By aiJsonResponce = By.XPath(responceLocatorFromAi);
                    mobileDriverLocator.MobileInputTextToField(aiJsonResponce, inputTextFromJson);
                }
            }
            #endregion
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
                // handle invalid JSON if needed
            }

            return -1; // or throw exception or use nullable int if preferred
        }
        public static string GetXPathFromAiJson(string json)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("xpath", out JsonElement xpathElement) && xpathElement.ValueKind == JsonValueKind.String)
                    {
                        return xpathElement.GetString();
                    }
                }
            }
            catch (JsonException)
            {
                // handle invalid JSON if needed
            }

            return null; // or throw exception if preferred
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
    }
}
