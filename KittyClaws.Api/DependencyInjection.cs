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
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IKittyClawsService, KittyClawsService>();
        services.AddScoped<IKittyClawsController, KittyClawsController>();

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string cosmosConnectionString = configuration.GetConnectionString("CosmosDb") ?? string.Empty;
        string databaseName = configuration.GetValue<string>("CosmosDbDatabaseName") ?? string.Empty;
        string containerName = configuration.GetValue<string>("CosmosDbContainerName") ?? string.Empty;

        if (string.IsNullOrEmpty(cosmosConnectionString) || string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(containerName))
        {
            throw new InvalidOperationException("CosmosDb configuration is missing or incomplete.");
        }

        services.AddSingleton(provider =>
            new CosmosClient(cosmosConnectionString)
        );

        services.AddSingleton<IKittyClawsRepository>(provider =>
        {
            var cosmosClient = provider.GetService<CosmosClient>();
            if (cosmosClient == null)
            {
                throw new InvalidOperationException("CosmosDb Client is null.");
            }
            return new KittyClawsRepository(
                cosmosClient,
                databaseName,
                containerName
            );
        });

        return services;
    }
}
