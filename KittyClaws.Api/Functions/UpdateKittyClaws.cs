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

public class UpdateKittyClaws(IKittyClawsController kittyCatController, ILogger<UpdateKittyClaws> logger)
{
    private readonly IKittyClawsController _kittyCatController = kittyCatController;
    private readonly ILogger<UpdateKittyClaws> _logger = logger;

    [Function("UpdateKittyClaws")]
    public async Task<IActionResult> Patch(
        [HttpTrigger(AuthorizationLevel.Function, "patch", Route = "kitties/{id}")] HttpRequestData updateKittyClawsRequest, string id, CancellationToken ct = default)
    {
        _logger.LogInformation($"{nameof(UpdateKittyClaws)} processed a request.");

        try
        {
            var updatedKittyClawsDto = await _kittyCatController.UpdateAsync(id, updateKittyClawsRequest.Body, ct);

            return new OkObjectResult(updatedKittyClawsDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in {ClassName} -> {MethodName} method.", nameof(UpdateKittyClaws), nameof(Patch));

            return ErrorDetector.DetectError(ex);
        }
    }
}
