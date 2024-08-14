/*using ComprehensiveAutomation.Test.Infra.BaseTest;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static ComprehensiveAutomation.Test.Infra.BaseTest.EnumList;

namespace ComprehensiveAutomation.Test.ExternalApiTests.GenerateApiUserTokenTest
{
    public class ApiGetTest : BaseTest
    {
        public interface IApiGetTest
        {
            [Get("/api/users?page=2")]

            Task<GetResponceOutputDTO> APIGetTest();
        }
        #region Generate Test data

        #endregion
       // [Fact]
        [Trait(Category, Categories.Api)]
        [Trait(Category, TestLevel.Level_1)]

        public async Task _ApiGetTest()
        {
            var getRequest = RestService.For<IApiGetTest>(GetTestData(configDataEnum.BaseApiUrl)); 
            var response = await getRequest.APIGetTest();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var totalResponed = response.page;     

        }
    }
    
}
*/