using AutoMapper;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MSAF.App.DAL.SmokeTest;
using MSAF.App.Functions.Helpers;
using MSAF.App.Functions.SmokeTest;
using MSAF.App.Services.SmokeTest;
using MSAF.App.Utility;

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
        s.AddSingleton(mapper);
        s.AddScoped<IHttpHelper, HttpHelper>();
        s.AddScoped<ISmokeTestService, SmokeTestService>();
        s.AddScoped<ISmokeTestRepo, SmokeTestRepo>();
    })
    .Build();

host.Run();