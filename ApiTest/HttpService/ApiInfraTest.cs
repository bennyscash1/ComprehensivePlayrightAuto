using ComprehensiveAutomation.Test.Infra.BaseTest;
using NUnit.Framework;
using System.Dynamic;
using System.Net;

namespace ComprehensivePlayrightAuto.ApiTest.HttpService
{
    public class ApiInfraTest : BaseTest
    {
        protected HttpService HttpService { get; private set; }
        public void SetUpBaseUrl(string defultUrl ="" )
        {
            if (string.IsNullOrEmpty(defultUrl))
            {
                defultUrl = GetTestData(configDataEnum.BaseApiUrl);
            }
            HttpService = new HttpService(
                new HttpServiceOptions
                {
                    BaseUrl = defultUrl
                }
             );
        }
        public async Task<dynamic> CallDynamicApi(string endpoint,
            Dictionary<string, object> body = null, 
            string token = null, 
            HttpCallMethod method = HttpCallMethod.Post, 
            HttpStatusCode expectedResponceStatus = HttpStatusCode.OK)
        {
            var options = new HttpCallOptionsBody(endpoint, token);

            if (body == null)
            {
                var response = await HttpService.CallWithoutBody<ExpandoObject>(
                       new HttpCallOptionsSimple(endpoint, token)
                       {
                           Method = method
                       }
                   );

                Assert.That(response.HttpStatus == expectedResponceStatus, 
                    response.BodyString);
                return response.Result;
            }
            else
            {
                var response = await HttpService
                    .CallWithBody<Dictionary<string, object>, 
                    ExpandoObject>(body, options, method);

                Assert.That(response.HttpStatus == expectedResponceStatus, 
                    response.BodyString);
                return response.Result;
            }
        }

    }
}
