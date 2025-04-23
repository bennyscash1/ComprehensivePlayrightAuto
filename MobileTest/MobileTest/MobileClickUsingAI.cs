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

namespace ComprehensiveAutomation.MobileTest.MobileTest
{
    [TestFixture, Category(
        Categories.MobileAndroid),
    Category(TestLevel.Level_1)]
    public class MobileClickUsingAI 
    {
        string appName = MobileEmulatorMenegar.GetForegroundAppPackage();

        [SetUp]
        public async Task SetupMobileDevice()
        {
            MobileEmulatorMenegar mobileDevicesMenegar = new MobileEmulatorMenegar();
            mobileDevicesMenegar.EnsureEmulatorRunning("Pixel_2_API_35");
            AppiumMenegar appiumMenegar = new AppiumMenegar();
            await appiumMenegar.RunAppiumServer();
        }
        [Test] 
        public async Task _MobileClickUsingAI()
        {
            MobileAiDriverFactory mobileDriver = new MobileAiDriverFactory(appName);
            MobileBaseFlow mobileFlow = new MobileBaseFlow(mobileDriver.appiumDriver);

            //Click on app buttons
           // await mobileFlow.TalkWithApp("Search contacts");
          //  await mobileFlow.TalkWithApp("Search contacts", "benny shor");
            await mobileFlow.TalkWithApp("Blue icon with the 10 dots");
            await mobileFlow.TalkWithApp("Click on number 8");
            await mobileFlow.TalkWithApp("Click on =");
        }









        //await mobileLoginFlow.TalkWithYouApp("Click on number 5");
        //await mobileLoginFlow.TalkWithYouApp("Plus button");
        //await mobileLoginFlow.TalkWithYouApp("Click on number 8");
        //await mobileLoginFlow.TalkWithYouApp("Click on =");

/*        await mobileLoginFlow.TalkWithYouApp("Click on 'use wihtout an account'");
        await mobileLoginFlow.TalkWithYouApp("More button");
        await mobileLoginFlow.TalkWithYouApp("Click on 'Got it'");
        await mobileLoginFlow.TalkWithYouApp("Search input field", "Automatico");*/
    }
}
