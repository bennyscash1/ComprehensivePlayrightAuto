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
        public static bool runOnRemote = true;
        private static string appUrl = "https://github.com/bennyscash1/ComprehensivePlayrightAuto/releases/download/publicCalculator/calculatorUpdated.apk";

        public MobileDriverFactory()
        {
            if (!runOnRemote)
            {
                appiumDriver = InitAppiumDriver();
            }
            else
            {
                InitRemoteAppiumDriver();
            }
        }

        public AndroidDriver InitAppiumDriver()
        {
            var appiumOption = InitAppiumOptions();
            var uri = new Uri("http://127.0.0.1:4723/wd/hub");
            var driver = new AndroidDriver(uri, appiumOption);
            return driver;
        }

        private AppiumDriver InitRemoteAppiumDriver()
        {
            var options = InitApppiumRemoteOptions();
          //  string apiToken = "99fxv9wdn0zl1s3oa25pmd9nj9qcr0ik92p1ynrg14dg02tjv9e7a2r9qaghkz185ps1152nrqjc5pu5qu1rw7shiqgks3agdrczzjm8fniyr6bf9z96pfy7y2d5805bfas8ndia9mmyt3m1jv4bg4ptk64j53nxhpj1ek86l144qrj7qcsfvwy53qjya56l54vwje4sknrem0f6z0yhx8d1uvmlfi1g56lz6psrpk61e8w44azki0eb9cdovmvb";
            var remoteUrl = new Uri($"https://api.appetize.io/wd/hub");
            return new AndroidDriver(remoteUrl, options);
        }

        /*    public AppiumOptions InitAppiumOptions()
            {
                string deviceUuid = GetDeviceUUID();
                string buyerAppPackage = GetTestData(configDataEnum.appPackage);
                string buyerAppActivity = GetTestData(configDataEnum.appActivity);

                var appiumOptions = new AppiumOptions();
                appiumOptions.PlatformName = "Android";
                appiumOptions.DeviceName = deviceUuid;
                appiumOptions.AutomationName = "UiAutomator2";

                appiumOptions.AddAdditionalAppiumOption(MobileCapabilityType.Udid, deviceUuid);

                appiumOptions.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, 100000);
                appiumOptions.AddAdditionalAppiumOption(MobileCapabilityType.PlatformName, "android");

                appiumOptions.AddAdditionalAppiumOption("appPackage", buyerAppPackage);
                appiumOptions.AddAdditionalAppiumOption("appActivity", buyerAppActivity);
                return appiumOptions;
            }*/
        public AppiumOptions InitAppiumOptions()
        {
            string deviceUuid = GetDeviceUUID();

            var appiumOptions = new AppiumOptions();
            appiumOptions.PlatformName = "Android";
            appiumOptions.DeviceName = deviceUuid;
            appiumOptions.AutomationName = "UiAutomator2";
            appiumOptions.AddAdditionalAppiumOption(MobileCapabilityType.Udid, deviceUuid);
            appiumOptions.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, 100000);

            appiumOptions.AddAdditionalAppiumOption("appPackage", "com.google.android.calculator");
            appiumOptions.AddAdditionalAppiumOption("appActivity", "com.android.calculator2.Calculator");
            // הסרת AppPackage ו-AppActivity כי האפליקציה תותקן עכשיו אוטומטית מהקישור.
            //appiumOptions.App = appUrl;

            return appiumOptions;
        }


        public AppiumOptions InitApppiumRemoteOptions()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalAppiumOption("platformName", "Android");
            appiumOptions.AddAdditionalAppiumOption("capabilityName", "Samsung Galaxy S23"); // שם המכשיר שיצרת
            appiumOptions.AddAdditionalAppiumOption("capabilityName", "UiAutomator2");
            // appiumOptions.DeviceName = "721bdcf2-2a36-482b-b3ee-96eb28d1a87e";

           // appiumOptions.App = "https://github.com/bennyscash1/ComprehensivePlayrightAuto/releases/download/publicCalculator/calculatorUpdated.apk";
           // appiumOptions.AddAdditionalAppiumOption("appPackage", "com.google.android.calculator");
          //  appiumOptions.AddAdditionalAppiumOption("appActivity", "com.android.calculator2.Calculator");
            appiumOptions.App = "b_ch3r4a2yddfea7tyotowanm5eq";
            appiumOptions.AddAdditionalAppiumOption("appetizeToken", "tok_lbe6tj6ocflf65ljhlp4pzjbwy");
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
