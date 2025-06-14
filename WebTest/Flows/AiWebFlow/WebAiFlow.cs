using ComprehensiveAutomation.Test.PageObject;
using ComprehensiveAutomation.Test.UiTest.Tests.Flows;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.WebTest.Flows.AiWebFlow
{
    public class WebAiFlow : BaseFlows
    {
        public IPage pDriver;
        LoginPage loginPage;

        public WebAiFlow(IPage i_driver) : base(i_driver)
        {
            pDriver = i_driver;
            loginPage = new LoginPage(i_driver);

        }
    }
}
