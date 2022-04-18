using AutoMapper;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MSAF.App.ApiClient.ApiClients.Functions2Api;
using MSAF.App.ApiClient.ApiClients.ODataApi;
using MSAF.App.DAL.SmokeTest;
using MSAF.App.Functions.Helpers;
using MSAF.App.Functions.SmokeTest;
using MSAF.App.Services.SmokeTest;
using MSAF.App.Utility;
using MSAF.App.Utility.NLogLayout;
using NLog;
using NLog.Extensions.Logging;
using System.Reflection;

var mapper = new MapperConfiguration(mc =>
{
    mc.AddProfile(new SmokeTestMapperProfile());
}).CreateMapper();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
    .ConfigureOpenApi()
    .ConfigureServices(s =>
    {
        s.AddOptions<AppSettings>()
        .Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.Bind(settings);
        });
        s.AddLogging((loggingBuilder) =>
        {
            var serviceProvider = loggingBuilder.Services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            //var assembly = Assembly.Load("NLog.Extensions.AzureBlobStorage");

            #region NLog setup
            LogManager.ThrowExceptions = true;
            LogManager.ThrowConfigExceptions = true;
            //LogManager.Setup()
            //     .SetupExtensions(s => s.AutoLoadAssemblies(false)
            //        //.RegisterAssembly(assembly)
            //        //.RegisterLayoutRenderer("custom-blobname", typeof(CustomBlobNameRenderer))
            //        .RegisterLayoutRenderer("custom-datetime", typeof(CustomDateTimeRenderer))
            //     )
            //     .LoadConfigurationFromSection(configuration)
            //     .LoadConfiguration(builder => builder.LogFactory.AutoShutdown = false);
            loggingBuilder.AddNLog(new NLogProviderOptions() { ShutdownOnDispose = true });
            #endregion
        });
        s.AddHttpClient<IFunction2ApiClient, Function2ApiClient>((provider, client) =>
        {
            client.BaseAddress = new Uri("http://localhost:7074/api/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        s.AddHttpClient<IODataApiClient, ODataApiClient>((provider, client) =>
        {
            client.BaseAddress = new Uri("https://localhost:7089/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        s.AddSingleton(mapper);
        s.AddScoped<IHttpHelper, HttpHelper>();
        s.AddScoped<ISmokeTestService, SmokeTestService>();
        s.AddScoped<ISmokeTestRepo, SmokeTestRepo>();
    })
    .Build();

host.Run();