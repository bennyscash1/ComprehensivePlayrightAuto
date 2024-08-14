using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Remote;
using ComprehensiveAutomation.Test.Infra.BaseTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveAutomation.MobileTest.Inital
{
  
    public class MobileDriverFactory : Base
    {
        public AndroidDriver appiumDriver;

        public MobileDriverFactory()
        {
            appiumDriver = InitAppiumDriver();
        }

        public AndroidDriver InitAppiumDriver()
        {
            var appiumOption = InitAppiumOptions();
            var uri = new Uri("http://127.0.0.1:4723/wd/hub");
            var driver = new AndroidDriver(uri, appiumOption);
            return driver;
        }
        
        string appPackage = GetTestData(configDataEnum.appPackage);
        string appActivity = GetTestData(configDataEnum.appActivity);

        public AppiumOptions InitAppiumOptions()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalAppiumOption(MobileCapabilityType.PlatformName, "android");
            appiumOptions.AddAdditionalAppiumOption("deviceName ", "bennys9");
            appiumOptions.AddAdditionalAppiumOption("appPackage", appPackage);
            appiumOptions.AddAdditionalAppiumOption("appActivity", appActivity);
            appiumOptions.AddAdditionalAppiumOption(MobileCapabilityType.Udid, "bfd406d21897");
            appiumOptions.AddAdditionalAppiumOption("unicodeKeyboard", false);
            appiumOptions.AddAdditionalAppiumOption("resetKeyboard", false);
            return appiumOptions;
        }

    }
}
