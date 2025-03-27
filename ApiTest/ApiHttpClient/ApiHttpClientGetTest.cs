using ComprehensiveAutomation.Test.ExternalApiTests.GenerateApiUserTokenTest;
using ComprehensivePlayrightAuto.ApiTest.HttpService;
using NUnit.Framework;
using System.Net;
using static ComprehensiveAutomation.Test.Infra.BaseTest.EnumList;

namespace ComprehensiveAutomation.ApiTest.ApiHttpClient
{
    [TestFixture, Category(Categories.ApiCategory),
    Category(TestLevel.Level_1)]
    public class ApiHttpClientGetTest : ApiInfraTest
    {
        [Test]
        public async Task _ApiHttpClientGetTest()
        {
            SetUpBaseUrl(GetTestData(configDataEnum.BaseApiUrl));

            var responseUserProfile = await HttpService
            .CallWithoutBody<GetResponceOutputDTO>(
                new HttpCallOptionsSimple("api/users?page=2")
                { Method = HttpCallMethod.Get });

            Assert.That(HttpStatusCode.OK == responseUserProfile.HttpStatus);
            var totalResponded = responseUserProfile.Result.page;

        }
    }

}
