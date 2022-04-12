using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MSAF.App.DAL.SmokeTest;
using MSAF.App.Functions.Helpers;
using MSAF.App.Functions.SmokeTest;
using MSAF.App.Services.SmokeTest;

var mapper = new MapperConfiguration(mc =>
{
    mc.AddProfile(new SmokeTestMapperProfile());
}).CreateMapper();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {
        s.AddSingleton(mapper);
        s.AddScoped<IHttpHelper, HttpHelper>();
        s.AddScoped<ISmokeTestService, SmokeTestService>();
        s.AddScoped<ISmokeTestRepo, SmokeTestRepo>();
    })
    .Build();

host.Run();