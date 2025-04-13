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
    public class MobileClickUsingAI : MobileDriverFactory
    {
        [Test]
        public async Task _MobileClickUsingAI()
        {


            MobileLoginFlow mobileLoginFlow = new MobileLoginFlow(appiumDriver);

            #region Give permission for mobile
            await mobileLoginFlow
                .ClickOnAiElement("5");
            await mobileLoginFlow
              .ClickOnAiElement("x");
            await mobileLoginFlow
              .ClickOnAiElement("7");
            #endregion
        }
    }
}
