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
using ComprehensivePlayrightAuto.MobileTest.InitalMobile.InitialMobileService;

namespace ComprehensivePlayrightAuto.MobileTest.MobileTest.AiPlay
{
    [TestFixture, Category(
        Categories.MobileAndroid),
    Category(TestLevel.Level_1)]
    public class MobileAiTaskMission
    {
        static string runingApp = "camera";

        [SetUp]
        public async Task SetupMobileDevice()
        {
            MobileEmulatorMenegar mobileDevicesMenegar = new MobileEmulatorMenegar();
            mobileDevicesMenegar.EnsureEmulatorRunning("Pixel_2_API_35");
            AppiumMenegar appiumMenegar = new AppiumMenegar();
            await appiumMenegar.RunAppiumServer();
        }
        [Test]
        public async Task _MobileAiTaskMission()
        {
            MobileAiDriverFactory mobileDriver = new MobileAiDriverFactory(runingApp);
            MobileAiTaskFlow mobileTaskFlow = new MobileAiTaskFlow(mobileDriver.appiumDriver);

            //Click on app buttons
            int type=  await mobileTaskFlow.HandleAiResponce("נווט למסך צילום וידאו ותלחץ על התחלת צילום וידאו");
        }

    }
}
