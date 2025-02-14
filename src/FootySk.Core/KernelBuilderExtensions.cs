namespace FootySk.Core;

using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Connectors.Sqlite;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Data;

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
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
        return AddAzureOpenAITextEmbeddingGeneration(builder, credentialsFilename, out _);
    }

    public static IKernelBuilder AddAzureOpenAITextEmbeddingGeneration(this IKernelBuilder builder, string credentialsFilename, out ITextEmbeddingGenerationService service)
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
        service = new AzureOpenAITextEmbeddingGenerationService(credentials.EmbeddingDeploymentName, credentials.Endpoint, credentials.ApiKey);
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        builder.Services.AddSingleton<ITextEmbeddingGenerationService>(service);
        return builder;
    }

    public static IKernelBuilder AddSqliteVectorStore(this IKernelBuilder builder, string connectionString)
    {
        return AddSqliteVectorStore(builder, connectionString, out _, out _);
    }

    public static IKernelBuilder AddSqliteVectorStore(this IKernelBuilder builder, string connectionString, out SqliteConnection connection, out IVectorStore vectorStore)
    {
        connection = new SqliteConnection(connectionString);
        connection.Open();

        //Using Sqlite implementation of Vector Store connector
        //https://learn.microsoft.com/en-us/semantic-kernel/concepts/vector-store-connectors/?pivots=programming-language-csharp
        //Need to get vec0 extension from https://github.com/asg017/sqlite-vec/releases - see README.md
        connection.LoadExtension("vec0");

        // Create a Sqlite VectorStore object
        vectorStore = new SqliteVectorStore(connection);
        builder.Services.AddSingleton<IVectorStore>(vectorStore);

        return builder;
    }

    public static IKernelBuilder AddPlayerVectorStoreTextSearch(this IKernelBuilder builder, IVectorStore vectorStore, ITextEmbeddingGenerationService textEmbeddingService)
    {
        return AddPlayerVectorStoreTextSearch(builder, vectorStore, textEmbeddingService, out _);
    }

    public static IKernelBuilder AddPlayerVectorStoreTextSearch(this IKernelBuilder builder, IVectorStore vectorStore, ITextEmbeddingGenerationService textEmbeddingService, out VectorStoreTextSearch<Player> textSearch)
    {
        textSearch = new VectorStoreTextSearch<Player>(VectorStoreHelper.GetPlayersCollection(vectorStore), textEmbeddingService, new Player.PlayerTextSearchStringMapper(), new Player.PlayerTextSearchResultMapper());
        builder.Services.AddSingleton<VectorStoreTextSearch<Player>>(textSearch);
        return builder;
    }
}
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.