namespace KittyClaws.Api.Services;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KittyClaws.Api.Dtos;
using KittyClaws.Api.Entities;
using KittyClaws.Api.Interfaces;
using KittyClaws.Api.Requests;

public class KittyClawsService : IKittyClawsService
{
    private readonly IKittyClawsRepository _kittyCatRepository;

    public KittyClawsService(IKittyClawsRepository kittyCatRepository)
    {
        _kittyCatRepository = kittyCatRepository;
    }

    public async Task<KittyClawsDto> GetAsync(string id, CancellationToken ct = default) => await _kittyCatRepository.GetAsync(id, ct);

    public async Task<IEnumerable<KittyClawsDto>> GetListAsync(CancellationToken ct = default) => await _kittyCatRepository.GetListAsync(ct);

    public async Task<KittyClawsDto> CreateAsync(CreateKittyClawsRequest kittyCat, CancellationToken ct = default)
    {
        var newKittyClaws = new KittyClaws
        {
            Id = Guid.NewGuid().ToString(),
            Name = kittyCat.Name,
        };
        return await _kittyCatRepository.CreateAsync(newKittyClaws, ct);
    }

    public async Task<KittyClawsDto> UpdateAsync(UpdateKittyClawsRequest kittyCat, CancellationToken ct = default)
    {
        var updatedKittyClaws = new KittyClaws
        {
            Id = kittyCat.Id,
            Name = kittyCat.Name,
        };
        return await _kittyCatRepository.UpdateAsync(updatedKittyClaws, ct);
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default) => await _kittyCatRepository.DeleteAsync(id, ct);
}
