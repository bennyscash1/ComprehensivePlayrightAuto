using ComprehensiveAutomation.Test.Infra.BaseTest;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ComprehensiveAutomation.Test.UiTest.MobileTest.MobilePageObject
{
    public class MobileLoginPage : MobileBasePages
    {
        private By m_accountIconBy = By.XPath("//android.widget.ImageView[@resource-id='com.google.android.contacts:id/og_apd_internal_image_view']");
        private By m_closeIconBy = By.Id("com.google.android.contacts:id/og_header_close_button");

        public MobileLoginPage(AndroidDriver i_driver) : base(i_driver)
        {
            appiumDriver = i_driver;
        }

        public MobileLoginPage ClickOnAccountIcon()
        {
            MobileClickElement(m_accountIconBy);
            return this;
        }
    
        public bool isCloseIconDisplay()
        {
            WaitForElement(m_closeIconBy);
            return true;
        }
    }
}
