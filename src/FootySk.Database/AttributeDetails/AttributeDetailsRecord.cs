namespace FootySk.Database.AttributeDetails;

using System.Text.Json.Serialization;

public class AttributeDetailsRecord
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("weighting")]
    public string? Weighting { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}