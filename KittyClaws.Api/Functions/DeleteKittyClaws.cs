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

public class DeleteOkObjectResult
{
    public required string Message { get; set; }
}

public class DeleteKittyClaws(IKittyClawsController kittyCatController, ILogger<DeleteKittyClaws> logger)
{
    private readonly IKittyClawsController _kittyCatController = kittyCatController;
    private readonly ILogger<DeleteKittyClaws> _logger = logger;

    [Function("DeleteKittyClaws")]
    public async Task<IActionResult> Delete(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "kitties/{id}")] HttpRequestData req, string id, CancellationToken ct = default)
    {
        _logger.LogInformation($"{nameof(DeleteKittyClaws)} processed a request.");

        try
        {
            await _kittyCatController.DeleteAsync(id, ct);

            return new OkObjectResult(new DeleteOkObjectResult
            {
                Message = $"KittyClaws with id {id} was deleted successfully."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in {ClassName} -> {MethodName} method.", nameof(DeleteKittyClaws), nameof(Delete));

            return ErrorDetector.DetectError(ex);
        }
    }
}
