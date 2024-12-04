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

public class GetKittyClaws(IKittyClawsController kittyCatController, ILogger<GetKittyClaws> logger)
{
    private readonly IKittyClawsController _kittyCatController = kittyCatController;
    private readonly ILogger<GetKittyClaws> _logger = logger;

    [Function("GetKittyClaws")]
    public async Task<IActionResult> Get(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "kitties/{id}")] HttpRequestData req, string id, CancellationToken ct = default)
    {
        _logger.LogInformation($"{nameof(GetKittyClaws)} processed a request.");

        try
        {
            var kittyCat = await _kittyCatController.GetAsync(id, ct);

            return new OkObjectResult(kittyCat);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in {ClassName} -> {MethodName} method.", nameof(GetKittyClaws), nameof(Get));

            return ErrorDetector.DetectError(ex);
        }
    }
}
