using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Connectors.Sqlite;
using FootySk.Core;
using FootySk.Database;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.SemanticKernel.Embeddings;

string executablePath = Path.GetDirectoryName(System.Environment.ProcessPath);
string rootPath = Path.GetFullPath("../../../../..", executablePath);

var connection = new SqliteConnection($"Data Source={Path.Combine(rootPath, "FootySkVectorStore.db")}");
connection.Open();

//Using Sqlite implementation of Vector Store connector
//https://learn.microsoft.com/en-us/semantic-kernel/concepts/vector-store-connectors/?pivots=programming-language-csharp
//Need to get vec0 extension from https://github.com/asg017/sqlite-vec/releases - see README.md
connection.LoadExtension("vec0");

// Create a Sqlite VectorStore object
var vectorStore = new SqliteVectorStore(connection);

string credentialsPath = Path.Combine(rootPath, "credentials.json");
// Create a kernel with Azure OpenAI chat completion andf text embeddings generation
var builder = Kernel
    .CreateBuilder()
    .AddAzureOpenAIChatCompletionWithCredentials(credentialsPath)
    .AddAzureOpenAITextEmbeddingGeneration(credentialsPath);

// Add enterprise components
builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

// Build the kernel
Kernel kernel = builder.Build();

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
var textEmbeddingService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

VectorStoreHelper.PopulatePlayersVectorStore(vectorStore, textEmbeddingService, rootPath);

// Add a plugin (the LightsPlugin class is defined below)
// Add in vector text search - see https://github.com/microsoft/semantic-kernel/tree/main/dotnet/samples/Demos/VectorStoreRAG
// Create new plugin to register players in a team?
kernel.Plugins.AddFromType<LightsPlugin>("Lights");

// Enable planning
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new() 
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

// Create a history store the conversation
var history = new ChatHistory();

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Initiate a back-and-forth chat
string? userInput;
do {
    // Collect user input
    Console.Write("User > ");
    userInput = Console.ReadLine();

    // Add user input
    history.AddUserMessage(userInput);

    // Get the response from the AI
    var result = await chatCompletionService.GetChatMessageContentAsync(
        history,
        executionSettings: openAIPromptExecutionSettings,
        kernel: kernel);

    // Print the results
    Console.WriteLine("Assistant > " + result);

    // Add the message from the agent to the chat history
    history.AddMessage(result.Role, result.Content ?? string.Empty);
} while (userInput is not null);
