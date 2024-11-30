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

public class CreateKittyClawsTest
{
    private readonly IKittyClawsController _mockKittyClawsController;
    private readonly ILogger<CreateKittyClaws> _mockLogger;
    private readonly CreateKittyClaws _createKittyClawsFunction;

    public CreateKittyClawsTest()
    {
        _mockKittyClawsController = Substitute.For<IKittyClawsController>();
        _mockLogger = Substitute.For<ILogger<CreateKittyClaws>>();
        _createKittyClawsFunction = new CreateKittyClaws(_mockKittyClawsController, _mockLogger);
    }

    [Fact]
    public async Task Post_ReturnsCreatedResult_WhenKittyClawsIsCreated()
    {
        // Arrange
        var createKittyClawsRequest = new CreateKittyClawsRequest { Name = "mockKittyClaws" };
        var newKittyClawsDto = new KittyClawsDto { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", Name = "mockKittyClaws" };
        _mockKittyClawsController.CreateAsync(createKittyClawsRequest, Arg.Any<CancellationToken>()).Returns(Task.FromResult(newKittyClawsDto));

        // Act
        var result = await _createKittyClawsFunction.Post(createKittyClawsRequest);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal($"/api/kitties", createdResult.Location);
        Assert.Equal(newKittyClawsDto, createdResult.Value);
    }

    [Fact]
    public async Task Post_ReturnsErrorResult_WhenExceptionIsThrown()
    {
        // Arrange
        var createKittyClawsRequest = new CreateKittyClawsRequest { Name = "mockKittyClaws" };
        _mockKittyClawsController.CreateAsync(createKittyClawsRequest, Arg.Any<CancellationToken>()).Throws(new Exception("Mock exception"));

        // Act
        var result = await _createKittyClawsFunction.Post(createKittyClawsRequest);

        // Assert
        var objectResult = Assert.IsType<HttpResponseInit>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var errorResult = Assert.IsType<BaseError>(objectResult.Value);
        Assert.Equal("Mock exception", errorResult.ErrorMessage);
    }
}
