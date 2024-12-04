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
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Microsoft.Azure.Functions.Worker;

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
        var httpRequestData = Mocks.CreateHttpRequestData(createKittyClawsRequest, "POST");

        _mockKittyClawsController.CreateAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(newKittyClawsDto));

        // Act
        var response = await _createKittyClawsFunction.Post(httpRequestData);

        var createdResult = Assert.IsType<CreatedResult>(response);
        var responseBody = JsonConvert.SerializeObject(createdResult.Value);
        var responseDto = JsonConvert.DeserializeObject<KittyClawsDto>(responseBody);

        // Assert
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal(newKittyClawsDto.Id, responseDto.Id);
        Assert.Equal(newKittyClawsDto.Name, responseDto.Name);
    }


    [Fact]
    public async Task Post_ReturnsErrorResult_WhenExceptionIsThrown()
    {
        // Arrange
        var createKittyClawsRequest = new CreateKittyClawsRequest { Name = "mockKittyClaws" };
        var httpRequestData = Mocks.CreateHttpRequestData(createKittyClawsRequest, "POST");

        _mockKittyClawsController.CreateAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Throws(new Exception("Mock exception"));

        // Act
        var response = await _createKittyClawsFunction.Post(httpRequestData);

        var createdResult = Assert.IsType<HttpResponseInit>(response);
        var errorResult = Assert.IsType<BaseError>(createdResult.Value);

        // Assert
        Assert.Equal(500, createdResult.StatusCode);
        Assert.Equal("Mock exception", errorResult.ErrorMessage);
    }
}
