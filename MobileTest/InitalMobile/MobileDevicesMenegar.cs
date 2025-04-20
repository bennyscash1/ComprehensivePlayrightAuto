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
            return lines.Length > 1; // device(s) listed after header
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

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = emulatorExe,
                        Arguments = $"-avd \"{emulatorName}\"",
                        WorkingDirectory = emulatorPath,
                        UseShellExecute = false,
                        CreateNoWindow = false
                    });

                    // Wait for emulator to appear in adb
                    int retries = 0;
                    while (!IsAnyDeviceConnected() && retries++ < 15)
                    {
                        Console.WriteLine("Waiting for emulator to appear in adb...");
                        Thread.Sleep(2000);
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
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/C appium --address 127.0.0.1 --port 4723",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            Console.WriteLine("Appium server starting...");

            const string statusUrl = "http://127.0.0.1:4723/wd/hub/status";
            var timeout = TimeSpan.FromSeconds(10);
            var startTime = DateTime.UtcNow;

            while (DateTime.UtcNow - startTime < timeout)
            {
                try
                {
                    using var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(statusUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Appium server is ready.");
                        return;
                    }
                }
                catch
                {
                    // ignore and retry
                }
                await Task.Delay(1000); // wait 1 second before retry
            }
            throw new Exception("Appium server failed to start within 10 seconds.");
        }

    }
}
