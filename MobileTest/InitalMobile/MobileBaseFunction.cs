using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using System.Diagnostics;

namespace ComprehensiveAutomation.MobileTest.InitalMobile
{
    public class MobileBaseFunction
    {
        public AndroidDriver appiumDriver;
        public AndroidDriver appiumChromeDriver;
        public TimeSpan timeOutInSeconds = TimeSpan.FromSeconds(6);
        int maxAttamp = 6;


        public MobileBaseFunction(AndroidDriver driver)
        {
            appiumDriver = driver;
            appiumChromeDriver = driver;
        }
        public bool isElementFound(By el)
        {
            WebDriverWait wait = new WebDriverWait(appiumDriver, timeOutInSeconds);
            bool isElementFound = false;
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(el));
                isElementFound = true;
            }
            catch (WebDriverTimeoutException e)
            {
                isElementFound = false;
            }
            return isElementFound;
        }


        public void WaitForElement(By el)
        {
            int attempt = 0;
            bool elementClick = false;

            while (attempt++ < maxAttamp)
            {
                if (isElementFound(el) == true)
                {
                    elementClick = true;
                    break;
                }
                attempt++;
            }
            var elementDetails = $"Element {el} search faild within {timeOutInSeconds}";
            Assert.That(elementClick, elementDetails);
        }
        public void MobileClickElement(By el)
        {
            int attempt = 0;
            bool elementClick = false;

            while (attempt++ < maxAttamp)
            {
                if (isElementFound(el) == true)
                {
                    appiumDriver.FindElement(el).Click();
                    elementClick = true;
                    break;
                }

                attempt++;
            }
            var elementDetails = $"Element {el} search faild within {timeOutInSeconds}";
            Assert.That(elementClick, elementDetails);
        }

        public void MobileInputTextToField(By el, String text)
        {
            bool elementClick = false;
            int attempt = 0;

            while (attempt++ < maxAttamp)
            {
                if (isElementFound(el) == true)
                {
                    appiumDriver.FindElement(el).SendKeys(text);
                    elementClick = true;
                    break;
                }
                attempt++;
            }
            var elementDetails = $"Element {el} sendkeys faild within {timeOutInSeconds} seconds.";
            Assert.That(elementClick, elementDetails);
        }
        public bool IsHavyElementFount(By el)
        {
            WebDriverWait wait = new WebDriverWait(appiumDriver, timeOutInSeconds);
            bool isElementFound = false;
            try
            {
                wait.Until(driver =>
                {
                    try
                    {
                        var element = driver.FindElement(el);
                        return element.Displayed && element.Enabled;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                });
                isElementFound = true;
            }
            catch (WebDriverTimeoutException e)
            {
                isElementFound = false;
            }
            return isElementFound;
        }
        public void WaitUntilMobilePageStable()
        {
            int attempt = 0;
            int maxAttampForLoadPage = 15;
            bool isStable = false;
            string previousSource = string.Empty;
            string currentSource = appiumDriver.PageSource;

            while (attempt++ < maxAttampForLoadPage)
            {
                Thread.Sleep(200); // Wait a bit between checks

                previousSource = currentSource;
                currentSource = appiumDriver.PageSource;

                if (previousSource.Equals(currentSource))
                {
                    // Confirm stability for a few checks (2 more)
                    Thread.Sleep(200);
                    var secondCheck = appiumDriver.PageSource;
                    Thread.Sleep(200);
                    var thirdCheck = appiumDriver.PageSource;

                    if (secondCheck.Equals(currentSource) && thirdCheck.Equals(currentSource))
                    {
                        isStable = true;
                        break;
                    }
                }

                attempt++;
            }

            var failMessage = $"Page did not stabilize within {timeOutInSeconds} seconds.";
            Assert.That(isStable, failMessage);
        }

        public void MovbileClickBack()
        {
            appiumDriver.Navigate().Back();
        }
        public void HideKyboard()
        {
            appiumDriver.HideKeyboard();
        }
        public void OpenStatusBar()
        {
            appiumDriver.OpenNotifications();
        }
        public void CloseStatusBar()
        {

            var screenWidth = appiumDriver.Manage().Window.Size.Width;
            var screenHeight = appiumDriver.Manage().Window.Size.Height;

            var touchAction = new TouchAction(appiumDriver);
            touchAction.Press(screenWidth / 2, screenHeight - 10).Release().Perform();

        }

        public MobileBaseFunction ScrollDown()
        {
            Thread.Sleep(1000);
            int startX = 500; // X coordinate to start the swipe
            int startY = 1800; // Y coordinate to start the swipe (bottom of the screen)
            int endX = 500; // X coordinate to end the swipe
            int endY = 100;
            TouchAction touchAction = new TouchAction(appiumChromeDriver);
            touchAction.Press(startX, startY).Wait(1000).MoveTo(endX, endY).Release().Perform();
            return this;
        }
 
        #region X and y 
        public void MobileClickLocatorXy(int x, int y)
        {
            var touchDevice = new PointerInputDevice(PointerKind.Touch);
            var sequence = new ActionSequence(touchDevice, 0);

            sequence.AddAction(touchDevice.CreatePointerMove(CoordinateOrigin.Viewport, x, y, TimeSpan.Zero));
            sequence.AddAction(touchDevice.CreatePointerDown(0)); // 0 = primary touch
            sequence.AddAction(touchDevice.CreatePointerUp(0));

            appiumDriver.PerformActions(new List<ActionSequence> { sequence });
        }
        public void AdbTap(int x, int y)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "adb",
                    Arguments = $"shell input tap {x} {y}",
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            proc.WaitForExit();
        }



        #endregion
    }
}
