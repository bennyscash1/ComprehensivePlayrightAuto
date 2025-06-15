using ComprehensiveAutomation.WebTest.IntialWeb;
using ComprehensivePlayrightAuto.WebTest.Flows.AiWebFlow;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ComprehensiveAutomation.Test.Infra.BaseTest.EnumList;

namespace ComprehensivePlayrightAuto.WebTest.Tests.WebAiTest
{
    [TestFixture, Category(Categories.AiWebTest),
    Category(TestLevel.Level_1)]
    public class AiTestCase : PlaywrightFactory
    {
        [Test]
        public async Task _AiTestCase()
        {
            await GotoAsync("https://practicetestautomation.com/practice-test-login/");
            WebAiFlow webAiFlow = new WebAiFlow(pPage);

            await webAiFlow
                .WebAiAction("User name input field", "student");
            await webAiFlow
                .WebAiAction("Password input field", "Password123");
            await webAiFlow.WebAiAction("Submit button");

        }
    }
}
