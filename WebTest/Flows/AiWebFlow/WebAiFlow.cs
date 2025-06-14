using ComprehensiveAutomation.Test.PageObject;
using ComprehensiveAutomation.Test.UiTest.Tests.Flows;
using ComprehensivePlayrightAuto.WebTest.PageObject.WebAiPages;
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
        public WebAiPages webAiPages;

        public WebAiFlow(IPage i_driver) : base(i_driver)
        {
            pDriver = i_driver;
            webAiPages = new WebAiPages(i_driver);

        }
        public async Task GetWebSingleLocatorFromUrl(string userAction)      
        {
            string xpathLocator = await webAiPages
                .GetXpathFromFullDom(userAction);

            await webAiPages
                .AiLocatorClickOnButton(xpathLocator);
        }

    }
}
