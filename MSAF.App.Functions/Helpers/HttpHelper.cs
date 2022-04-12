using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using System.Net;

namespace MSAF.App.Functions.Helpers
{
    public class HttpHelper : IHttpHelper
    {
        public async Task<HttpResponseData> CreateSuccessfulHttpResponse(HttpRequestData req, object data)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync(JsonConvert.SerializeObject(data));

            return response;
        }
    }
}
