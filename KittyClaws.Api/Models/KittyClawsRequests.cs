namespace KittyClaws.Api.Requests;

using System.Text.Json.Serialization;

public class CreateKittyClawsRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("createdBy")]
    public string CreatedBy { get; set; } = default!;

    [JsonPropertyName("updatedBy")]
    public string UpdatedBy { get; set; } = default!;
}

public class UpdateKittyClawsRequest
{
    [JsonIgnore]
    public string Id { get; set; } = default!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("updatedBy")]
    public string UpdatedBy { get; set; } = default!;
}
