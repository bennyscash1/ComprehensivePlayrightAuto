using OpenQA.Selenium;
using ComprehensiveAutomation.Test.UiTest.MobileTest.MobilePageObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace ComprehensiveAutomation.Test.UiTest.MobileTest.MobileFlows
{
    public class MobileLoginFlow : MobileBaseFlow
    {
        MobileLoginPage mobileLoginPageObject;
        public MobileLoginFlow(AndroidDriver i_driver) : base(i_driver)
        {
            appiumDriver = i_driver;
            mobileLoginPageObject = new MobileLoginPage(appiumDriver);
        }
/*        public MobileLoginFlow MobileGivePermissionAndOpenAccountFrame()
        {
            mobileLoginPageObject
                .ClickOnApprovePopupDialogMessage();
            mobileLoginPageObject
                .ClickOnApprovePopupDialogMessage();

            mobileLoginPageObject
                .ClickOnAccountIcon();
            return this;
        }*/
        public MobileLoginFlow ClickOnCalFlwo(string calNum)
        {
            mobileLoginPageObject             
                .ClickOnCalculator(calNum);
            return this;    
        }

    }
}
