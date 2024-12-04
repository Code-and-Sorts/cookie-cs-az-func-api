namespace KittyClaws.Api.Controllers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using KittyClaws.Api.Dtos;
using KittyClaws.Api.Interfaces;
using KittyClaws.Api.Requests;
using KittyClaws.Api.Validation;
using Newtonsoft.Json;

public class KittyClawsController : IKittyClawsController
{
    private readonly IKittyClawsService _kittyCatService;

    public KittyClawsController(IKittyClawsService kittyCatService)
    {
        _kittyCatService = kittyCatService;
    }

    private static async Task<T> DeserializeRequestBodyAsync<T>(Stream body)
    {
        using var reader = new StreamReader(body);
        var bodyString = await reader.ReadToEndAsync();
        var result = JsonConvert.DeserializeObject<T>(bodyString) ?? throw new JsonSerializationException("Deserialization returned null.");
        return result;
    }

    public async Task<KittyClawsDto> GetAsync(string id, CancellationToken ct = default) => await _kittyCatService.GetAsync(id, ct);

    public async Task<IEnumerable<KittyClawsDto>> GetListAsync(CancellationToken ct = default) => await _kittyCatService.GetListAsync(ct);

    public async Task<KittyClawsDto> CreateAsync(Stream kittyCat, CancellationToken ct = default)
    {
        var deserializedRequest = await DeserializeRequestBodyAsync<CreateKittyClawsRequest>(kittyCat);
        var validator = new CreateKittyClawsRequestValidator();
        await validator.ValidateAndThrowAsync(deserializedRequest, ct);
        return await _kittyCatService.CreateAsync(deserializedRequest, ct);
    }

    public async Task<KittyClawsDto> UpdateAsync(string id, Stream kittyCat, CancellationToken ct = default)
    {
        var deserializedRequest = await DeserializeRequestBodyAsync<UpdateKittyClawsRequest>(kittyCat);
        deserializedRequest.Id = id;
        var validator = new UpdateKittyClawsRequestValidator();
        await validator.ValidateAndThrowAsync(deserializedRequest, ct);
        return await _kittyCatService.UpdateAsync(deserializedRequest, ct);
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default) => await _kittyCatService.DeleteAsync(id, ct);
}
