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

PlayerRecord[] players = await new PlayerCsvParser("../../../../../src/FootySk.Database/all_players.csv").Parse();
Console.WriteLine($"Using database with {players.Length} players");

var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();

//Using Sqlite implementation of Vector Store connector
//https://learn.microsoft.com/en-us/semantic-kernel/concepts/vector-store-connectors/?pivots=programming-language-csharp
//Need to get vec0 extension from https://github.com/asg017/sqlite-vec/releases - see README.md
connection.LoadExtension("vec0");

// Create a Sqlite VectorStore object
var vectorStore = new SqliteVectorStore(connection);

// Choose a collection from the database and specify the type of key and record stored in it via Generic parameters.
var collection = vectorStore.GetCollection<ulong, Player>("players");

// Create the collection if it doesn't exist yet.
await collection.CreateCollectionIfNotExistsAsync();

// Create a kernel with Azure OpenAI chat completion
var builder = Kernel
    .CreateBuilder()
    .AddAzureOpenAIChatCompletionWithCredentials("../../../../../credentials.json")
    .AddAzureOpenAITextEmbeddingGeneration("../../../../../credentials.json");

// Add enterprise components
builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

// Build the kernel
Kernel kernel = builder.Build();
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
var textEmbeddingService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

//For now just update the first 10 players
foreach (var player in players.Take(10))
{
    // Create a record and generate a vector for the description using your chosen embedding generation implementation.
    await collection.UpsertAsync(await Player.Create(player, textEmbeddingService));
}

// Add a plugin (the LightsPlugin class is defined below)
kernel.Plugins.AddFromType<LightsPlugin>("Lights");

// Enable planning
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new() 
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

// Create a history store the conversation
var history = new ChatHistory();

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
