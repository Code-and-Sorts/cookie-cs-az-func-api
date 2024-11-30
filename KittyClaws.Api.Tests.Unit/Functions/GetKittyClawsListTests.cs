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
using System.Collections.Generic;

public class GetKittyClawsListTest
{
    private readonly IKittyClawsController _mockKittyClawsController;
    private readonly ILogger<GetKittyClawsList> _mockLogger;
    private readonly GetKittyClawsList _getKittyClawsFunction;

    public GetKittyClawsListTest()
    {
        _mockKittyClawsController = Substitute.For<IKittyClawsController>();
        _mockLogger = Substitute.For<ILogger<GetKittyClawsList>>();
        _getKittyClawsFunction = new GetKittyClawsList(_mockKittyClawsController, _mockLogger);
    }

    [Fact]
    public async Task Get_ReturnsGetListResult_WhenKittyClawsListIsGot()
    {
        // Arrange
        var getListKittyClawsDto = new List<KittyClawsDto>
        {
            new KittyClawsDto { Id = "0f3a7ff7-a601-4d23-b33c-7f8f18b57a4c", Name = "mockKittyClaws1" },
            new KittyClawsDto { Id = "5615ff05-3032-4459-88ad-b6a4c3e51ca0", Name = "mockKittyClaws2" },
        };
        _mockKittyClawsController.GetListAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult((IEnumerable<KittyClawsDto>)getListKittyClawsDto));

        // Act
        var result = await _getKittyClawsFunction.Get();

        // Assert
        var getResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, getResult.StatusCode);
        Assert.Equal(getListKittyClawsDto, getResult.Value);
    }

    [Fact]
    public async Task Get_ReturnsErrorResult_WhenExceptionIsThrown()
    {
        // Arrange
        _mockKittyClawsController.GetListAsync(Arg.Any<CancellationToken>()).Throws(new Exception("Mock exception"));

        // Act
        var result = await _getKittyClawsFunction.Get();

        // Assert
        var objectResult = Assert.IsType<HttpResponseInit>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var errorResult = Assert.IsType<BaseError>(objectResult.Value);
        Assert.Equal("Mock exception", errorResult.ErrorMessage);
    }
}
