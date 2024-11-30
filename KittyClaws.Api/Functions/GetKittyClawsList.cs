namespace KittyClaws.Api.Functions;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using KittyClaws.Api.Interfaces;
using KittyClaws.Api.Utils;

public class GetKittyClawsList
{
    private readonly IKittyClawsController _kittyCatController;
    private readonly ILogger<GetKittyClawsList> _logger;

    public GetKittyClawsList(IKittyClawsController kittyCatController, ILogger<GetKittyClawsList> logger)
    {
        _kittyCatController = kittyCatController;
        _logger = logger;
    }

    [FunctionName("GetKittyClawsList")]
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
