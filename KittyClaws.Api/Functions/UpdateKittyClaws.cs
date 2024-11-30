namespace KittyClaws.Api.Functions;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using KittyClaws.Api.Interfaces;
using KittyClaws.Api.Requests;
using KittyClaws.Api.Utils;

public class UpdateKittyClaws
{
    private readonly IKittyClawsController _kittyCatController;
    private readonly ILogger<UpdateKittyClaws> _logger;

    public UpdateKittyClaws(IKittyClawsController kittyCatController, ILogger<UpdateKittyClaws> logger)
    {
        _kittyCatController = kittyCatController;
        _logger = logger;
    }

    [FunctionName("UpdateKittyClaws")]
    public async Task<IActionResult> Patch(
        [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "kitties/{id}")] UpdateKittyClawsRequest updateKittyClawsRequest, string id, CancellationToken ct = default)
    {
        _logger.LogInformation($"{nameof(UpdateKittyClaws)} processed a request.");

        try
        {
            updateKittyClawsRequest.Id = id;
            var updatedKittyClawsDto = await _kittyCatController.UpdateAsync(updateKittyClawsRequest, ct);

            return new OkObjectResult(updatedKittyClawsDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in {ClassName} -> {MethodName} method.", nameof(UpdateKittyClaws), nameof(Patch));

            return ErrorDetector.DetectError(ex);
        }
    }
}
