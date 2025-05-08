using ComprehensiveAutomation.Test.UiTest.MobileTest.MobilePageObject;
using ComprehensivePlayrightAuto.MobileTest.MobileServices.RecordLocators;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Mac;
using OpenQA.Selenium.DevTools.V117.Security;
using SafeCash.Test.ApiTest.InternalApiTest.Buyer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveAutomation.Test.UiTest.MobileTest.MobileFlows
{
    public class MobileBaseFlow
    {
        public AndroidDriver appiumDriver;
        MobileBasePages mobileBasePages;

        public MobileBaseFlow(AndroidDriver i_driver)
        {
            appiumDriver = i_driver;
            mobileBasePages = new MobileBasePages(i_driver);
        }

        public string GetFullPageSource()
        {
            string fullPageSource = appiumDriver.PageSource;
            return fullPageSource;
        }
        #region Talk with ai by text
        public async Task TalkWithApp(string elementView, string inputText ="")
        {
            By? element = await GetAiElementLocator(elementView);
            Assert.That(element != null, $"The element for view '{elementView}' was not found by AI.");

            if (string.IsNullOrEmpty(inputText))
            {
                mobileBasePages.MobileClickElement(element);
            }
            else
            {
                mobileBasePages.MobileInputTextToField(element, inputText);
            }

        }


        private async Task<By?> GetAiElementLocator(string elementView)
        {
            mobileBasePages.WaitForPageToLoad();
            string fullPageSource = GetFullPageSource();
            var aiService = new AndroidAiService();

            string locator = "";
            int retry = 0;

            while (retry < 2)
            {
                locator = await aiService.GetAndroidLocatorFromUserTextInput(fullPageSource, elementView);
                //Test if the locator valid, of no send it again to the ai
                if (AndroidAiService.IsLocatorIsVald(locator))
                    return By.XPath(locator);
                retry++;
            }

            Console.WriteLine($"[AI] Could not resolve a valid locator for '{elementView}'. Last attempt: {locator}");
            return null;
        }
        #endregion

        #region Click on Xy cordinate
        public async Task ClickOnXyUsingFile(string fileCordinatePath)
        {
            RecordLocatoreService recordLocatoreService = new RecordLocatoreService();
            string screenSize = RecordLocatoreService.GetDeviceScreenSizeString();
            var tapPoints = RecordLocatoreService.ExtractTouchCoordinates(fileCordinatePath);

            foreach (var (x, y) in tapPoints)
            {
                mobileBasePages.WaitForPageToLoad();
                mobileBasePages.AdbTap(x, y);
            }
        }
        private async Task<By?> GetAiElementLocatorFromXy(string fullSizeScreen,
            int x, int y, string screenSize)
        {
            mobileBasePages.WaitForPageToLoad();
            string fullPageSource = GetFullPageSource();
            var aiService = new AndroidAiService();

            string locator = "";
            int retry = 0;

            while (retry < 2)
            {
                locator = await aiService.GetAndroidLocatorFromUserXyCordinate(fullSizeScreen,
                    x, y, screenSize);
                //Test if the locator valid, of no send it again to the ai
                if (AndroidAiService.IsLocatorIsVald(locator))
                    return By.XPath(locator);
                retry++;
            }

            Console.WriteLine($"[AI] Could not resolve a valid locator for x:'{ x} and y: {y}'cordinate." +
                $" Last attempt: {locator}");
            return null;
        }
        #endregion

    }
}
