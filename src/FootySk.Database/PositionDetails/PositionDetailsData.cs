namespace FootySk.Database.PositionDetails;

using System.Text.Json.Serialization;

public class PositionDetailsData
{
    [JsonPropertyName("dataSource")]
    public string? DataSource { get; set; }

    [JsonPropertyName("data")]
    public PositionDetailsRecord[]? Data { get; set; }
}