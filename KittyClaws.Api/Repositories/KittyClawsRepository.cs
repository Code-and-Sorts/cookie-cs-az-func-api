namespace KittyClaws.Api.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KittyClaws.Api.Dtos;
using KittyClaws.Api.Entities;
using KittyClaws.Api.Interfaces;
using Microsoft.Azure.Cosmos;

public class KittyClawsRepository : IKittyClawsRepository
{
    private readonly Container _container;

    public KittyClawsRepository(CosmosClient cosmosClient, string databaseName, string containerName)
    {
        _container = cosmosClient.GetContainer(databaseName, containerName);
    }

    private async Task<KittyClaws> GetItemAsync(string id, CancellationToken ct)
    {
        var response = await _container.ReadItemAsync<KittyClaws>(id, new PartitionKey(id), null, ct);
        var kittyCat = response.Resource;

        if (kittyCat.IsDeleted)
        {
            throw new CosmosException("Item not found", System.Net.HttpStatusCode.NotFound, 0, string.Empty, 0);
        }
        return kittyCat;
    }

    public async Task<KittyClawsDto> GetAsync(string id, CancellationToken ct)
    {
        var kittyCat = await GetItemAsync(id, ct);

        return new KittyClawsDto
        {
            Id = kittyCat.Id,
            Name = kittyCat.Name,
        };
    }

    public async Task<IEnumerable<KittyClawsDto>> GetListAsync(CancellationToken ct)
    {
        var query = _container.GetItemQueryIterator<KittyClaws>("SELECT * FROM c WHERE c.isDeleted = false");
        var results = new List<KittyClaws>();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync(ct);
            results.AddRange(response.Resource);
        }

        var kittyCatDtos = results.Select(kittyCat => new KittyClawsDto
        {
            Id = kittyCat.Id,
            Name = kittyCat.Name,
        });

        return kittyCatDtos;
    }

    public async Task<KittyClawsDto> CreateAsync(KittyClaws kittyCat, CancellationToken ct)
    {
        var response = await _container.CreateItemAsync(kittyCat, new PartitionKey(kittyCat.Id), null, ct);
        var kittyCatDto = response.Resource;
        return new KittyClawsDto
        {
            Id = kittyCatDto.Id,
            Name = kittyCatDto.Name,
        };
    }

    public async Task<KittyClawsDto> UpdateAsync(KittyClaws item, CancellationToken ct)
    {
        var currentItem = await GetItemAsync(item.Id, ct);
        var updateItem = new KittyClaws
        {
            Id = currentItem.Id,
            Name = item.Name ?? currentItem.Name,
            CreatedBy = currentItem.CreatedBy,
            CreatedTimestamp = currentItem.CreatedTimestamp,
            UpdatedBy = item.UpdatedBy ?? currentItem.UpdatedBy,
            UpdatedTimestamp = DateTime.UtcNow,
        };
        var response = await _container.ReplaceItemAsync<KittyClaws>(updateItem, updateItem.Id, new PartitionKey(updateItem.Id), null, ct);
        var kittyCatResponse = response.Resource;
        return new KittyClawsDto
        {
            Id = kittyCatResponse.Id,
            Name = kittyCatResponse.Name,
        };
    }

    public async Task DeleteAsync(string id, CancellationToken ct)
    {
        var item = await GetItemAsync(id, ct);
        item.IsDeleted = true;
        var response = await _container.ReplaceItemAsync<KittyClaws>(item, item.Id, new PartitionKey(item.Id), null, ct);
    }
}
