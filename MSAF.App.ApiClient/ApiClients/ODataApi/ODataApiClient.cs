using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MSAF.App.ApiClient.ApiClients.ODataApi
{
    public class ODataApiClient : BaseApiClient, IODataApiClient
    {
        public ODataApiClient(
            HttpClient httpClient,
            ILogger<ODataApiClient> logger) : base(httpClient, logger)
        {

        }

        public async Task<List<ODataApiResponse>> GetWeatherForecastBySummary(string summary)
        {
            RequestApiModel requestApiModel = new RequestApiModel
            {
                Url = $"WeatherForecast?filter=summary eq '{summary}'",
                Payload = null
            };
            var response = await GetAsync(requestApiModel);
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ODataApiResponse>>(result);
        }
    }
}
