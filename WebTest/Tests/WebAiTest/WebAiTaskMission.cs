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
    public class WebAiTaskMission : PlaywrightFactory
    {
        [Test]
        public async Task _WebAiTaskMission()
        {
            string url = "https://practicetestautomation.com/practice-test-login/";
            string url2 = "file:///C:/Bennys/AutoProjects/CSharpCompAuto/ComprehensivePlayrightAuto/clickDemo.html";
            await GotoAsync(url2);
            WebAiFlow webAiFlow = new WebAiFlow(pPage);

            await webAiFlow.WebAiTaskMissionFlow("On the input field insertr 'Hello BENY', next click on 'Click here' button");

            //await webAiFlow.WebAiTaskMissionFlow("On the user name inser 'Password123', on the password field insert 'Password123' and click submit button");

        }
    }
}
