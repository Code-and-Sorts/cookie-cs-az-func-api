namespace KittyClaws.Api.Controllers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KittyClaws.Api.Dtos;
using KittyClaws.Api.Interfaces;
using KittyClaws.Api.Requests;

public class KittyClawsController : IKittyClawsController
{
    private readonly IKittyClawsService _kittyCatService;

    public KittyClawsController(IKittyClawsService kittyCatService)
    {
        _kittyCatService = kittyCatService;
    }

    public async Task<KittyClawsDto> GetAsync(string id, CancellationToken ct = default) => await _kittyCatService.GetAsync(id, ct);

    public async Task<IEnumerable<KittyClawsDto>> GetListAsync(CancellationToken ct = default) => await _kittyCatService.GetListAsync(ct);

    public async Task<KittyClawsDto> CreateAsync(CreateKittyClawsRequest kittyCat, CancellationToken ct = default)
    {
        // TODO: Add validation
        return await _kittyCatService.CreateAsync(kittyCat, ct);
    }

    public async Task<KittyClawsDto> UpdateAsync(UpdateKittyClawsRequest kittyCat, CancellationToken ct = default)
    {
        // TODO: Add validation
        return await _kittyCatService.UpdateAsync(kittyCat, ct);
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default) => await _kittyCatService.DeleteAsync(id, ct);
}
