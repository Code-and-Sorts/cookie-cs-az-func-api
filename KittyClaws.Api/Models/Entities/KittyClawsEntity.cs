namespace KittyClaws.Api.Entities;

using Newtonsoft.Json;

public class KittyClaws : BaseEntity
{
    [JsonProperty("name")]
    public string Name { get; set; } = default!;
}
