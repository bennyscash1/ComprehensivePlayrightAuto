﻿using ComprehensiveAutomation.Test.UiTest.MobileTest.MobileFlows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComprehensiveAutomation.MobileTest.Inital;
using static ComprehensiveAutomation.Test.Infra.BaseTest.EnumList;
using NUnit.Framework;

namespace ComprehensiveAutomation.MobileTest.MobileTest
{
    [TestFixture, Category(Categories.MobileAndroid),
    Category(TestLevel.Level_1)]
    public class MobilaBasicTest :MobileDriverFactory
    {
        [Test]
        public void _MobilaBasicTest()
        {
            string contactPerson = GetTestData(configDataEnum.ContactName);
            string contactPhone = GetTestData(configDataEnum.ContactNumber);

            MobileLoginFlow mobileLoginFlow = new MobileLoginFlow(appiumDriver);

            #region Insert phone number
   
            mobileLoginFlow
                .MobileGivePermissionAndOpenAccountFrame();
            #endregion

            #region init home page popups
            bool isCloseIconDisplay=
                mobileLoginFlow
                .isCloseIconDisplay();
            Assert.That(isCloseIconDisplay);
            #endregion
        }
    }
}
