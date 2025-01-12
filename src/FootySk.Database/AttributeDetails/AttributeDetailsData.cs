namespace FootySk.Database.AttributeDetails;

using System.Text.Json.Serialization;

public class AttributeDetailsData
{
    [JsonPropertyName("dataSource")]
    public string? DataSource { get; set; }

    [JsonPropertyName("data")]
    public AttributeDetailsRecord[]? Data { get; set; }
}