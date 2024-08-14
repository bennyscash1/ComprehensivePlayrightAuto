using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace ComprehensiveAutomation.Infra.HttpService
{
    public class HttpService
    {
        #region Data Members
        private readonly HttpClient m_HttpClient;

        private readonly HttpServiceOptions m_Options;
        #endregion

        #region Constructor
        public HttpService(HttpServiceOptions options)
            : this(options, TimeSpan.FromSeconds(15))
        {
        }

        public HttpService(HttpServiceOptions options, TimeSpan timeout)
        {
            m_HttpClient = new HttpClient();
            m_HttpClient.Timeout = timeout;

            m_Options = options;
        }
        #endregion

        #region Http Calls
        private HttpRequestMessage CreateRequest(HttpCallOptionsBase options, HttpMethod method)
        {
            string finalUrl = BuildUrl(options);

            var requestMessage = new HttpRequestMessage(method, finalUrl);

            foreach (KeyValuePair<string, string> headerItem in options.Headers)
            {
                requestMessage.Headers.Add(headerItem.Key, headerItem.Value);
            }

            if (!string.IsNullOrEmpty(options.Token))
            {
                requestMessage.Headers.Add("Authorization", "Bearer " + options.Token);

                //requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer ", options.Token);
            }

            return requestMessage;
        }

        private string BuildUrl(HttpCallOptionsBase options)
        {
            if (!options.IsAppended)
            {
                return options.Url;
            }

            Uri uri = new(new Uri(m_Options.BaseUrl), options.Url);

            return uri.ToString();
        }


        private async Task<HttpServiceResult<TResult>> HandleCall<TResult>(HttpRequestMessage request)
        {
            var response = await m_HttpClient.SendAsync(request);
            var data = await response?.Content.ReadAsStringAsync();
            Console.WriteLine($"Login call was successful, returned data: {data}");

            try
            {
                var dataObj = JsonConvert.DeserializeObject<TResult>(data);

                return new HttpServiceResult<TResult>(response.StatusCode, dataObj);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<HttpServiceResult<TResult>> CallWithoutBody<TResult>(HttpCallOptionsSimple options)
        {
            HttpMethod FindMethod()
            {
                switch (options.Method)
                {
                    case HttpCallMethod.Put:
                        return HttpMethod.Put;

                    case HttpCallMethod.Post:
                        return HttpMethod.Post;

                    case HttpCallMethod.Delete:
                        return HttpMethod.Delete;

                    case HttpCallMethod.Get:
                        return HttpMethod.Get;

                    default:
                        throw new Exception($"Cannot Find Method: Method {options.Method} is unknown");

                }
            }

            var method = FindMethod();

            using var request = CreateRequest(options, method);

            return await HandleCall<TResult>(request);
        }

        public async Task<HttpServiceResult<TResult>> CallWithBody<TBody, TResult>(TBody body, HttpCallOptionsBody options)
        {
            var method = options.Method == HttpMethodBody.Put
                ? HttpMethod.Put
                : HttpMethod.Post;

            using var request = CreateRequest(options, method);

            string json = JsonConvert.SerializeObject(body);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return await HandleCall<TResult>(request);
        }
        #endregion
    }
}
