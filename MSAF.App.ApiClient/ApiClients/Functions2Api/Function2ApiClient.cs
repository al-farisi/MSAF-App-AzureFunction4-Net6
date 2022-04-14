using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MSAF.App.ApiClient.ApiClients.Functions2Api
{
    public class Function2ApiClient : BaseApiClient, IFunction2ApiClient
    {
        public Function2ApiClient(
            HttpClient httpClient, 
            ILogger<Function2ApiClient> logger) : base(httpClient, logger)
        {

        }

        public async Task<Function2ApiResponse> RunFn2Api()
        {
            RequestApiModel requestApiModel = new RequestApiModel
{
                Url = "Fn2Api",
                Payload = null
            };
            var response = await PostAsync(requestApiModel);
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Function2ApiResponse>(result);
        }
    }
}
