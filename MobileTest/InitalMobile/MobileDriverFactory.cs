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
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace ComprehensiveAutomation.MobileTest.Inital
{
  
    public class MobileDriverFactory : Base, IDisposable
    {
        public AndroidDriver appiumDriver;
        public static bool runOnRealDevice = true;
        public static bool toInstallApp = false;
        private static string appUrl = "https://github.com/bennyscash1/ComprehensivePlayrightAuto/releases/download/publicCalculator/calculatorUpdated.apk";

        public MobileDriverFactory()
        {
            if (runOnRealDevice)
            {
                appiumDriver = InitAppiumDriver(toInstallApp);
            }
            else
            {
                appiumDriver = InitRemoteAppiumDriver(toInstallApp);
            }
        }

        public AndroidDriver InitAppiumDriver(bool toInstallApp)
        {
            var appiumOption = InitAppiumOptions();
            var uri = new Uri("http://127.0.0.1:4723/wd/hub");
            var driver = new AndroidDriver(uri, appiumOption);
            return driver;
        }

        private AndroidDriver InitRemoteAppiumDriver(bool toInstallApp)
        {
            var options = InitApppiumRemoteOptions();
            var remoteUrl = new Uri($"http://localhost:4723/wd/hub");
            var driver = new AndroidDriver(remoteUrl, options);

            return driver;
        }

 
        public AppiumOptions InitAppiumOptions()
        {
            string deviceUuid = GetDeviceUUID();

            var appiumOptions = new AppiumOptions();
            appiumOptions.PlatformName = "Android";
            appiumOptions.DeviceName = deviceUuid;
            appiumOptions.AutomationName = "UiAutomator2";
            appiumOptions.AddAdditionalAppiumOption(MobileCapabilityType.Udid, deviceUuid);
            appiumOptions.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, 150000);

            appiumOptions.AddAdditionalAppiumOption("appPackage", "com.google.android.calculator");
            appiumOptions.AddAdditionalAppiumOption("appActivity", "com.android.calculator2.Calculator");

            //Install from download url - or reset app data
            if (toInstallApp)
            {
                appiumOptions.App = appUrl;
            }
            //
            //appiumOptions.AddAdditionalAppiumOption("noReset", false);


            return appiumOptions;
        }


        public AppiumOptions InitApppiumRemoteOptions()
        {
            string deviceUuid = GetDeviceUUID();
            var appiumOptions = new AppiumOptions();
            appiumOptions.PlatformName = "Android";
            appiumOptions.DeviceName = deviceUuid;
            appiumOptions.AutomationName = "UiAutomator2";

          //   appiumOptions.App = appUrl;
            appiumOptions.AddAdditionalAppiumOption("appPackage", "com.google.android.calculator");
            appiumOptions.AddAdditionalAppiumOption("appActivity", "com.android.calculator2.Calculator");
            //appiumOptions.AddAdditionalAppiumOption("noReset", false);

            // הוספת האפליקציה מקישור ישיר
            return appiumOptions;
        }
        public string GetDeviceUUID()
        {
            // Start a new process for adb command
            Process process = new Process();
            process.StartInfo.FileName = "adb";
            process.StartInfo.Arguments = "get-serialno";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;

            // Start the process and get the output
            process.Start();
            string uuid = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();
            return uuid;
        }

        public void Dispose()
        {
            appiumDriver.Quit();
        }

        }
}
