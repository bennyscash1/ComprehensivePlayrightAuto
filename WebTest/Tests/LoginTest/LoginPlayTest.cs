using ComprehensiveAutomation.Test.UiTest.BaseData;
using ComprehensiveAutomation.Test.UiTest.Tests.Flows;

using static ComprehensiveAutomation.Test.Infra.BaseTest.EnumList;
using NUnit.Framework;
using Microsoft.Playwright;
using System.Runtime.Intrinsics.Arm;
using ComprehensiveAutomation.WebTest.IntialWeb;
namespace ComprehensiveAutomation.Test.UiTest.Tests.LoginTest
{
    [TestFixture, Category(Categories.UiWeb), 
        Category(TestLevel.Level_1)]

    public class LoginWebPlay : PlaywrightFactory
    {
        string m_url = GetTestData(configDataEnum.WebUrl);
        string WebUserName = GetTestData(configDataEnum.WebUserName);
        string WebPassword = GetTestData(configDataEnum.WebPassword);
        [SetUp]
        public void Setup()
        {
         
        }

        [Test]

        public async Task LoginWebTest()
        {
            await GotoAsync(m_url);

            LoginFlow loginFlow = new LoginFlow(pPage);
            await loginFlow
                .BoValidLoginAsync(WebUserName, WebPassword);
            bool isHomePage = await loginFlow.IsHomePageDisplay();
            Assert.That(isHomePage, Is.True);

        }
    }
}
