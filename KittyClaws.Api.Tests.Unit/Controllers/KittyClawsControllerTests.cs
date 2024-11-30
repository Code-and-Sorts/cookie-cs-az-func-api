namespace KittyClaws.Api.Tests.Unit;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KittyClaws.Api.Controllers;
using KittyClaws.Api.Dtos;
using KittyClaws.Api.Interfaces;
using KittyClaws.Api.Requests;
using NSubstitute;
using Xunit;

public class KittyClawsControllerTest
{
    private readonly IKittyClawsService _mockKittyClawsService;
    private readonly KittyClawsController _kittyCatController;

    public KittyClawsControllerTest()
    {
        _mockKittyClawsService = Substitute.For<IKittyClawsService>();
        _kittyCatController = new KittyClawsController(_mockKittyClawsService);
    }

    [Fact]
    public async Task GetAsync_ReturnsKittyClawsDto()
    {
        // Arrange
        var id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c";
        var expectedKittyClaws = new KittyClawsDto { Id = id };
        _mockKittyClawsService.GetAsync(id, Arg.Any<CancellationToken>()).Returns(expectedKittyClaws);

        // Act
        var result = await _kittyCatController.GetAsync(id);

        // Assert
        Assert.Equal(expectedKittyClaws, result);
    }

    [Fact]
    public async Task GetListAsync_ReturnsListOfKittyClawsDto()
    {
        // Arrange
        var expectedKittyClawsList = new List<KittyClawsDto> { new KittyClawsDto { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c" }, new KittyClawsDto { Id = "5615ff05-3032-4459-88ad-b6a4c3e51ca0" } };
        _mockKittyClawsService.GetListAsync(Arg.Any<CancellationToken>()).Returns(expectedKittyClawsList);

        // Act
        var result = await _kittyCatController.GetListAsync();

        // Assert
        Assert.Equal(expectedKittyClawsList, result);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedKittyClawsDto()
    {
        // Arrange
        var createRequest = new CreateKittyClawsRequest { Name = "mockCreateKittyClaws" };
        var expectedKittyClaws = new KittyClawsDto { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", Name = "mockCreateKittyClaws" };
        _mockKittyClawsService.CreateAsync(createRequest, Arg.Any<CancellationToken>()).Returns(expectedKittyClaws);

        // Act
        var result = await _kittyCatController.CreateAsync(createRequest);

        // Assert
        Assert.Equal(expectedKittyClaws, result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedKittyClawsDto()
    {
        // Arrange
        var updateRequest = new UpdateKittyClawsRequest { Id = "1", Name = "mockUpdatedKittyClaws" };
        var expectedKittyClaws = new KittyClawsDto { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", Name = "mockUpdatedKittyClaws" };
        _mockKittyClawsService.UpdateAsync(updateRequest, Arg.Any<CancellationToken>()).Returns(expectedKittyClaws);

        // Act
        var result = await _kittyCatController.UpdateAsync(updateRequest);

        // Assert
        Assert.Equal(expectedKittyClaws, result);
    }

    [Fact]
    public async Task DeleteAsync_CallsDeleteOnService()
    {
        // Arrange
        var id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c";
        _mockKittyClawsService.DeleteAsync(id, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        // Act
        await _kittyCatController.DeleteAsync(id);

        // Assert
        await _mockKittyClawsService.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
    }
}
