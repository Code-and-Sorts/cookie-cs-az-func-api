namespace KittyClaws.Api.Functions;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using KittyClaws.Api.Interfaces;
using KittyClaws.Api.Utils;
using Microsoft.Azure.Functions.Worker.Http;

public class CreateKittyClaws(IKittyClawsController kittyCatController, ILogger<CreateKittyClaws> logger)
{
    private readonly IKittyClawsController _kittyCatController = kittyCatController;
    private readonly ILogger<CreateKittyClaws> _logger = logger;

    [Function("CreateKittyClaws")]
    public async Task<IActionResult> Post(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "kitties")] HttpRequestData createKittyClawsRequest, CancellationToken ct = default)
    {
        _logger.LogInformation($"{nameof(CreateKittyClaws)} processed a request.");

        try
        {
            var newKittyClawsDto = await _kittyCatController.CreateAsync(createKittyClawsRequest.Body, ct);

            return new CreatedResult($"/api/kitties", newKittyClawsDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in {ClassName} -> {MethodName} method.", nameof(CreateKittyClaws), nameof(Post));

            return ErrorDetector.DetectError(ex);
        }
    }
}
