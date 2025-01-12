namespace FootySk.Database.PlaystyleDetails;

using System.Text.Json.Serialization;

public class PlaystyleDetailsData
{
    [JsonPropertyName("dataSource")]
    public string? DataSource { get; set; }

    [JsonPropertyName("data")]
    public PlaystyleDetailsRecord[]? Data { get; set; }
}