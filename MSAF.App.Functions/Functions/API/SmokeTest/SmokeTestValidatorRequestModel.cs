using Newtonsoft.Json;

namespace MSAF.App.Functions.SmokeTest
{
    public class SmokeTestValidatorRequestModel
    {
        [JsonProperty("string_val")]
        public string? StringVal { get; set; }

        [JsonProperty("int_val")]
        public int IntVal { get; set; }

        [JsonProperty("date_val")]
        public DateTimeOffset DateVal { get; set; }
    }
}
