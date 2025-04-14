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
            MobileAiDriverFactory mobileDriver = new MobileAiDriverFactory("camera");
            MobileLoginFlow mobileLoginFlow = new MobileLoginFlow(mobileDriver.appiumDriver);
            #region for clock ai 
            await mobileLoginFlow.ClickOnAiElement("the big center button Take image button");

            await mobileLoginFlow.ClickOnAiElement("דיוקן");
            #endregion


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
