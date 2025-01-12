namespace FootySk.Database.PositionDetails;

using System.Text.Json.Serialization;

public class PositionDetailsRecord
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("abbreviation")]
    public string? Abbreviation { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}