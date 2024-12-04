namespace KittyClaws.Api.Interfaces;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KittyClaws.Api.Dtos;
using KittyClaws.Api.Entities;

public interface IKittyClawsRepository
{
    Task<KittyClawsDto> GetAsync(string id, CancellationToken ct = default);

    Task<IEnumerable<KittyClawsDto>> GetListAsync(CancellationToken ct = default);

    Task<KittyClawsDto> CreateAsync(KittyClaws kittyCat, CancellationToken ct = default);

    Task<KittyClawsDto> UpdateAsync(KittyClaws kittyCat, CancellationToken ct = default);

    Task DeleteAsync(string id, CancellationToken ct = default);
}
