using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace MSAF.App.Functions_2
{
    public class Functions2Api
    {
        private readonly ILogger _logger;

        public Functions2Api(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Functions2Api>();
        }

        [Function("Fn2Api")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var data = new { IsFnOK = true };
            
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(data);

            return response;
        }
    }
}
