namespace KittyClaws.Api.Tests.Unit;

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using KittyClaws.Api.Controllers;
using KittyClaws.Api.Dtos;
using KittyClaws.Api.Interfaces;
using KittyClaws.Api.Requests;
using Newtonsoft.Json;
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

    private static MemoryStream CreateMemoryStream<T>(T kittyCatRequest)
    {
        var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, leaveOpen: true);
        writer.Write(JsonConvert.SerializeObject(kittyCatRequest));
        writer.Flush();
        stream.Position = 0;
        return stream;
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
        await _mockKittyClawsService.Received(1).GetAsync(id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetListAsync_ReturnsListOfKittyClawsDto()
    {
        // Arrange
        var expectedKittyClawsList = new List<KittyClawsDto> { new KittyClawsDto { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c" }, new KittyClawsDto { Id = "5615ff05-3032-4459-88ad-b6a4c3e51ca0" } };
        _mockKittyClawsService.GetListAsync(Arg.Any<CancellationToken>()).Returns(expectedKittyClawsList);

        // Act
        await _kittyCatController.GetListAsync();

        // Assert
        await _mockKittyClawsService.Received(1).GetListAsync(Arg.Any<CancellationToken>());
    }

    // [Fact]
    // public async Task CreateAsync_ReturnsCreatedKittyClawsDto()
    // {
    //     // Arrange
    //     var createRequest = new CreateKittyClawsRequest { Name = "mockCreateKittyClaws", CreatedBy = null, UpdatedBy = null };
    //     var expectedKittyClaws = new KittyClawsDto { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", Name = "mockCreateKittyClaws" };
    //     _mockKittyClawsService.CreateAsync(createRequest, Arg.Any<CancellationToken>()).Returns(expectedKittyClaws);

    //     // Act
    //     var stream = CreateMemoryStream(createRequest);
    //     await _kittyCatController.CreateAsync(stream);

    //     // Assert
    //     await _mockKittyClawsService.Received(1).CreateAsync(createRequest, Arg.Any<CancellationToken>());
    // }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedKittyClawsDto()
    {
        // Arrange
        var createRequest = new CreateKittyClawsRequest { Name = "mockCreateKittyClaws", CreatedBy = "TestUser", UpdatedBy = "TestUser" };
        var expectedKittyClaws = new KittyClawsDto { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", Name = "mockCreateKittyClaws" };

        _mockKittyClawsService
            .CreateAsync(Arg.Is<CreateKittyClawsRequest>(r => r.Name == createRequest.Name), Arg.Any<CancellationToken>())
            .Returns(expectedKittyClaws);

        var stream = CreateMemoryStream(createRequest);

        // Act
        var result = await _kittyCatController.CreateAsync(stream);

        // Assert
        await _mockKittyClawsService.Received(1).CreateAsync(
            Arg.Is<CreateKittyClawsRequest>(r => r.Name == createRequest.Name), Arg.Any<CancellationToken>());
        Assert.Equal(expectedKittyClaws, result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedKittyClawsDto()
    {
        // Arrange
        var kittyCatId = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c";
        var updateRequest = new UpdateKittyClawsRequest { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", Name = "mockUpdatedKittyClaws" };
        var expectedKittyClaws = new KittyClawsDto { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", Name = "mockUpdatedKittyClaws" };

        _mockKittyClawsService
            .UpdateAsync(Arg.Is<UpdateKittyClawsRequest>(r => r.Name == updateRequest.Name), Arg.Any<CancellationToken>())
            .Returns(expectedKittyClaws);

        // Act
        var stream = CreateMemoryStream(updateRequest);
        var result = await _kittyCatController.UpdateAsync(kittyCatId, stream);

        // Assert
        await _mockKittyClawsService.Received(1).UpdateAsync(
            Arg.Is<UpdateKittyClawsRequest>(r => r.Name == updateRequest.Name), Arg.Any<CancellationToken>());
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
