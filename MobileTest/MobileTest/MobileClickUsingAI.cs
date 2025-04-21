using ComprehensiveAutomation.Test.UiTest.MobileTest.MobileFlows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComprehensiveAutomation.MobileTest.Inital;
using static ComprehensiveAutomation.Test.Infra.BaseTest.EnumList;
using NUnit.Framework;
using OpenQA.Selenium.DevTools.V117.Runtime;
using ComprehensivePlayrightAuto.MobileTest.InitalMobile;

namespace ComprehensiveAutomation.MobileTest.MobileTest
{
    [TestFixture, Category(
        Categories.MobileAndroid),
    Category(TestLevel.Level_1)]
    public class MobileClickUsingAI 
    {
        [SetUp]
        public async Task SetupMobileDevice()
        {
            MobileDevicesMenegar mobileDevicesMenegar = new MobileDevicesMenegar();

            mobileDevicesMenegar.EnsureEmulatorRunning();
            await mobileDevicesMenegar.RunAppiumServer();
        }
        [Test]
        public async Task _MobileClickUsingAI()
        {
            MobileAiDriverFactory mobileDriver = new MobileAiDriverFactory("calculator");
            MobileLoginFlow mobileLoginFlow = new MobileLoginFlow(mobileDriver.appiumDriver);

            //Click on the app

            await mobileLoginFlow.ClickOnAiElement("Click on number 5");
            await mobileLoginFlow.ClickOnAiElement("Plus button");
            await mobileLoginFlow.ClickOnAiElement("Click on number 8");
            await mobileLoginFlow.ClickOnAiElement("Click on =");

            //await mobileLoginFlow.InputAiElement("Search input field", "Automatico");
           // await mobileLoginFlow.ClickOnAiElement("click on the first result from the list");
        }
    }
}
