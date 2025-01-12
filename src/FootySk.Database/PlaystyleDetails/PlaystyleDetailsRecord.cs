namespace FootySk.Database.PlaystyleDetails;

using System.Text.Json.Serialization;

public class PlaystyleDetailsRecord
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("info")]
    public string? Info { get; set; }

    [JsonPropertyName("playstyle")]
    public string? PLaystyle { get; set; }

    [JsonPropertyName("playstylePlus")]
    public string? PlaystylePlus { get; set; }
}