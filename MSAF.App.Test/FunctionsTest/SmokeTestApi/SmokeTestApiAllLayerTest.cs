using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
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
    public class SmokeTestApiAllLayerTest
    {
        private const string _appName = "MSAF.App.Test";

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

            var logger = (ListLogger)TestFactory.CreateLogger(LoggerTypes.List);

            var mockLoggerFactory = new Mock<ILoggerFactory>();

            var mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new SmokeTestMapperProfile());
            }).CreateMapper();

            var mockAppSettings = new Mock<IOptions<AppSettings>>();
            var appSettings = new AppSettings()
            {
                AppName = _appName
            };
            mockAppSettings.Setup(p => p.Value).Returns(appSettings);

            var mockSmokeTestService = new Mock<ISmokeTestService>();
            mockSmokeTestService.Setup(s => s.GetTestDataAsync(data)).ReturnsAsync(serviceResult);

            SmokeTestApiFunction fn = new SmokeTestApiFunction(mockAppSettings.Object, mockLoggerFactory.Object, mapper, mockSmokeTestService.Object);

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
    }
}
