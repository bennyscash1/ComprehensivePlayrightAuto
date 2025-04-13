using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComprehensiveAutomation.MobileTest.Inital;
using ComprehensiveAutomation.MobileTest.InitalMobile;

namespace ComprehensiveAutomation.Test.UiTest.MobileTest.MobilePageObject
{
    public class MobileBasePages : MobileBaseFunction
    {
        public AndroidDriver appiumDriver;
        public TimeSpan timeOutInSeconds = TimeSpan.FromSeconds(10);
        int maxAttamp = 2;

        public MobileBasePages(AndroidDriver i_driver) : base(i_driver)
        {
            appiumDriver = i_driver;
        }
        public static string GetMobileSourcePageLocator(AppiumDriver appiumDriver)
        {
            return appiumDriver.PageSource;
        }

        public MobileBasePages ClickOnElementByLocator(By i_elementLocator)
        {
            MobileClickElement(i_elementLocator);
            return this;
        }
        public MobileBasePages WaitForPageToLoad()
        {
            WaitUntilMobilePageStable();
            return this;
        }

    }
}
