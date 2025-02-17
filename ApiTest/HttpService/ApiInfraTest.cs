using ComprehensiveAutomation.Test.Infra.BaseTest;

namespace ComprehensivePlayrightAuto.ApiTest.HttpService
{
    public class ApiInfraTest : BaseTest
    {
        protected HttpService HttpService { get; private set; }
        public void SetUpBaseUrl(string defultUrl  )
        {
            HttpService = new HttpService(
                new HttpServiceOptions
                {
                    BaseUrl = defultUrl
                }
             );
        }
    }
}
