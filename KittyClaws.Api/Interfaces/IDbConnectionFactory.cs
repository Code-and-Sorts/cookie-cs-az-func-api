namespace KittyClaws.Api.Interfaces;

using Microsoft.Azure.Cosmos;

public interface IDbConnectionFactory
{
    CosmosClient CreateClient();
}
