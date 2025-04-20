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
        [Test]
        public async Task _MobileClickUsingAI()
        {
            MobileDevicesMenegar mobileDevicesMenegar = new MobileDevicesMenegar();

            mobileDevicesMenegar.RunEmulator("Small_Phone_API_35");
            MobileAiDriverFactory mobileDriver = new MobileAiDriverFactory("chrome");
            MobileLoginFlow mobileLoginFlow = new MobileLoginFlow(mobileDriver.appiumDriver);
            //Click on the app
            await mobileLoginFlow.ClickOnAiElement("User without an account");

            await mobileLoginFlow.ClickOnAiElement("More");

            await mobileLoginFlow.InputAiElement("click on search input", "xxx");

            await mobileLoginFlow.ClickOnAiElement("Click on number 6");

        }
    }
}
