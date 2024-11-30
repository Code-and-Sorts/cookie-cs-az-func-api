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

public class GetKittyClawsTest
{
    private readonly IKittyClawsController _mockKittyClawsController;
    private readonly ILogger<GetKittyClaws> _mockLogger;
    private readonly GetKittyClaws _getKittyClawsFunction;

    public GetKittyClawsTest()
    {
        _mockKittyClawsController = Substitute.For<IKittyClawsController>();
        _mockLogger = Substitute.For<ILogger<GetKittyClaws>>();
        _getKittyClawsFunction = new GetKittyClaws(_mockKittyClawsController, _mockLogger);
    }

    [Fact]
    public async Task Get_ReturnsGetResult_WhenKittyClawsIsGot()
    {
        // Arrange
        var kittyCatId = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c";
        var getKittyClawsDto = new KittyClawsDto { Id = kittyCatId, Name = "mockKittyClaws" };
        _mockKittyClawsController.GetAsync(kittyCatId, Arg.Any<CancellationToken>()).Returns(Task.FromResult(getKittyClawsDto));

        // Act
        var result = await _getKittyClawsFunction.Get(kittyCatId);

        // Assert
        var getResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, getResult.StatusCode);
        Assert.Equal(getKittyClawsDto, getResult.Value);
    }

    [Fact]
    public async Task Get_ReturnsErrorResult_WhenExceptionIsThrown()
    {
        // Arrange
        var kittyCatId = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c";
        _mockKittyClawsController.GetAsync(kittyCatId, Arg.Any<CancellationToken>()).Throws(new Exception("Mock exception"));

        // Act
        var result = await _getKittyClawsFunction.Get(kittyCatId);

        // Assert
        var objectResult = Assert.IsType<HttpResponseInit>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var errorResult = Assert.IsType<BaseError>(objectResult.Value);
        Assert.Equal("Mock exception", errorResult.ErrorMessage);
    }
}
