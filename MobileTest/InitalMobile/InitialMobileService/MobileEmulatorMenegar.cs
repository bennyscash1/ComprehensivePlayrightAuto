using ComprehensiveAutomation.MobileTest.Inital;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.MobileTest.InitalMobile.InitialMobileService
{
    public class MobileEmulatorMenegar
    {
        #region Check if any device connect
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
        #endregion

        #region Kill adb and restart it
        public void RestartAdb()
        {
            Console.WriteLine("Restarting ADB...");
            Process.Start("adb", "kill-server")?.WaitForExit();
            Process.Start("adb", "start-server")?.WaitForExit();
        }
        #endregion

        #region get device id 
        public string GetFirstConnectedDeviceId()
        {
            int retries = 0;
            while (retries++ < 20)
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

                var lines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                                  .Skip(1)
                                  .Where(line => line.Contains("\tdevice"))
                                  .ToList();

                if (lines.Any())
                {
                    return lines.First().Split('\t')[0];
                }

                Console.WriteLine("Waiting for device to connect...");
                Thread.Sleep(1000);
            }

            throw new Exception("No connected device found after waiting.");
        }
        #endregion

        #region Check if device is ready to use
        public bool IsDeviceBootCompleted(string deviceId)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "adb",
                        Arguments = $"-s {deviceId} shell getprop sys.boot_completed",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();

                return output == "1";
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Run the emulator and make sure it ready to use
        public void EnsureEmulatorRunning(string emulatorName = "Small_Phone_API_35")
        {
            if (!IsAnyDeviceConnected())
            {
                RestartAdb();

                if (!IsAnyDeviceConnected())
                {
                    Console.WriteLine("No devices found. Starting emulator...");

                    string emulatorPath = Path.Combine(Directory.GetCurrentDirectory(), "MobileTest", "MobileServices", "emulator");
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
                    while (!IsAnyDeviceConnected() && retries++ < 40)
                    {
                        Console.WriteLine("Waiting for emulator to appear in adb...");
                        Thread.Sleep(2000);
                    }

                    if (!IsAnyDeviceConnected())
                        throw new Exception("Emulator failed to connect to ADB.");
                }
            }

            Console.WriteLine("Device is connected via ADB.");

            // ✅ Wait for device to finish booting
            string deviceId = GetFirstConnectedDeviceId();
            int bootCheckRetries = 0;
            while (!IsDeviceBootCompleted(deviceId) && bootCheckRetries++ < 60)
            {
                Console.WriteLine("Waiting for device to complete boot...");
                Thread.Sleep(2000);
            }

            if (!IsDeviceBootCompleted(deviceId))
                throw new Exception("Device did not finish booting in time.");

            Console.WriteLine("Device is ready for use.");
        }

        #endregion

        #region Get app opened name 
        public static string GetForegroundAppPackage()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "adb",
                        Arguments = "shell \"dumpsys window | grep mCurrentFocus\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // Trim and check if output contains the expected string
                if (string.IsNullOrWhiteSpace(output) || !output.Contains("mCurrentFocus"))
                    return "not application is open now, please check again";

                // Extract the app package from the full string
                var marker = "mCurrentFocus=";
                var index = output.IndexOf(marker);
                if (index != -1)
                {
                    var sub = output.Substring(index + marker.Length).Trim();
                    var tokens = sub.Split(' ');
                    foreach (var token in tokens)
                    {
                        if (token.Contains("/"))
                        {
                            var appPackage = token.Split('/')[0];
                            return appPackage;
                        }
                    }
                }

                return "not application is open now, please check again";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        #endregion
    }
}
