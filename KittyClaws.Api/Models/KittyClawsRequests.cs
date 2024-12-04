namespace KittyClaws.Api.Requests;

using Newtonsoft.Json;

public class CreateKittyClawsRequest
{
    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("createdBy")]
    public string CreatedBy { get; set; } = default!;

    [JsonProperty("updatedBy")]
    public string UpdatedBy { get; set; } = default!;
}

public class UpdateKittyClawsRequest
{
    [JsonIgnore]
    public string Id { get; set; } = default!;

    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("updatedBy")]
    public string UpdatedBy { get; set; } = default!;
}
