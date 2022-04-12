using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MSAF.App.Functions.Helpers;
using MSAF.App.Services.SmokeTest;

namespace MSAF.App.Functions.SmokeTest
{
    public class SmokeTestApiFunction
    {
        private const string _basePath = "smoke-test";

        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IHttpHelper _httpHelper;
        private readonly ISmokeTestService _service;

        public SmokeTestApiFunction(
            ILoggerFactory loggerFactory,
            IMapper mapper,
            IHttpHelper httpHelper,
            ISmokeTestService service)
        {
            _logger = loggerFactory.CreateLogger<SmokeTestApiFunction>();
            _mapper = mapper;
            _httpHelper = httpHelper;
            _service = service;
        }

        [Function("SmokeTestApiFunction-AllLayer")]
        public async Task<HttpResponseData> TestAllLayer([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = _basePath + "/all-layer/{data:alpha}")] HttpRequestData req, string data)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = await _service.GetTestDataAsync(data);
            var responseModel = _mapper.Map<SmokeTestResponseModel>(response);

            var httpResponse = await _httpHelper.CreateSuccessfulHttpResponse(req, responseModel);

            return httpResponse;
        }
    }
}
