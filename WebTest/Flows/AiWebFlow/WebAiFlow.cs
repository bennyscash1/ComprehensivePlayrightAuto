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
        public WebAiActionPages webAiPages;

        public WebAiFlow(IPage i_driver) : base(i_driver)
        {
            pDriver = i_driver;
            webAiPages = new WebAiActionPages(i_driver);

        }
        public async Task WebAiAction(string userAction , string userInput ="")      
        {
            string xpathLocator = await webAiPages
                .GetXpathFromFullDom(userAction);
            if (string.IsNullOrEmpty(userInput))
            {
                await webAiPages
                    .AiLocatorClickOnButton(xpathLocator);
            }
            
         
            if (!string.IsNullOrEmpty(userInput))
            {
                await webAiPages
                    .AiLocatorInputAction(xpathLocator, userInput);
            }
        }
        public async Task<int> WebAiTaskMissionFlow(string userTaskMission)
        {
            WebAiTaskPages webAiTaskPages = new WebAiTaskPages(pDriver);
            int jsonResponceAi = await webAiTaskPages
                .GetXpathFromDomAccordingToUserTaks(userTaskMission);

            //here will be the input or click according to the return json type
            return jsonResponceAi;
        }

    }
}
