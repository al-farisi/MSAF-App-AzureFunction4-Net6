using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MSAF.App.DAL.SmokeTest;
using MSAF.App.Functions.Helpers;
using MSAF.App.Services.SmokeTest;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {
        s.AddScoped<IHttpHelper, HttpHelper>();
        s.AddScoped<ISmokeTestService, SmokeTestService>();
        s.AddScoped<ISmokeTestRepo, SmokeTestRepo>();
    })
    .Build();

host.Run();