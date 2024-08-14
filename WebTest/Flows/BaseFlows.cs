using OpenQA.Selenium;
using ComprehensiveAutomation.Test.Infra.BaseTest;
using ComprehensiveAutomation.Test.PageObject;
using System.ComponentModel;
using Microsoft.Playwright;

namespace ComprehensiveAutomation.Test.UiTest.Tests.Flows
{
    public class BaseFlows 
    {
        public IWebDriver m_driver;
        public IPage pDriver ;

        public BasePages basePages;


        public BaseFlows(IPage i_driver)
        {
            this.pDriver = i_driver;
            basePages = new BasePages(i_driver);
        }

        public string GetCurrentUrl()
        {
            return m_driver.Url;
        }
        public void NavigateToUrl(string i_url)
        {
            m_driver.Navigate().GoToUrl(i_url);
        }

    }
}
