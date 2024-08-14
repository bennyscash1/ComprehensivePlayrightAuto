using ComprehensiveAutomation.Test.Infra.BaseTest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ComprehensiveAutomation.Test.Infra.BaseTest.EnumList;
using ComprehensiveAutomation.Test.ExternalApiTests.GenerateApiUserTokenTest;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using ComprehensiveAutomation.Infra.HttpService;
using NUnit.Framework;

namespace ComprehensiveAutomation.ApiTest.LoadTest
{
    [TestFixture, Category(Categories.Api),
    Category(TestLevel.Level_1)]
    public class ParrallelGetRequestTest : BaseTest
    {
        [Test]

        public async Task _ParrallelGetRequestTest()
        {
            int threadNum = 2;
            int maxThreadInParallel = 1;
            string fileName = GenerateUniqueKey(3) + "_LoadTest_";
            string dateFormat = "yyyy-MM-dd HH:mm:ss.ff";


            var stopWatch = Stopwatch.StartNew();
            WriteToFile(fileName, "Get load start at: "
            + DateTime.Now.ToString(dateFormat)
            + "\nThread loop is: " + threadNum
            + "\nMax threads in parallel is: " + maxThreadInParallel);

            #region create transaction loop
            await Parallel.ForEachAsync(
                Enumerable.Range(1, threadNum),
                new ParallelOptions { MaxDegreeOfParallelism = maxThreadInParallel },
                async (index, _) =>
                {
                    HttpService m_httpServiceGetGroup = new HttpService(
                   new HttpServiceOptions { BaseUrl = GetTestData(configDataEnum.BaseApiUrl) }, TimeSpan.FromSeconds(3));
                    var m_responceGetRequest = await m_httpServiceGetGroup
                        .CallWithoutBody<RegisterOutputDTO>(
                      new HttpCallOptionsSimple("get") { Method = HttpCallMethod.Get });

                });
            #endregion

            #region summery test report 
            DateTime endTestTime = DateTime.Now;
            WriteToFile(fileName, "The transaction ended at: "
                + endTestTime
                + "\nTotal test time: " + stopWatch.Elapsed);
            #endregion

        }
    }
}
