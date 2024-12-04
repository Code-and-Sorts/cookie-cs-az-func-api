namespace KittyClaws.Api.Functions;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using KittyClaws.Api.Interfaces;
using KittyClaws.Api.Utils;

public class GetKittyClawsList(IKittyClawsController kittyCatController, ILogger<GetKittyClawsList> logger)
{
    private readonly IKittyClawsController _kittyCatController = kittyCatController;
    private readonly ILogger<GetKittyClawsList> _logger = logger;

    [Function("GetKittyClawsList")]
    public async Task<IActionResult> Get(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "kitties")] CancellationToken ct = default)
    {
        _logger.LogInformation($"{nameof(GetKittyClawsList)} processed a request.");

        try
        {
            var kittyCatList = await _kittyCatController.GetListAsync(ct);

            return new OkObjectResult(kittyCatList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in {ClassName} -> {MethodName} method.", nameof(GetKittyClawsList), nameof(Get));

            return ErrorDetector.DetectError(ex);
        }
    }
}
