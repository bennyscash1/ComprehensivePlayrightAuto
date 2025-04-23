using ComprehensiveAutomation.Test.UiTest.MobileTest.MobilePageObject;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Mac;
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
        public async Task TalkWithApp(string elementView, string inputText ="")
        {
            By? element = await GetAiElementLocator(elementView);
            if (element ==null)
            {
                element = await GetAiElementLocator(elementView);
            }
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

/*        public async Task InputAiElement(string elementView, string inputText)
        {
            By? element = await GetAiElementLocator(elementView);
            Assert.That(element != null, $"The element for view '{elementView}' was not found by AI.");

        }*/

        private async Task<By?> GetAiElementLocator(string elementView)
        {
            mobileBasePages.WaitForPageToLoad();
            string fullPageSource = GetFullPageSource();
            var aiService = new AndroidAiService();

            string locator = "";
            int retry = 0;

            while (retry < 2)
            {
                locator = await aiService.GetLocatorFromAndroidSourcePage(fullPageSource, elementView);
                if (AndroidAiService.IsLocatorIsVald(locator))
                    return By.XPath(locator);
                retry++;
            }

            Console.WriteLine($"[AI] Could not resolve a valid locator for '{elementView}'. Last attempt: {locator}");
            return null;
        }

    }
}
