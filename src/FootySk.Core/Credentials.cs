namespace FootySk.Core;

using System.Text.Json.Serialization;

public class Credentials
{
    [JsonPropertyName("chatDeploymentName")]
    public string? ChatDeploymentName { get; set; }

    [JsonPropertyName("embeddingDeploymentName")]
    public string? EmbeddingDeploymentName { get; set; }

    [JsonPropertyName("endpoint")]
    public string? Endpoint { get; set; }

    [JsonPropertyName("apiKey")]
    public string? ApiKey { get; set; }
}
