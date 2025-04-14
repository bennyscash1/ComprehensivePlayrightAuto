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
        public async Task ClickOnAiElement(string elementView)
        {
            mobileBasePages.WaitForPageToLoad();
            string fullPageSource = GetFullPageSource();
   

            AndroidAiService androidAiService = new AndroidAiService();
            string locatorXpathFromAi = await androidAiService
                .GetLocatorFromAndroidSourcePage(fullPageSource, elementView);
            bool isLocatorValid = AndroidAiService.IsLocatorIsVald(locatorXpathFromAi);
            int retry = 0;
            while (!isLocatorValid && retry<2)
            {
                locatorXpathFromAi = await androidAiService
                .GetLocatorFromAndroidSourcePage(fullPageSource, elementView);
                isLocatorValid = AndroidAiService.IsLocatorIsVald(locatorXpathFromAi);
                retry++;

            }
            if (isLocatorValid)
            {
                By element = By.XPath(locatorXpathFromAi);
                mobileBasePages
                    .ClickOnElementByLocator(element);
            }
            Assert.That(isLocatorValid, $"The element for view {elementView} not being found");

        }
        public async Task inputAiElement(string elementView, string inputText)
        {
            mobileBasePages.WaitForPageToLoad();
            string fullPageSource = GetFullPageSource();
            if (string.IsNullOrEmpty(fullPageSource))
            {
                fullPageSource = GetFullPageSource();
            }
            AndroidAiService androidAiService = new AndroidAiService();
            string locatorXpathFromAi = await androidAiService
                .GetLocatorFromAndroidSourcePage(fullPageSource, elementView);
            bool isLocatorValid = AndroidAiService.IsLocatorIsVald(locatorXpathFromAi);
            Assert.That(isLocatorValid, $"The element for view {elementView} not being found");

            if (isLocatorValid)
            {
                By element = By.XPath(locatorXpathFromAi);
                mobileBasePages
                    .fillText(element, inputText);
            }
            else
            {
                Console.WriteLine($"The element for user input {elementView}, from ai is not valid element {locatorXpathFromAi}");
            }
        }
    }
}
