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

public class DeleteOkObjectResult
{
    public string Message { get; set; }
}

public class DeleteKittyClaws
{
    private readonly IKittyClawsController _kittyCatController;
    private readonly ILogger<DeleteKittyClaws> _logger;

    public DeleteKittyClaws(IKittyClawsController kittyCatController, ILogger<DeleteKittyClaws> logger)
    {
        _kittyCatController = kittyCatController;
        _logger = logger;
    }

    [FunctionName("DeleteKittyClaws")]
    public async Task<IActionResult> Delete(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "kitties/{id}")] string id, CancellationToken ct = default)
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
