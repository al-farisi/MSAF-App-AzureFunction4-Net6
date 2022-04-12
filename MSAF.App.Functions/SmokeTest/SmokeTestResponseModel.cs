using Newtonsoft.Json;

namespace MSAF.App.Functions.SmokeTest
{
    public class SmokeTestResponseModel
    {
        [JsonProperty("data")]
        public string? Data { get; set; }

        [JsonProperty("is_repository_ok")]
        public bool IsRepositoryOK { get; set; }

        [JsonProperty("is_service_ok")]
        public bool IsServiceOK { get; set; }

        [JsonProperty("is_mapper_ok")]
        public bool IsMapperOK { get; set; }
    }
}
