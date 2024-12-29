using ComprehensiveAutomation.Test.Infra.BaseTest;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Refit;
using static ComprehensiveAutomation.Test.Infra.BaseTest.EnumList;
using ComprehensiveAutomation.Test.ExternalApiTests.GenerateApiUserTokenTest;
using System.Net;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;
using ComprehensivePlayrightAuto.ApiTest.HttpService;

namespace ComprehensiveAutomation.ApiTest.ApiInit
{
    [TestFixture, Category(Categories.Api),
    Category(TestLevel.Level_1)]
    public class ApiHttpClientGetTest : BaseTest
    {
        [Test]

        public async Task _ApiHttpClientGetTest()
        {   
            HttpService m_httpService = new HttpService(
            new HttpServiceOptions { BaseUrl = GetTestData(configDataEnum.BaseApiUrl) });

            var m_responceUserProfile = await m_httpService
                .CallWithoutBody<GetResponceOutputDTO>
                    (new HttpCallOptionsSimple("api/users?page=2") 
                    { Method = HttpCallMethod.Get });

            Assert.That(HttpStatusCode.OK== m_responceUserProfile.HttpStatus);
            var totalResponed = m_responceUserProfile.Result.page;

        }
    }

}
