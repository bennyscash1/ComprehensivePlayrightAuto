using ComprehensiveAutomation.MobileTest.Inital;
using ComprehensiveAutomation.Test.UiTest.MobileTest.MobileFlows;
using ComprehensivePlayrightAuto.MobileTest.InitalMobile.InitialMobileService;
using ComprehensivePlayrightAuto.MobileTest.MobileServices.RecordLocators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
            MobileAiDriverFactory mobileRecordDriver = new MobileAiDriverFactory("Calculator");

            MobileBaseFlow mobileRecordFlow = new MobileBaseFlow(mobileRecordDriver.appiumDriver);

            RecordLocatoreService recordLocatoreService = new RecordLocatoreService();
            string recordFile = recordLocatoreService.CreateRecordFile();

            var proccess = recordLocatoreService.StartAdbRecordingToFile(recordFile);

            Thread.Sleep(3000);
            recordLocatoreService.StopAdbRecording(proccess);

            MobileEmulatorMenegar mobileDevicesMenegar = new MobileEmulatorMenegar();
            mobileDevicesMenegar.EnsureEmulatorRunning("Pixel_2_API_35");
            AppiumMenegar appiumMenegar = new AppiumMenegar();
            await appiumMenegar.RunAppiumServer();

            MobileAiDriverFactory mobileRunDriver = new MobileAiDriverFactory("Calculator");

            MobileBaseFlow mobileRunFlow = new MobileBaseFlow(mobileRunDriver.appiumDriver);
            mobileRunFlow.ClickOnXyUsingFile(recordFile);

        }
    }
}
