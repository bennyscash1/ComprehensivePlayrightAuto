using ComprehensivePlayrightAuto.ApiTest.HttpService;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static ComprehensiveAutomation.Test.Infra.BaseTest.Base;
using static ComprehensiveAutomation.Test.Infra.BaseTest.EnumList;

namespace ComprehensivePlayrightAuto.ApiTest.ApiDynamicService
{
    [TestFixture, Category(Categories.ApiCategory),
        Category(TestLevel.Level_1)]
    public class DynamicApiRequestTest : ApiInfraTest
    {
        [Test]
        public async Task _DynamicApiRequestTest()
        {
            SetUpBaseUrl();
            string email = GetTestData(configDataEnum.ApiEmail);
            string passworr = GetTestData(configDataEnum.ApiPassword);

            var response = await CallDynamicApi(
                    "/api/register",
                    new Dictionary<string, object>
                    {
                        { "email", "eve.holt@reqres.in1" },
                        { "password", "@pistol" }
                    },
                    expectedResponceStatus: HttpStatusCode.BadRequest

                );
            var responcex = response;
            var error = response.error;
            Console.WriteLine( error);
            //var userId = response.id;
        }

    }
}
