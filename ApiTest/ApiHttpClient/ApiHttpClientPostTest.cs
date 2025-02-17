using ComprehensiveAutomation.Test.ExternalApiTests.GenerateApiUserTokenTest;
using ComprehensiveAutomation.Test.Infra.BaseTest;
using ComprehensivePlayrightAuto.ApiTest.HttpService;
using NUnit.Framework;
using System.Net;
using static ComprehensiveAutomation.Test.Infra.BaseTest.EnumList;

namespace ComprehensiveAutomation.ApiTest.ApiHttpClient
{
    [TestFixture, Category(Categories.Api),
    Category(TestLevel.Level_1)]
    public class ApiHttpClientPostTest : ApiInfraTest
    {
        [Test]
        public async Task _ApiHttpClientPostTest()
        {
            string i_token = "example token";
            string email = GetTestData(configDataEnum.ApiEmail);
            string passworr = GetTestData(configDataEnum.ApiPassword);
            SetUpBaseUrl(GetTestData(configDataEnum.BaseApiUrl));

            var loginRequest = new RegisterInputDTO
            {
                email = email,
                password = passworr
            };
            var responsevalidateOtp = await HttpService
                .CallWithBody<RegisterInputDTO, RegisterOutputDTO>
              (loginRequest, new HttpCallOptionsBody("/api/register", i_token));
            Assert.That(HttpStatusCode.OK == responsevalidateOtp.HttpStatus);
            int userId = responsevalidateOtp.Result.id;
            string token = responsevalidateOtp.Result.token;

        }
    }

}
