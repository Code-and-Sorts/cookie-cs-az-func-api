namespace KittyClaws.Api.Tests.Unit;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KittyClaws.Api.Dtos;
using KittyClaws.Api.Entities;
using KittyClaws.Api.Interfaces;
using KittyClaws.Api.Requests;
using KittyClaws.Api.Services;
using NSubstitute;
using Xunit;

public class KittyClawsServiceTest
{
    private readonly IKittyClawsRepository _kittyCatRepositoryMock;
    private readonly KittyClawsService _kittyCatService;

    public KittyClawsServiceTest()
    {
        _kittyCatRepositoryMock = Substitute.For<IKittyClawsRepository>();
        _kittyCatService = new KittyClawsService(_kittyCatRepositoryMock);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnKittyClawsDto()
    {
        // Arrange
        var kittyCatId = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c";
        var expectedKittyClaws = new KittyClawsDto { Id = kittyCatId, Name = "mockKittyClaws" };
        _kittyCatRepositoryMock.GetAsync(kittyCatId, Arg.Any<CancellationToken>())
            .Returns(expectedKittyClaws);

        // Act
        var result = await _kittyCatService.GetAsync(kittyCatId);

        // Assert
        Assert.Equal(expectedKittyClaws, result);
    }

    [Fact]
    public async Task GetListAsync_ShouldReturnListOfKittyClawsDto()
    {
        // Arrange
        var expectedKittyClawsList = new List<KittyClawsDto>
        {
            new KittyClawsDto { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", Name = "mockKittyClaws1" },
            new KittyClawsDto { Id = "5615ff05-3032-4459-88ad-b6a4c3e51ca0", Name = "mockKittyClaws2" }
        };
        _kittyCatRepositoryMock.GetListAsync(Arg.Any<CancellationToken>())
            .Returns(expectedKittyClawsList);

        // Act
        var result = await _kittyCatService.GetListAsync();

        // Assert
        Assert.Equal(expectedKittyClawsList, result);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedKittyClawsDto()
    {
        // Arrange
        var createRequest = new CreateKittyClawsRequest { Name = "mockCreateKittyClaws" };
        var newKittyClaws = new KittyClaws { Id = Guid.NewGuid().ToString(), Name = createRequest.Name };
        var expectedKittyClaws = new KittyClawsDto { Id = newKittyClaws.Id, Name = newKittyClaws.Name };
        _kittyCatRepositoryMock.CreateAsync(Arg.Any<KittyClaws>(), Arg.Any<CancellationToken>())
            .Returns(expectedKittyClaws);

        // Act
        var result = await _kittyCatService.CreateAsync(createRequest);

        // Assert
        Assert.Equal(expectedKittyClaws, result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedKittyClawsDto()
    {
        // Arrange
        var updateRequest = new UpdateKittyClawsRequest { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", Name = "mockUpdateKittyClaws" };
        var updatedKittyClaws = new KittyClaws { Id = updateRequest.Id, Name = updateRequest.Name };
        var expectedKittyClaws = new KittyClawsDto { Id = updatedKittyClaws.Id, Name = updatedKittyClaws.Name };
        _kittyCatRepositoryMock.UpdateAsync(Arg.Any<KittyClaws>(), Arg.Any<CancellationToken>())
            .Returns(expectedKittyClaws);

        // Act
        var result = await _kittyCatService.UpdateAsync(updateRequest);

        // Assert
        Assert.Equal(expectedKittyClaws, result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepositoryDelete()
    {
        // Arrange
        var kittyCatId = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c";
        _kittyCatRepositoryMock.DeleteAsync(kittyCatId, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await _kittyCatService.DeleteAsync(kittyCatId);

        // Assert
        await _kittyCatRepositoryMock.Received(1).DeleteAsync(kittyCatId, Arg.Any<CancellationToken>());
    }
}
