using Newtonsoft.Json;

namespace MSAF.App.ApiClient.ApiClients.ODataApi
{
    public class ODataApiResponse
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("temperatureC")]
        public int TemperatureC { get; set; }

        [JsonProperty("temperatureF")]
        public int TemperatureF { get; set; }

        [JsonProperty("summary")]
        public string? Summary { get; set; }
    }
}
