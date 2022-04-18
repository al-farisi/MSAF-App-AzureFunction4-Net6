using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace MSAF.App.ApiClient.ApiClients
{
    public class BaseApiClient
    {
        protected readonly HttpClient _httpClient;

        public ILogger Logger { get; set; }
        public BaseApiClient(
            HttpClient httpClient,
            ILogger logger)
        {
            _httpClient = httpClient;
            Logger = logger;
        }

        protected async Task<HttpResponseMessage> PostAsync(RequestApiModel requestApi, Dictionary<string, string> customHeaders = null)
        {
            return await ProcessAsync(HttpMethod.Post, requestApi, customHeaders);
        }

        protected async Task<HttpResponseMessage> GetAsync(RequestApiModel requestApi, Dictionary<string, string> customHeaders = null)
        {
            return await  ProcessAsync(HttpMethod.Get, requestApi, customHeaders);
        }

        private async Task<HttpResponseMessage> ProcessAsync(HttpMethod method, RequestApiModel requestApi, Dictionary<string, string> customHeaders = null)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(requestApi.Payload), Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage(method, requestApi.Url)
            {
                Content = stringContent
            };

            if (customHeaders != null && customHeaders.Count >= 0)
            {
                foreach (var customHeader in customHeaders)
                {
                    request.Headers.Add(customHeader.Key, customHeader.Value);
                }
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response;
        }
    }
}
