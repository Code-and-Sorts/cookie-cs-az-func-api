namespace KittyClaws.Api.Tests.Unit;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KittyClaws.Api.Entities;
using KittyClaws.Api.Repositories;
using Microsoft.Azure.Cosmos;
using NSubstitute;
using Xunit;

public class KittyClawsRepositoryTest
{
    private readonly Container _mockContainer;
    private readonly KittyClawsRepository _repository;

    public KittyClawsRepositoryTest()
    {
        var mockCosmosClient = Substitute.For<CosmosClient>();
        _mockContainer = Substitute.For<Container>();
        mockCosmosClient.GetContainer(Arg.Any<string>(), Arg.Any<string>()).Returns(_mockContainer);
        _repository = new KittyClawsRepository(mockCosmosClient, "mockDatabaseName", "mockContainerName");
    }

    [Fact]
    public async Task GetAsync_ShouldReturnKittyClawsDto()
    {
        // Arrange
        var kittyCat = new KittyClaws { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", Name = "mockKittyClaws" };
        var response = Substitute.For<ItemResponse<KittyClaws>>();
        response.Resource.Returns(kittyCat);
        _mockContainer.ReadItemAsync<KittyClaws>(Arg.Any<string>(), Arg.Any<PartitionKey>(), Arg.Any<ItemRequestOptions>(), Arg.Any<CancellationToken>())
                        .Returns(response);

        // Act
        var result = await _repository.GetAsync("0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", result.Id);
        Assert.Equal("mockKittyClaws", result.Name);
    }

    [Fact]
    public async Task GetListAsync_ShouldReturnListOfKittyClawsDto()
    {
        // Arrange
        var kittyCatList = new List<KittyClaws>
        {
            new() { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", Name = "mockKittyClaws1" },
            new() { Id = "5615ff05-3032-4459-88ad-b6a4c3e51ca0", Name = "mockKittyClaws2" }
        };
        var feedResponse = Substitute.For<FeedResponse<KittyClaws>>();
        feedResponse.Resource.Returns(kittyCatList);

        var feedIterator = Substitute.For<FeedIterator<KittyClaws>>();
        feedIterator.HasMoreResults.Returns(true, false);
        feedIterator.ReadNextAsync(Arg.Any<CancellationToken>()).Returns(feedResponse);

        _mockContainer.GetItemQueryIterator<KittyClaws>("SELECT * FROM c WHERE c.isDeleted = false")
            .Returns(feedIterator);

        // Act
        var result = await _repository.GetListAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.Id == "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c" && r.Name == "mockKittyClaws1");
        Assert.Contains(result, r => r.Id == "5615ff05-3032-4459-88ad-b6a4c3e51ca0" && r.Name == "mockKittyClaws2");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedKittyClawsDto()
    {
        // Arrange
        var kittyCat = new KittyClaws { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", Name = "mockKittyClaws" };
        var response = Substitute.For<ItemResponse<KittyClaws>>();
        response.Resource.Returns(kittyCat);
        _mockContainer.CreateItemAsync(Arg.Any<KittyClaws>(), Arg.Any<PartitionKey>(), Arg.Any<ItemRequestOptions>(), Arg.Any<CancellationToken>())
                        .Returns(response);

        // Act
        var result = await _repository.CreateAsync(kittyCat, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", result.Id);
        Assert.Equal("mockKittyClaws", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedKittyClawsDto()
    {
        // Arrange
        var kittyCat = new KittyClaws { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", Name = "mockKittyClawsNew", UpdatedBy = "User1" };
        var currentKittyClaws = new KittyClaws { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", Name = "mockKittyClawsOld", CreatedBy = "User2", CreatedTimestamp = DateTime.UtcNow };
        var readResponse = Substitute.For<ItemResponse<KittyClaws>>();
        readResponse.Resource.Returns(currentKittyClaws);
        var replaceResponse = Substitute.For<ItemResponse<KittyClaws>>();
        replaceResponse.Resource.Returns(kittyCat);
        _mockContainer.ReadItemAsync<KittyClaws>(Arg.Any<string>(), Arg.Any<PartitionKey>(), Arg.Any<ItemRequestOptions>(), Arg.Any<CancellationToken>())
                        .Returns(readResponse);
        _mockContainer.ReplaceItemAsync(Arg.Any<KittyClaws>(), Arg.Any<string>(), Arg.Any<PartitionKey>(), Arg.Any<ItemRequestOptions>(), Arg.Any<CancellationToken>())
                        .Returns(replaceResponse);

        // Act
        var result = await _repository.UpdateAsync(kittyCat, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", result.Id);
        Assert.Equal("mockKittyClawsNew", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallDeleteItemAsync()
    {
        // Arrange
        var mockResponse = Substitute.For<ItemResponse<KittyClaws>>();
        _mockContainer.DeleteItemAsync<KittyClaws>(
            Arg.Any<string>(),
            Arg.Any<PartitionKey>(),
            Arg.Any<ItemRequestOptions>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(mockResponse));


        // Act
        await _repository.DeleteAsync("0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", CancellationToken.None);

        // Assert
        await _mockContainer.Received(1).DeleteItemAsync<KittyClaws>("0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", new PartitionKey("0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c"), Arg.Any<ItemRequestOptions>(), Arg.Any<CancellationToken>());
    }
}
