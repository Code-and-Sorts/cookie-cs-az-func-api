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
using KittyClaws.Api.Controllers;

public class CreateKittyClaws
{
    private readonly IKittyClawsController _kittyCatController;
    private readonly ILogger<CreateKittyClaws> _logger;

    public CreateKittyClaws(IKittyClawsController kittyCatController, ILogger<CreateKittyClaws> logger)
    {
        _kittyCatController = kittyCatController;
        _logger = logger;
    }

    [FunctionName("CreateKittyClaws")]
    public async Task<IActionResult> Post(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "kitties")] CreateKittyClawsRequest createKittyClawsRequest, CancellationToken ct = default)
    {
        _logger.LogInformation($"{nameof(CreateKittyClaws)} processed a request.");

        try
        {
            var newKittyClawsDto = await _kittyCatController.CreateAsync(createKittyClawsRequest, ct);

            return new CreatedResult($"/api/kitties", newKittyClawsDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in {ClassName} -> {MethodName} method.", nameof(CreateKittyClaws), nameof(Post));

            return ErrorDetector.DetectError(ex);
        }
    }
}
