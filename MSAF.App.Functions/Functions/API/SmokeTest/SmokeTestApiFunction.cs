using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MSAF.App.Functions.Helpers;
using MSAF.App.Services.SmokeTest;
using MSAF.App.Utility;
using Newtonsoft.Json;
using System.Net;

namespace MSAF.App.Functions.SmokeTest
{
    public class SmokeTestApiFunction : BaseFunctions
    {
        private const string _basePath = "smoke-test";

        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        //private readonly IHttpHelper _httpHelper;
        private readonly ISmokeTestService _service;

        protected readonly AppSettings _appSettings;

        public SmokeTestApiFunction(
            IOptions<AppSettings> options,
            ILoggerFactory loggerFactory,
            IMapper mapper,
            //IHttpHelper httpHelper,
            ISmokeTestService service) : base(options)
        {
            _appSettings = options.Value;
            _logger = loggerFactory.CreateLogger<SmokeTestApiFunction>();
            _mapper = mapper;
            //_httpHelper = httpHelper;
            _service = service;
        }

        [Function("SmokeTestApiFunction-AllLayer")]
        [OpenApiOperation(operationId: "SmokeTestApiFunction-AllLayer", tags: new[] { _basePath + "/all-layer" })]
        [OpenApiParameter(name: "Token", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "Token to test the functions")]
        [OpenApiParameter(name: "data", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Data to test the functions")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "OK response")]
        public async Task<HttpResponseData> TestAllLayer([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = _basePath + "/all-layer/{data:alpha}")] HttpRequestData req, string data)
        {
            //_logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var headers = req.Headers;

                var response = await _service.GetTestDataAsync(data);
                var responseModel = _mapper.Map<SmokeTestResponseModel>(response);
                responseModel.AppName = _appSettings.AppName;
                responseModel.Token = headers.TryGetValues("Token", out var token) ? token.FirstOrDefault() : string.Empty;
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
