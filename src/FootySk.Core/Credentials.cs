namespace FootySk.Core;

using System.Text.Json.Serialization;

public class Credentials
{
    [JsonPropertyName("deploymentName")]
    public string? DeploymentName { get; set; }

    [JsonPropertyName("endpoint")]
    public string? Endpoint { get; set; }

    [JsonPropertyName("apiKey")]
    public string? ApiKey { get; set; }
}
