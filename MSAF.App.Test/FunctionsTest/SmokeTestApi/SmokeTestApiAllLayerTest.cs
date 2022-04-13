using AutoMapper;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MSAF.App.DAL.SmokeTest;
using MSAF.App.Functions.SmokeTest;
using MSAF.App.Services.SmokeTest;
using MSAF.App.TestHelpers;
using MSAF.App.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MSAF.App.Test.FunctionsTest.SmokeTestApi
{
    public class SmokeTestApiAllLayerTest
    {
        private const string _appName = "MSAF.App.Test";

        [Fact]
        public async Task SmokeTestApiAllLayerTest_ValidData_ReturnOk()
        {
            const string data = "abcd";

            // arrange
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
                Token = "1234556"
            };

            var httpRequest = TestFactory.CreateHttpRequest(null, null, null);
            //var request = new HttpRequestData("GET", new Uri("http://localhost:7071/api/smoke-test/all-layer/aaaaa"));
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

            //SmokeTestApiFunction fn = new SmokeTestApiFunction(mockAppSettings.Object, mockLoggerFactory.Object, mapper, mockSmokeTestService.Object);

            //act
            //var result = await fn.TestAllLayer(request, data);
        }
    }
}
