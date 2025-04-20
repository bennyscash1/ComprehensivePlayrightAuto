using ComprehensiveAutomation.MobileTest.Inital;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.MobileTest.InitalMobile
{
    public class MobileDevicesMenegar
    {
        #region Run emulator and make sure adb devices not null
        public bool IsAnyDeviceConnected()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "adb",
                    Arguments = "devices",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            var lines = output.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            // Skip header and check if any line ends with "device" (online)
            return lines.Skip(1).Any(line => line.EndsWith("\tdevice"));
        }


        public void RestartAdb()
        {
            Console.WriteLine("Restarting ADB...");
            Process.Start("adb", "kill-server")?.WaitForExit();
            Process.Start("adb", "start-server")?.WaitForExit();
        }

        public void EnsureEmulatorRunning(string emulatorName = "Small_Phone_API_35")
        {
            if (!IsAnyDeviceConnected())
            {
                RestartAdb();

                if (!IsAnyDeviceConnected())
                {
                    Console.WriteLine("No devices found. Starting emulator...");

                    string emulatorPath = Path.Combine(Directory.GetCurrentDirectory(), "MobileTest", "emulator");
                    string emulatorExe = Path.Combine(emulatorPath, "emulator.exe");

                    if (!File.Exists(emulatorExe))
                        throw new FileNotFoundException("emulator.exe not found at: " + emulatorExe);
                    string commandAvd = $"-avd {emulatorName}";
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = emulatorExe,
                        Arguments = commandAvd,
                        WorkingDirectory = emulatorPath,
                        UseShellExecute = false,
                        CreateNoWindow = false
                    });

                    // Wait for emulator to appear in adb
                    int retries = 0;
                 
                    while (!IsAnyDeviceConnected() && retries++ < 10)
                    {
                        Console.WriteLine("Waiting for emulator to appear in adb...");
                        Thread.Sleep(1000);
                    }

                    if (!IsAnyDeviceConnected())
                        throw new Exception("Emulator failed to connect to ADB.");
                }
            }

            Console.WriteLine("Device is connected via ADB.");
        }
        #endregion
        public async Task RunAppiumServer()
        {
            string baseUrlAppium = MobileAiDriverFactory.baseAppiumUrl; // e.g., http://127.0.0.1:4723
            string statusUrl = $"{baseUrlAppium}/status";

            bool IsAppiumRunning()
            {
                try
                {
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

            // Start Appium server
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/C appium --address 0.0.0.0 --port 4723 --base-path /wd/hub", // ✅ optional if using /wd/hub
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            Console.WriteLine("Appium server starting...");

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

    }
}
