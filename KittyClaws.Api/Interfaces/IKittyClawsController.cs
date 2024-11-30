namespace KittyClaws.Api.Interfaces;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KittyClaws.Api.Dtos;
using KittyClaws.Api.Requests;

public interface IKittyClawsController
{
    Task<KittyClawsDto> GetAsync(string id, CancellationToken ct = default);

    Task<IEnumerable<KittyClawsDto>> GetListAsync(CancellationToken ct = default);

    Task<KittyClawsDto> CreateAsync(CreateKittyClawsRequest item, CancellationToken ct = default);

    Task<KittyClawsDto> UpdateAsync(UpdateKittyClawsRequest item, CancellationToken ct = default);

    Task DeleteAsync(string id, CancellationToken ct = default);
}
