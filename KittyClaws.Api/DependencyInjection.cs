namespace KittyClaws.Api;

using System;
using KittyClaws.Api.Controllers;
using KittyClaws.Api.Interfaces;
using KittyClaws.Api.Repositories;
using KittyClaws.Api.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddScoped<IKittyClawsService, KittyClawsService>();
        services.AddScoped<IKittyClawsController, KittyClawsController>();

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string cosmosConnectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString");
        string databaseName = Environment.GetEnvironmentVariable("CosmosDbDatabaseName");
        string containerName = Environment.GetEnvironmentVariable("CosmosDbContainerName");

        services.AddSingleton<CosmosClient>(provider =>
            new CosmosClient(cosmosConnectionString));

        services.AddSingleton<IKittyClawsRepository>(provider =>
            new KittyClawsRepository(
                provider.GetRequiredService<CosmosClient>(),
                databaseName,
                containerName
            ));

        return services;
    }
}
