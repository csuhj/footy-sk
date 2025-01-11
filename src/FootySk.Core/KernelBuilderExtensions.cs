namespace FootySk.Core;

using System.Text.Json;
using Microsoft.SemanticKernel;

public static class KernelBuilderExtensions
{
    public static IKernelBuilder AddAzureOpenAIChatCompletionWithCredentials(this IKernelBuilder builder, string credentialsFilename)
    {
        Credentials? credentials;
        using (var reader = new StreamReader(credentialsFilename))
        {
            credentials = JsonSerializer.Deserialize<Credentials>(reader.BaseStream);
        }

        if (credentials == null)
            throw new Exception($"Got null credentials when reading {Path.GetFullPath(credentialsFilename)}");

        if (string.IsNullOrEmpty(credentials.ChatDeploymentName))
            throw new Exception($"Deployment name is not set in credentials file {Path.GetFullPath(credentialsFilename)}");

        if (string.IsNullOrEmpty(credentials.Endpoint))
            throw new Exception($"Endpoint URL is not set in credentials file {Path.GetFullPath(credentialsFilename)}");

        if (string.IsNullOrEmpty(credentials.ApiKey))
            throw new Exception($"API Key is not set in credentials file {Path.GetFullPath(credentialsFilename)}");

        // Create a kernel with Azure OpenAI chat completion
        return builder.AddAzureOpenAIChatCompletion(credentials.ChatDeploymentName, credentials.Endpoint, credentials.ApiKey);
    }

    public static IKernelBuilder AddAzureOpenAITextEmbeddingGeneration(this IKernelBuilder builder, string credentialsFilename)
    {
        Credentials? credentials;
        using (var reader = new StreamReader(credentialsFilename))
        {
            credentials = JsonSerializer.Deserialize<Credentials>(reader.BaseStream);
        }

        if (credentials == null)
            throw new Exception($"Got null credentials when reading {Path.GetFullPath(credentialsFilename)}");

        if (string.IsNullOrEmpty(credentials.EmbeddingDeploymentName))
            throw new Exception($"Deployment name is not set in credentials file {Path.GetFullPath(credentialsFilename)}");

        if (string.IsNullOrEmpty(credentials.Endpoint))
            throw new Exception($"Endpoint URL is not set in credentials file {Path.GetFullPath(credentialsFilename)}");

        if (string.IsNullOrEmpty(credentials.ApiKey))
            throw new Exception($"API Key is not set in credentials file {Path.GetFullPath(credentialsFilename)}");

        // Create a kernel with Azure OpenAI text embedding generation
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        return builder.AddAzureOpenAITextEmbeddingGeneration(credentials.EmbeddingDeploymentName, credentials.Endpoint, credentials.ApiKey);
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    }
}
