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
using KittyClaws.Api.Dtos;
using NSubstitute.ExceptionExtensions;
using KittyClaws.Api.Utils;

public class DeleteKittyClawsTest
{
    private readonly IKittyClawsController _mockKittyClawsController;
    private readonly ILogger<DeleteKittyClaws> _mockLogger;
    private readonly DeleteKittyClaws _deleteKittyClawsFunction;

    public DeleteKittyClawsTest()
    {
        _mockKittyClawsController = Substitute.For<IKittyClawsController>();
        _mockLogger = Substitute.For<ILogger<DeleteKittyClaws>>();
        _deleteKittyClawsFunction = new DeleteKittyClaws(_mockKittyClawsController, _mockLogger);
    }

    [Fact]
    public async Task Delete_ReturnsDeleteResult_WhenKittyClawsIsDeleted()
    {
        // Arrange
        var kittyCatId = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c";
        var deleteKittyClawsDto = new KittyClawsDto { Id = kittyCatId, Name = "mockKittyClaws" };
        var httpRequestData = Mocks.CreateHttpRequestData(deleteKittyClawsDto, "DELETE");

        _mockKittyClawsController.DeleteAsync(kittyCatId, Arg.Any<CancellationToken>()).Returns(Task.FromResult(deleteKittyClawsDto));

        // Act
        var result = await _deleteKittyClawsFunction.Delete(httpRequestData, kittyCatId);

        var deleteResult = Assert.IsType<OkObjectResult>(result);
        var errorResult = Assert.IsType<DeleteOkObjectResult>(deleteResult.Value);

        // Assert
        Assert.Equal(200, deleteResult.StatusCode);
        Assert.Equal("KittyClaws with id 0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c was deleted successfully.", errorResult.Message);
    }

    [Fact]
    public async Task Delete_ReturnsErrorResult_WhenExceptionIsThrown()
    {
        // Arrange
        var kittyCatId = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c";
        var deleteKittyClawsDto = new KittyClawsDto { Id = kittyCatId, Name = "mockKittyClaws" };
        var httpRequestData = Mocks.CreateHttpRequestData(deleteKittyClawsDto, "DELETE");

        _mockKittyClawsController.DeleteAsync(kittyCatId, Arg.Any<CancellationToken>()).Throws(new Exception("Mock exception"));

        // Act
        var result = await _deleteKittyClawsFunction.Delete(httpRequestData, kittyCatId);

        var objectResult = Assert.IsType<HttpResponseInit>(result);
        var errorResult = Assert.IsType<BaseError>(objectResult.Value);

        // Assert
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Mock exception", errorResult.ErrorMessage);
    }
}
