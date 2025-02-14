using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using FootySk.Core;

string executablePath = Path.GetDirectoryName(Environment.ProcessPath);
string rootPath = Path.GetFullPath("../../../../..", executablePath);

string credentialsPath = Path.Combine(rootPath, "credentials.json");
string vectorStoreConnectionString = $"Data Source={Path.Combine(rootPath, "FootySkVectorStore.db")}";

// Create a kernel with Azure OpenAI chat completion, text embeddings generation and SQLite vector store
var builder = Kernel
    .CreateBuilder()
    .AddAzureOpenAIChatCompletionWithCredentials(credentialsPath)
    .AddAzureOpenAITextEmbeddingGeneration(credentialsPath, out var textEmbeddingService)
    .AddSqliteVectorStore(vectorStoreConnectionString, out var sqliteConnection, out var vectorStore);

// Add enterprise components
builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

await VectorStoreHelper.PopulateAttributeDataVectorStore(vectorStore, textEmbeddingService, rootPath);
await VectorStoreHelper.PopulatePositionDataVectorStore(vectorStore, textEmbeddingService, rootPath);
await VectorStoreHelper.PopulatePlaystyleDataVectorStore(vectorStore, textEmbeddingService, rootPath);

Dictionary<string, string> positionAbbreviationToNameMap = await DatabaseHelper.GetPositionAbbreviationToNameMap(sqliteConnection);
await VectorStoreHelper.PopulatePlayersVectorStore(vectorStore, textEmbeddingService, rootPath, positionAbbreviationToNameMap);

builder.AddPlayerVectorStoreTextSearch(vectorStore, textEmbeddingService, out var playerTextSearch);

// Build the kernel
Kernel kernel = builder.Build();
kernel.Plugins.AddFromObject(new PlayerSearchPlugin(playerTextSearch), "PlayerSearch");
kernel.Plugins.AddFromType<TeamBuilderPlugin>("TeamBuilder");

// Enable planning
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new() 
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

// Create a history store the conversation
var history = new ChatHistory();

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

string systemMessage = 
@"You are a helpful assistant that is used to get information about male and female football players and to build a team.
Your name is Angus.
When you are asked about players you should take your knowledge of football from the PlayerSearch plugin.
When you are asked to add players to a team or asked who is in the team you should use the TeamBuilder plugin.
In this context football means soccer, although you should refer to it as football.";

history.AddSystemMessage(systemMessage);

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
