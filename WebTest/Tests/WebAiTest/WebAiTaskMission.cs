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
            await GotoAsync("file:///C:/Bennys/AutoProjects/CSharpCompAuto/ComprehensivePlayrightAuto/clickDemo.html");
            WebAiFlow webAiFlow = new WebAiFlow(pPage);

    
            await webAiFlow
                .WebAiTaskMissionFlow("Enter name 'Benny shor' and next click on 'click here' button");

        }
    }
}
