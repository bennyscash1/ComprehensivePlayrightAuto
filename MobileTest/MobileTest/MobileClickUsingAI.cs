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
            //Enter app name
            MobileAiDriverFactory mobileDriver = new MobileAiDriverFactory("Calculator");
            MobileLoginFlow mobileLoginFlow = new MobileLoginFlow(mobileDriver.appiumDriver);
            //Click on the app
            await mobileLoginFlow.ClickOnAiElement("Click on number 5");
            await mobileLoginFlow.ClickOnAiElement("Click on double button");
            await mobileLoginFlow.ClickOnAiElement("Click on number 2");
            await mobileLoginFlow.InputAiElement("input field phone number", "0501234568");


            #region click or enter text for ai chrome
            //   await mobileLoginFlow
            //       .ClickOnAiElement("Use without account");
            //  /* await mobileLoginFlow
            //      .ClickOnAiElement("Continue");*/
            //   await mobileLoginFlow
            //    .ClickOnAiElement("More");
            //   await mobileLoginFlow
            //   .ClickOnAiElement("Got it");
            ///*   await mobileLoginFlow
            //   .ClickOnAiElement("3 dots on top for settings");*/
            //   await mobileLoginFlow
            //     .inputAiElement("Search or type URL", "shalom");
            #endregion
        }
    }
}
