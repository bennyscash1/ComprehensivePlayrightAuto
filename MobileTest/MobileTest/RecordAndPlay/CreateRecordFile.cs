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

namespace ComprehensivePlayrightAuto.MobileTest.MobileTest.RecordAndPlay
{
    [TestFixture, Category(
         Categories.MobileAndroid),
             Category(TestLevel.Level_2)]
    public class CreateRecordFile
    {
        static string runingApp = "Chrome";
        public static string recordFileName = "chromeFlow1";
        [SetUp]
        public async Task _SetupMobile()
        {
            MobileEmulatorMenegar mobileDevicesMenegar = new MobileEmulatorMenegar();
            mobileDevicesMenegar.EnsureEmulatorRunning("Pixel_2_API_35");
            AppiumMenegar appiumMenegar = new AppiumMenegar();
            await appiumMenegar.RunAppiumServer();


        }
        [Test]
        public async Task _CreateRecordFile()
        {
            #region Open recording session
            MobileAiDriverFactory mobileRecordDriver = new MobileAiDriverFactory(runingApp);
            MobileBaseFlow mobileRecordFlow = new MobileBaseFlow(mobileRecordDriver.appiumDriver);
            #endregion

            #region Get recording into file
            RecordLocatoreService recordLocatoreService = new RecordLocatoreService();
            string recordFile = recordLocatoreService.CreateRecordFile(recordFileName);

            Process recordProccess = recordLocatoreService.StartAdbRecordingToFile(recordFile);

            Thread.Sleep(1000);
            recordLocatoreService.StopAdbRecording(recordProccess);
            #endregion


        }
    }
}
