using ComprehensiveAutomation.Test.UiTest.MobileTest.MobilePageObject;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Mac;
using SafeCash.Test.ApiTest.InternalApiTest.Buyer;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public string GetFullPageElementByFirstTime()
        {
            return MobileBasePages.GetMobileSourcePageLocator(appiumDriver);
        }
        public async Task ClickOnAiElement(string elementView)
        {
            string fullPageSource = MobileBasePages.GetMobileSourcePageLocator(appiumDriver);
            if (string.IsNullOrEmpty(fullPageSource))
            {
                fullPageSource = GetFullPageElementByFirstTime();
            }
            AndroidAiService androidAiService = new AndroidAiService();
            string locatorXpathFromAi = await androidAiService
                .GetLocatorFromAndroidSourcePage(fullPageSource, elementView);
            bool isLocatorValid = AndroidAiService.IsLocatorIsVald(locatorXpathFromAi);
            if (isLocatorValid)
            {
                By element = By.XPath(locatorXpathFromAi);
                mobileBasePages
                    .ClickOnElementByLocator(element);
            }
            else
            {
                Console.WriteLine($"The element from ai is not valid element {locatorXpathFromAi}");
            }
           
        }
    }
}
