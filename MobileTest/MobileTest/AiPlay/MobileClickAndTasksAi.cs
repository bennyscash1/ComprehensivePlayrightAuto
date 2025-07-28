using ComprehensiveAutomation.MobileTest.Inital;
using ComprehensiveAutomation.Test.UiTest.MobileTest.MobileFlows;
using ComprehensivePlayrightAuto.MobileTest.InitalMobile.InitialMobileService;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ComprehensiveAutomation.Test.Infra.BaseTest.EnumList;
using static ComprehensiveAutomation.Test.UiTest.MobileTest.MobileFlows.MobileAiTaskFlow;

namespace ComprehensivePlayrightAuto.MobileTest.MobileTest.AiPlay
{
    [TestFixture, Category(
       Categories.MobileAndroid),
   Category(TestLevel.Level_1)]
    public class MobileClickAndTasksAi
    {
        static string runingApp = "Calculator";

        [SetUp]
        public async Task SetupMobileDevice()
        {
            MobileEmulatorMenegar mobileDevicesMenegar = new MobileEmulatorMenegar();
            mobileDevicesMenegar.EnsureEmulatorRunning();
            AppiumMenegar appiumMenegar = new AppiumMenegar();
            await appiumMenegar.RunAppiumServer();
        }
        [Test]
        public async Task _MobileClickAndTasksAi()
        {
            MobileAiDriverFactory mobileDriver = new MobileAiDriverFactory(runingApp);
            MobileAiTaskFlow mobileTaskFlow = new MobileAiTaskFlow(mobileDriver.appiumDriver);

            //Click on app buttons
            await mobileTaskFlow.TalkWithApp("Click on .0");



            int type = (int)aiResponceTypeEnumMobile.ButtonLocator;
            type = await mobileTaskFlow.HandleAiTaskMission(
                   "Click on number 9,  then on operator +, and click on number 8");
            await mobileTaskFlow.TalkWithApp("Click on Operator =");

        }

    }
}
