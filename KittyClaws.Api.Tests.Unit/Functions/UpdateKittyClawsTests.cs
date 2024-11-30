namespace KittyClaws.Api.Tests.Unit;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using KittyClaws.Api.Functions;
using KittyClaws.Api.Interfaces;
using KittyClaws.Api.Requests;
using KittyClaws.Api.Dtos;
using NSubstitute.ExceptionExtensions;
using KittyClaws.Api.Utils;

public class UpdateKittyClawsTest
{
    private readonly IKittyClawsController _mockKittyClawsController;
    private readonly ILogger<UpdateKittyClaws> _mockLogger;
    private readonly UpdateKittyClaws _updateKittyClawsFunction;

    public UpdateKittyClawsTest()
    {
        _mockKittyClawsController = Substitute.For<IKittyClawsController>();
        _mockLogger = Substitute.For<ILogger<UpdateKittyClaws>>();
        _updateKittyClawsFunction = new UpdateKittyClaws(_mockKittyClawsController, _mockLogger);
    }

    [Fact]
    public async Task Patch_ReturnsUpdatedResult_WhenKittyClawsIsUpdated()
    {
        // Arrange
        var kittyCatId = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c";
        var updateKittyClawsRequest = new UpdateKittyClawsRequest { Name = "mockKittyClaws" };
        var newKittyClawsDto = new KittyClawsDto { Id = kittyCatId, Name = "mockKittyClaws" };
        _mockKittyClawsController.UpdateAsync(updateKittyClawsRequest, Arg.Any<CancellationToken>()).Returns(Task.FromResult(newKittyClawsDto));

        // Act
        var result = await _updateKittyClawsFunction.Patch(updateKittyClawsRequest, kittyCatId);

        // Assert
        var updatedResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, updatedResult.StatusCode);
        Assert.Equal(newKittyClawsDto, updatedResult.Value);
    }

    [Fact]
    public async Task Patch_ReturnsErrorResult_WhenExceptionIsThrown()
    {
        // Arrange
        var kittyCatId = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c";
        var updateKittyClawsRequest = new UpdateKittyClawsRequest { Name = "mockKittyClaws" };
        _mockKittyClawsController.UpdateAsync(updateKittyClawsRequest, Arg.Any<CancellationToken>()).Throws(new Exception("Mock exception"));

        // Act
        var result = await _updateKittyClawsFunction.Patch(updateKittyClawsRequest, kittyCatId);

        // Assert
        var objectResult = Assert.IsType<HttpResponseInit>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var errorResult = Assert.IsType<BaseError>(objectResult.Value);
        Assert.Equal("Mock exception", errorResult.ErrorMessage);
    }
}