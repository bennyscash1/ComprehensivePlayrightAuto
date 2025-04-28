using ComprehensiveAutomation.MobileTest.Inital;
using ComprehensiveAutomation.Test.UiTest.MobileTest.MobileFlows;
using ComprehensivePlayrightAuto.MobileTest.InitalMobile.InitialMobileService;
using ComprehensivePlayrightAuto.MobileTest.MobileServices.RecordLocators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ComprehensiveAutomation.Test.Infra.BaseTest.EnumList;

namespace ComprehensivePlayrightAuto.MobileTest.MobileTest
{
    [TestFixture, Category(
    Categories.MobileAndroid),
        Category(TestLevel.Level_1)]
    public class MobileRecordAndPlay
    {
        static string runingApp = "Chrome";

        [SetUp]
        public async Task SetupMobileRecodr()
        {
            MobileEmulatorMenegar mobileDevicesMenegar = new MobileEmulatorMenegar();
            mobileDevicesMenegar.EnsureEmulatorRunning("Pixel_2_API_35");
            AppiumMenegar appiumMenegar = new AppiumMenegar();
            await appiumMenegar.RunAppiumServer();


        }
        [Test]
        public async Task _MobileRecordAndPlay()
        {
            #region Open recording session
            MobileAiDriverFactory mobileRecordDriver = new MobileAiDriverFactory(runingApp);
            MobileBaseFlow mobileRecordFlow = new MobileBaseFlow(mobileRecordDriver.appiumDriver);
            #endregion

            #region Get recording into file
            RecordLocatoreService recordLocatoreService = new RecordLocatoreService();
            string recordFile = recordLocatoreService.CreateRecordFile();

            Process recordProccess = recordLocatoreService.StartAdbRecordingToFile(recordFile);

            Thread.Sleep(1000);
            recordLocatoreService.StopAdbRecording(recordProccess);
            #endregion

            #region Get touch coordinates
            MobileAiDriverFactory mobileRecordDriverx = new MobileAiDriverFactory(runingApp);
            MobileBaseFlow mobileRecordFlowx = new MobileBaseFlow(mobileRecordDriverx.appiumDriver);

            await mobileRecordFlowx.ClickOnXyUsingFile(recordFile);
            #endregion

        }
    }
}
