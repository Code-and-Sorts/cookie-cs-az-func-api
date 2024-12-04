namespace KittyClaws.Api.Interfaces;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KittyClaws.Api.Dtos;

public interface IKittyClawsController
{
    Task<KittyClawsDto> GetAsync(string id, CancellationToken ct = default);

    Task<IEnumerable<KittyClawsDto>> GetListAsync(CancellationToken ct = default);

    Task<KittyClawsDto> CreateAsync(Stream item, CancellationToken ct = default);

    Task<KittyClawsDto> UpdateAsync(string id, Stream item, CancellationToken ct = default);

    Task DeleteAsync(string id, CancellationToken ct = default);
}
