using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;
using MSAF.App.Utility;
using MSAF.App.Utility.CommonModels;
using Newtonsoft.Json;
using System.Net;

namespace MSAF.App.Functions
{
    public class BaseFunctions
    {
        protected readonly AppSettings _appSettings;
        public BaseFunctions(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        protected async Task<HttpResponseData> HandleSuccess(HttpRequestData req, object data)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonConvert.SerializeObject(data));

            return response;
        }

        protected async Task<HttpResponseData> HandleException(HttpRequestData req, string errorMessage)
        {
            var data = new ErrorResponse()
            {
                ErrorMessage = errorMessage
            };

            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonConvert.SerializeObject(data));

            return response;
        }
    }
}
