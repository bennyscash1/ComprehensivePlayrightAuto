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
using NUnit.Framework;

namespace ComprehensiveAutomation.MobileTest.Inital
{
  
    public class MobileAiDriverFactory 
    {
        public AndroidDriver appiumDriver;
        public static bool runOnRealDevice = true;
        public static bool toInstallApp = false;
        private static bool retryInstallUiAutomator = true;
        public static string baseAppiumUrl = "http://127.0.0.1:4723/wd/hub";
        private static string appUrl = "https://github.com/bennyscash1/ComprehensivePlayrightAuto/releases/download/publicCalculator/calculatorUpdated.apk";

        public MobileAiDriverFactory(string appName = "")
        {
            //Can take the app name from user hard code by next code
            string allpackageList = GetMobileInstalledApps();
            string appPackage = GetAppPackageByName(appName);
            bool isAppListHaseAppPackage = IsAppPackageInTheMobileList(
                allpackageList, appPackage);
           // Assert.That(isAppListHaseAppPackage,$"The app {appName} not found on the mobile app, the app package return {appPackage}");         
            string appActivity = GetAppMainActivity(appPackage);
           // Assert.That(!string.IsNullOrEmpty(appActivity), $"The app activity for {appName} not found on the mobile app, the app package return {appPackage}");
            appiumDriver = InitAppiumDriver(appPackage, appActivity);

            //Or can take the app name from user> get the mobile brand and send it to ai 
        }
        private int retryCount = 0;
        private const int maxRetries = 1;
        public AndroidDriver InitAppiumDriver(string appP = "", string appA = "")
        {
            try
            {
                var appiumOptions = InitAppiumOptions(appP, appA);
                var uri = new Uri(baseAppiumUrl);
                var driver = new AndroidDriver(uri, appiumOptions, TimeSpan.FromMinutes(3));

                // Reset retry count after successful connection
                retryCount = 0;
                return driver;
            }
            catch (Exception ex)
            {
                if (retryCount < maxRetries)
                {
                    retryCount++;
                    UninstallUiAutomator2Packages();
                    Console.WriteLine($"Retrying Appium driver initialization... (attempt {retryCount})");
                    return InitAppiumDriver(appP, appA);
                }

                throw new Exception("Failed to initialize Appium driver after retrying.", ex);
            }
        }


        public AppiumOptions InitAppiumOptions(string appP, string appA)
        {
            string deviceUuid = GetDeviceUUID();

            var appiumOptions = new AppiumOptions();
            appiumOptions.PlatformName = "Android";
            appiumOptions.DeviceName = deviceUuid;
            appiumOptions.AutomationName = "UiAutomator2";
            appiumOptions.AddAdditionalAppiumOption(MobileCapabilityType.Udid, deviceUuid);
            appiumOptions.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, 150000);
            //Open the app if it exsist
            if (!string.IsNullOrEmpty(appP))
            {
                appiumOptions.AddAdditionalAppiumOption("appPackage", appP);
                appiumOptions.AddAdditionalAppiumOption("appActivity", appA);
            }


            //Install from download url - or reset app data
            if (toInstallApp)
            {
                appiumOptions.App = appUrl;
            }
            //
            return appiumOptions;
        }
        public AppiumOptions InitAppPackage(string appP, string appA)
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalAppiumOption("appPackage", appP);
            appiumOptions.AddAdditionalAppiumOption("appActivity", appA); 
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
        private void UninstallUiAutomator2Packages()
        {
            Console.WriteLine("Uninstalling UiAutomator2 server packages...");
            RunAdbCommand("uninstall io.appium.uiautomator2.server");
            RunAdbCommand("uninstall io.appium.uiautomator2.server.test");
        }
        private void RunAdbCommand(string args)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "adb",
                        Arguments = args,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit();
                Console.WriteLine($"ADB Output: {process.StandardOutput.ReadToEnd()}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error running adb command '{args}': {e.Message}");
            }
        }


        public string GetMobileInstalledApps()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "adb",
                        Arguments = "shell pm list packages",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                string packageList = string.Join(", ",
                    output.Split('\n')
                          .Where(line => !string.IsNullOrWhiteSpace(line))
                          .Select(line => line.Replace("package:", "").Trim()));

                return packageList;
            }
            catch (Exception ex)
            {
                return $"Error retrieving installed apps: {ex.Message}";
            }
        }

        #region Get app package and app activity
        public string GetAppPackageByName(string appName)
        {
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "adb",
                    Arguments = "shell pm list packages",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            var lines = output.Split('\n')
                              .Select(line => line.Trim().Replace("package:", "").Trim())
                              .Where(pkg => !string.IsNullOrWhiteSpace(pkg))
                              .ToList();

            // Try exact match first
            var exactMatch = lines.FirstOrDefault(pkg => pkg.Equals(appName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(exactMatch))
                return exactMatch;

            // Then try packages that end with ".appname" or contain ".appname"
            var smartMatch = lines.FirstOrDefault(pkg =>
                pkg.EndsWith("." + appName, StringComparison.OrdinalIgnoreCase) ||
                pkg.Contains("." + appName + ".", StringComparison.OrdinalIgnoreCase));

            return smartMatch;
        }

        public string GetAppMainActivity(string appPackage)
        {
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "adb",
                    Arguments = $"shell cmd package resolve-activity --brief {appPackage}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            // Split lines and find the one that contains the full activity
            var lines = output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines.Reverse())
            {
                if (line.Contains("/") && line.StartsWith(appPackage))
                {
                    var parts = line.Trim().Split('/');
                    if (parts.Length == 2)
                        return parts[1]; // return only the activity part
                }
            }

            return null;
        }

        #endregion
        #region test if the app list include the app package
        public bool IsAppPackageInTheMobileList(string appList, string appPackage)
        {
            if (string.IsNullOrWhiteSpace(appList) || string.IsNullOrWhiteSpace(appPackage))
                return false;

            var packages = appList.Split(',')
                                  .Select(p => p.Trim()) // clean up spaces
                                  .Where(p => !string.IsNullOrEmpty(p));

            return packages.Contains(appPackage);
        }

        #endregion
    
        public void Dispose()
        {
            appiumDriver.Quit();
        }

    }
}
