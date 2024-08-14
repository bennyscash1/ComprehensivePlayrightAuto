/*using Allure.Xunit.Attributes;
using ComprehensiveAutomation.Test.Infra.BaseTest;
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
    public class ApiPostTest : BaseTest
    {
        public interface IApiPostTest
        {
            [Post("/api/register")]

            Task<ApiResponse<RegisterOutputDTO>> APIRegistration([Body] RegisterInputDTO loginRequest);
        }
        #region Generate Test data
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {              
                    new object [] { GetTestData(configDataEnum.ApiEmail), GetTestData(configDataEnum.ApiPassword), HttpStatusCode.OK },
                    new object [] { "userInvalid@gmail.com", GetTestData(configDataEnum.ApiPassword), HttpStatusCode.BadRequest },
                    new object [] { GetTestData(configDataEnum.ApiEmail), "0000", HttpStatusCode.OK }
            };
        #endregion
      //  [Theory]
        [AllureFeature("Login")]
        [AllureStory("API Login")]
        [Trait(Category, Categories.Api)]
        [Trait(Category, TestLevel.Level_1)]
        [MemberData(nameof(TestData))]      

        public async Task _ApiPostTest(string i_username, string i_password, HttpStatusCode expectedResponceStatus)
        {
            var loginApiPost = RestService.For<IApiPostTest>(GetTestData(configDataEnum.BaseApiUrl));

            var loginRequest = new RegisterInputDTO
            {
                email = i_username,
                password = i_password
            };

            var response = await loginApiPost.APIRegistration(loginRequest);
            Assert.Equal(expectedResponceStatus, response.StatusCode);
            if (response.StatusCode!=HttpStatusCode.OK)
            {
               

            }
            var dataResponed = response.Error.Content;
            var dataErrorRespones = JsonConvert.DeserializeObject<RegisterOutputDTO>(dataResponed);



        }
    }
    
}
*/