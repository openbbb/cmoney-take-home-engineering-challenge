using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Domain.Utilities
{
    public class HttpRestfulClient : IHttpRestfulClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpRestfulClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 呼叫Restful API
        /// </summary>
        /// <param name="serverUrl">Server的URL</param>
        /// <param name="apiUrl">API的路徑</param>
        /// <param name="requestObj">Request物件</param>
        /// <param name="httpMethod"></param>
        /// <param name="guid">Guid</param>
        /// <param name="logPrefix">輸出Log使用的前贅字</param>
        /// <returns></returns>
        public async Task<ClientResponse> SendRequestAsync(string serverUrl, string apiUrl, object requestObj, HttpMethod httpMethod)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var httpRequest = new HttpRequestMessage(httpMethod, $"{serverUrl}{apiUrl}");
            httpRequest.Headers.Add("Accept", "application/vnd.github.v3+json");
            if (httpMethod != HttpMethod.Get)
            {
                string json = JsonConvert.SerializeObject(requestObj);
                httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response = httpClient.SendAsync(httpRequest).GetAwaiter().GetResult();

            var result = new ClientResponse() { HttpCode = response.StatusCode.ToString(), HttpBody = "" };

            if (response.IsSuccessStatusCode)
            {
                result.HttpBody = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new System.Exception($"{apiUrl} Failed HttpState:{result.HttpCode}");
            }
            return result;
        }
    }

    public class ClientResponse
    {
        public string HttpCode { get; set; }
        public string HttpBody { get; set; }
    }
}