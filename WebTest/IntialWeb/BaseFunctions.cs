using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Drawing;

namespace ComprehensiveAutomation.UiTest.BaseData
{
    public class BaseFunctions
    {

        public IWebDriver m_driver;
        public int timeWait = 6000;
        public TimeSpan timeOutInSeconds = TimeSpan.FromSeconds(6);
        public By m_okButonInPopupWindowBy = By.XPath("//button[normalize-space()='Ok']");
        public IPage _page;
        
        // Assuming a setup method to initialize the Playwright and page objects
        #region playright
        public BaseFunctions (IPage i_driver)
        {
            _page = i_driver;    
        }

        public async Task<bool> IsElementXpathFoundAsync(string selector)
        {
            try
            {
                await _page.WaitForSelectorAsync($"xpath={selector}", new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 5000
                });

                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }


        public async Task ClickAsync(string selector)
        {
            bool elementVisible = false;
            if (await IsElementXpathFoundAsync(selector) == true)
            {
                await IsElementXpathFoundAsync(selector);
                await _page.ClickAsync($"xpath={selector}");
                elementVisible = true;
            }
            Assert.That(elementVisible);
        }
        public async Task FuillTextAsync(string selector, string input)
        {
            bool elementVisible = false;
            if (await IsElementXpathFoundAsync(selector) == true)
            {
                await IsElementXpathFoundAsync(selector);
                await _page.FillAsync($"xpath={selector}", input);
                elementVisible = true;
            }
            Assert.That(elementVisible);
        }
        #endregion

    }
}
