
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
[assembly: FunctionsStartup(typeof(KittyClaws.Api.Startup))]
namespace KittyClaws.Api;

using Microsoft.Extensions.Configuration;
using System;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

        builder.Services.AddApplication(configuration);
        builder.Services.AddPersistence(configuration);
    }
}
