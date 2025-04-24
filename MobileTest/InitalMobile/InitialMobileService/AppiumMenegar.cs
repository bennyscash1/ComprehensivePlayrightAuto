using ComprehensiveAutomation.MobileTest.Inital;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.MobileTest.InitalMobile.InitialMobileService
{
    public class AppiumMenegar
    {

        #region appium server run
        public async Task RunAppiumServer()
        {
            bool IsAppiumRunning()
            {
                try
                {
                    string baseUrlAppium = MobileAiDriverFactory.baseAppiumUrl; // 
                    string statusUrl = $"{baseUrlAppium}/status";

                    using var client = new HttpClient();
                    var response = client.GetAsync(statusUrl).Result;
                    return response.IsSuccessStatusCode;
                }
                catch
                {
                    return false;
                }
            }

            // Check if already running
            if (IsAppiumRunning())
            {
                Console.WriteLine("Appium server is already running.");
                return;
            }
            string appiumLoalPath = "C:\\Bennys\\Developing\\MobileServices\\AppiumService\\appium.cmd";

            // string appiumPath = @"C:\Users\benis\AppData\Roaming\npm\appium.cmd";
            string appiumArg = $"--address 127.0.0.1 --port {MobileAiDriverFactory.appiumPort}";

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = appiumLoalPath,
                    Arguments = appiumArg,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.OutputDataReceived += (sender, args) => Console.WriteLine("OUT: " + args.Data);
            process.ErrorDataReceived += (sender, args) => Console.WriteLine("ERR: " + args.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // Retry until it's available
            int maxRetries = 10;
            for (int i = 0; i < maxRetries; i++)
            {
                if (IsAppiumRunning())
                {
                    Console.WriteLine("Appium server is ready.");
                    return;
                }

                await Task.Delay(1000);
                Console.WriteLine($"Waiting for Appium server... retry {i + 1}/{maxRetries}");
            }

            throw new Exception("Appium server failed to start after multiple retries.");
        }
        #endregion
    }
}
