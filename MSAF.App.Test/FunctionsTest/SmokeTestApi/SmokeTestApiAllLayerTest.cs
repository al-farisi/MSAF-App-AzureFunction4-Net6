using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MSAF.App.ApiClient.ApiClients.Functions2Api;
using MSAF.App.ApiClient.ApiClients.ODataApi;
using MSAF.App.DAL.SmokeTest;
using MSAF.App.Functions.Functions.API.SmokeTest;
using MSAF.App.Functions.SmokeTest;
using MSAF.App.Services.SmokeTest;
using MSAF.App.TestHelpers;
using MSAF.App.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MSAF.App.Test.FunctionsTest.SmokeTestApi
{
    public class SmokeTestApiSetup
    {
        public Mock<IOptions<AppSettings>> MockAppSettings { get; set; }
        public Mock<ILoggerFactory> MockLoggerFactory { get; set; }
        public Mock<ISmokeTestService> MockSmokeTestService { get; set; }
        public Mock<IFunction2ApiClient> MockFunction2ApiClient { get; set; }
        public Mock<IODataApiClient> MockODataApiClient { get; set; }
        public IMapper Mapper { get; set; }
    }

    public class SmokeTestApiAllLayerTest
    {
        private const string _appName = "MSAF.App.Test";

        private SmokeTestApiSetup GetSetup()
        {
            var mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new SmokeTestMapperProfile());
            }).CreateMapper();

            var logger = (ListLogger)TestFactory.CreateLogger(LoggerTypes.List);

            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory.Setup(f => f.CreateLogger(It.IsAny<string>())).Returns(logger);

            var mockAppSettings = new Mock<IOptions<AppSettings>>();
            var appSettings = new AppSettings()
            {
                AppName = _appName
            };
            mockAppSettings.Setup(p => p.Value).Returns(appSettings);

            var mockSmokeTestService = new Mock<ISmokeTestService>();

            var mockApiClient = new Mock<IFunction2ApiClient>();

            var mockODataApiClient = new Mock<IODataApiClient>();

            return new SmokeTestApiSetup()
            {
                MockLoggerFactory = mockLoggerFactory,
                MockAppSettings = mockAppSettings,
                MockFunction2ApiClient = mockApiClient,
                MockSmokeTestService = mockSmokeTestService,
                MockODataApiClient = mockODataApiClient,
                Mapper = mapper,
            };
        }

        [Fact]
        public async Task Function1_ValidData_ReturnOk()
        {
            // Arrange
            var body = new MemoryStream(Encoding.ASCII.GetBytes("{ \"test\": true }"));
            var context = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(
                            context.Object,
                            new Uri("https://stackoverflow.com"),
                            body);
            var mockLoggerFactory = new Mock<ILoggerFactory>();

            // Act
            var function = new Function1(mockLoggerFactory.Object);
            var result = function.Run(request);
            result.Body.Position = 0;

            // Assert
            var reader = new StreamReader(result.Body);
            var responseBody = await reader.ReadToEndAsync();
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Welcome to Azure Functions!", responseBody);
        }

        [Fact]
        public async Task SmokeTestApiAllLayerTest_ValidData_ReturnOk()
        {
            const string data = "abcd";
            const string token = "1234556";

            // arrange
            IEnumerable<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Token", token)
            };
            var httpHeaders = new HttpHeadersCollection(headers);
            var body = new MemoryStream(Encoding.ASCII.GetBytes("{ \"test\": true }"));
            var context = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(
                            context.Object,
                            new Uri("https://stackoverflow.com"),
                            body,
                            httpHeaders);

            var serviceResult = new SmokeTestData()
            {
                Data = data,
                IsRepositoryOK = true,
                IsServiceOK = true
            };
            var expectedResult = new SmokeTestResponseModel()
            {
                AppName = _appName,
                Data = data,
                IsMapperOK = true,
                IsRepositoryOK = true,
                IsServiceOK = true,
                Token = token
            };

            var setup = GetSetup();
            setup.MockSmokeTestService.Setup(s => s.GetTestDataAsync(data)).ReturnsAsync(serviceResult);
            setup.MockFunction2ApiClient.Setup(s => s.RunFn2Api()).ReturnsAsync(new Function2ApiResponse() { IsFnOK = true });

            SmokeTestApiFunction fn = new SmokeTestApiFunction(
                setup.MockAppSettings.Object, 
                setup.MockLoggerFactory.Object, 
                setup.Mapper, 
                setup.MockSmokeTestService.Object, 
                setup.MockFunction2ApiClient.Object,
                setup.MockODataApiClient.Object);

            //act
            var result = await fn.TestAllLayer(request, data);
            result.Body.Position = 0;

            //assert
            var reader = new StreamReader(result.Body);
            var responseBody = await reader.ReadToEndAsync();
            var responseBodyObj = JsonConvert.DeserializeObject<SmokeTestResponseModel>(responseBody);

            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedResult.Data, responseBodyObj.Data);
            Assert.Equal(expectedResult.Token, responseBodyObj.Token);
            Assert.Equal(expectedResult.AppName, responseBodyObj.AppName);
            Assert.Equal(expectedResult.IsServiceOK, responseBodyObj.IsServiceOK);
            Assert.Equal(expectedResult.IsRepositoryOK, responseBodyObj.IsRepositoryOK);
            Assert.Equal(expectedResult.IsMapperOK, responseBodyObj.IsMapperOK);
        }

        [Fact]
        public async Task SmokeTestApiOverApiClient_ValidData_ReturnOk()
        {
            // arrange
            var context = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(
                            context.Object,
                            new Uri("https://stackoverflow.com"));

            var setup = GetSetup();
            setup.MockFunction2ApiClient.Setup(s => s.RunFn2Api()).ReturnsAsync(new Function2ApiResponse() { IsFnOK = true });

            SmokeTestApiFunction fn = new SmokeTestApiFunction(
                setup.MockAppSettings.Object,
                setup.MockLoggerFactory.Object,
                setup.Mapper,
                setup.MockSmokeTestService.Object,
                setup.MockFunction2ApiClient.Object,
                setup.MockODataApiClient.Object);

            //act
            var result = await fn.TestOverApiClient(request);
            result.Body.Position = 0;

            //assert
            var reader = new StreamReader(result.Body);
            var responseBody = await reader.ReadToEndAsync();
            var responseBodyObj = JsonConvert.DeserializeObject<Function2ApiResponse>(responseBody);

            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.True(responseBodyObj.IsFnOK);
        }

        [Fact]
        public async Task SmokeTestApiGetWeatherForecastODataBySummary_ValidData_ReturnOk()
        {
            // arrange
            var context = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(
                            context.Object,
                            new Uri("https://stackoverflow.com"));
            string summary = "Hot";

            var expectedResults = new List<ODataApiResponse>();
            var result1 = new ODataApiResponse()
            {
                Date = DateTime.Now,
                Summary = summary,
                TemperatureC = 30,
                TemperatureF = 86
            };
            expectedResults.Add(result1);

            var setup = GetSetup();
            setup.MockODataApiClient.Setup(s => s.GetWeatherForecastBySummary(summary)).ReturnsAsync(expectedResults);

            SmokeTestApiFunction fn = new SmokeTestApiFunction(
                setup.MockAppSettings.Object,
                setup.MockLoggerFactory.Object,
                setup.Mapper,
                setup.MockSmokeTestService.Object,
                setup.MockFunction2ApiClient.Object,
                setup.MockODataApiClient.Object);

            //act
            var result = await fn.GetWeatherForecastODataBySummary(request, summary);
            result.Body.Position = 0;

            //assert
            var reader = new StreamReader(result.Body);
            var responseBody = await reader.ReadToEndAsync();
            var responseBodyObj = JsonConvert.DeserializeObject<List<ODataApiResponse>>(responseBody);

            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.True(responseBodyObj.Count == 1);
            Assert.Equal(result1.Date, responseBodyObj[0].Date);
            Assert.Equal(result1.Summary, responseBodyObj[0].Summary);
            Assert.Equal(result1.TemperatureC, responseBodyObj[0].TemperatureC);
            Assert.Equal(result1.TemperatureF, responseBodyObj[0].TemperatureF);
        }
    }
}
