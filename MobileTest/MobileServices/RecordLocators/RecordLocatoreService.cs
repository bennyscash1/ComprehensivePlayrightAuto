using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.MobileTest.MobileServices.RecordLocators
{
    public class RecordLocatoreService
    {
        public static string screenHandler = "#SCREEN";
        public string CreateRecordFile(string fileName = "")
        {
            string chromeGeneralPath = Path.Combine(Directory.GetCurrentDirectory(), "MobileTest", "MobileServices", "RecordLocators", "LocatorsFiles");
            Directory.CreateDirectory(chromeGeneralPath);

            if (string.IsNullOrEmpty(fileName))
                fileName = Guid.NewGuid().ToString() + ".txt";
            else
                fileName = fileName + ".txt";

            return Path.Combine(chromeGeneralPath, fileName);
        }
        public static (int x, int y) GetDevicesSize()
        {
            string? screenSizeLine = RunShell("adb shell wm size")
                .Split('\n')
                .FirstOrDefault(x => x.Contains("Physical size"));

            if (screenSizeLine == null)
                throw new Exception("Unable to get device screen size.");

            // Extract "1080x1920" from "Physical size: 1080x1920"
            string sizePart = screenSizeLine.Split(":")[1].Trim();
            var parts = sizePart.Split("x");

            int width = int.Parse(parts[0]);
            int height = int.Parse(parts[1]);

            return (width, height);
        }
        public static string GetDeviceScreenSizeString()
        {
            string output = RunShell("adb shell wm size");
            return $"The current device screen size is: {output.Trim()}";
        }

        public Process StartAdbRecordingToFile(string fullFilePath)
        {
            (int deviceX, int deviceY) screenSizeLine = GetDevicesSize();
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "adb",
                    Arguments = "shell getevent -t",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            Task.Run(async () =>
            {
                using var writer = new StreamWriter(fullFilePath);
                if (!string.IsNullOrEmpty(screenSizeLine.deviceX.ToString()))
                    await writer.WriteLineAsync($"{screenHandler} Physical size: {screenSizeLine.deviceX}x{screenSizeLine.deviceY}");

                while (!process.StandardOutput.EndOfStream)
                {
                    var line = await process.StandardOutput.ReadLineAsync();
                    if (line != null)
                        await writer.WriteLineAsync(line);
                }
            });

            return process;
        }

        private static string RunShell(string cmd)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {cmd}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            return output;
        }

        public void StopAdbRecording(Process process)
        {
            if (process != null && !process.HasExited)
            {
                Thread.Sleep(500); // wait for I/O flush (you can bump this if needed)

                process.Kill();
                Thread.Sleep(1000); // wait for I/O flush (you can bump this if needed)
                process.Dispose();
            }
        }

        public static List<(int x, int y)> ExtractTouchCoordinates(string eventFilePath)
        {
            var allLines = File.ReadAllLines(eventFilePath).ToList();
            var screenLine = allLines.FirstOrDefault(l => l.StartsWith("#SCREEN"));
            if (screenLine == null)
                throw new Exception("Missing screen size in recording file.");

            allLines.Remove(screenLine);

            (int currentWidth, int currentHeight) = GetDevicesSize();
            var coordinates = new List<(int x, int y)>();

            int? rawX = null, rawY = null;
            bool touchStarted = false;

            const int maxRawX = 4095;
            const int maxRawY = 4095;

            foreach (var line in allLines)
            {
                if (line.Contains("0039"))
                {
                    if (line.Contains("ffffffff"))
                    {
                        touchStarted = false;
                    }
                    else
                    {
                        touchStarted = true;
                        rawX = rawY = null;
                    }
                }

                if (!touchStarted)
                    continue;

                if (line.Contains("0035"))
                    rawX = Convert.ToInt32(line.Trim().Split(' ').Last(), 16);

                if (line.Contains("0036"))
                    rawY = Convert.ToInt32(line.Trim().Split(' ').Last(), 16);

                if (rawX.HasValue && rawY.HasValue)
                {
                    int scaledX = rawX.Value * currentWidth / maxRawX;
                    int scaledY = rawY.Value * currentHeight / maxRawY;
                    coordinates.Add((scaledX, scaledY));
                    rawX = rawY = null;
                }
            }

            return coordinates;
        }





    }
}
