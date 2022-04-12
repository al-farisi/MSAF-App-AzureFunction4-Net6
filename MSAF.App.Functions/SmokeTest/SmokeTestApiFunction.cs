using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MSAF.App.Functions.Helpers;
using MSAF.App.Services.SmokeTest;
using MSAF.App.Utility;

namespace MSAF.App.Functions.SmokeTest
{
    public class SmokeTestApiFunction : BaseFunctions
    {
        private const string _basePath = "smoke-test";

        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IHttpHelper _httpHelper;
        private readonly ISmokeTestService _service;

        protected readonly AppSettings _appSettings;

        public SmokeTestApiFunction(
            IOptions<AppSettings> options,
            ILoggerFactory loggerFactory,
            IMapper mapper,
            IHttpHelper httpHelper,
            ISmokeTestService service) : base(options)
        {
            _appSettings = options.Value;
            _logger = loggerFactory.CreateLogger<SmokeTestApiFunction>();
            _mapper = mapper;
            _httpHelper = httpHelper;
            _service = service;
        }

        [Function("SmokeTestApiFunction-AllLayer")]
        public async Task<HttpResponseData> TestAllLayer([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = _basePath + "/all-layer/{data:alpha}")] HttpRequestData req, string data)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var response = await _service.GetTestDataAsync(data);
                var responseModel = _mapper.Map<SmokeTestResponseModel>(response);
                responseModel.AppName = _appSettings.AppName;

                //var httpResponse = await _httpHelper.CreateSuccessfulHttpResponse(req, responseModel);
                //return httpResponse;

                return await HandleSuccess(req, responseModel);
            }
            catch (Exception ex)
            {
                return await HandleException(req, ex.Message);
            }
        }
    }
}
