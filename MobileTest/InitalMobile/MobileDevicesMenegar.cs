using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.MobileTest.InitalMobile
{
    public class MobileDevicesMenegar
    {
        public void RunEmulator(string defaultEmulator = "Small_Phone_API_35")
        {
            string emulatorPath = Path.Combine(Directory.GetCurrentDirectory(), "MobileTest", "emulator");
            string emulatorExe = Path.Combine(emulatorPath, "emulator.exe");

            if (!File.Exists(emulatorExe))
            {
                throw new FileNotFoundException("emulator.exe not found at: " + emulatorExe);
            }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = emulatorExe,
                Arguments = $"-avd \"{defaultEmulator}\"",
                WorkingDirectory = emulatorPath, // רק התיקייה – לא הקובץ
                UseShellExecute = false,
                CreateNoWindow = false
            };

            Process emulatorProcess = new Process
            {
                StartInfo = startInfo
            };
            emulatorProcess.Start();
        }
        public void RunAppiumServer()
        {
            try
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
                Console.WriteLine("Appium server started.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start Appium: {ex.Message}");
            }
        }

    }
}
