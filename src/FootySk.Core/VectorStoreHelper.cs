using Microsoft.Extensions.VectorData;
using FootySk.Database.Player;
using FootySk.Database.AttributeDetails;
using Microsoft.SemanticKernel.Embeddings;
using FootySk.Database.PositionDetails;
using FootySk.Database.PlaystyleDetails;

namespace FootySk.Core;

public class VectorStoreHelper
{
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public static async Task PopulatePlayersVectorStore(IVectorStore vectorStore, ITextEmbeddingGenerationService textEmbeddingService, string rootPath, Dictionary<string, string> positionAbbreviationToNameMap) {
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        // Choose a collection from the database and specify the type of key and record stored in it via Generic parameters.
        var collection = vectorStore.GetCollection<ulong, Player>("players");

        if (await collection.CollectionExistsAsync()) {
            Console.WriteLine("Players collection already exists so not repopulating");
            return;
        }

        // Create the collection if it doesn't exist yet.
        await collection.CreateCollectionIfNotExistsAsync();

        PlayerRecord[] players = await new PlayerCsvParser(Path.Combine(rootPath, "src/FootySk.Database/all_players.csv")).Parse();
        Console.WriteLine($"Populating players collection from database with {players.Length} players");

        //See https://github.com/MicrosoftDocs/semantic-kernel-docs/blob/main/semantic-kernel/concepts/vector-store-connectors/vector-search.md
        //    https://learn.microsoft.com/en-us/semantic-kernel/concepts/vector-store-connectors/vector-search?pivots=programming-language-csharp
        //For now just update the first 10 players
        foreach (var player in players.Take(10))
        {
            // Create a record and generate a vector for the description using your chosen embedding generation implementation.
            await collection.UpsertAsync(await Player.Create(player, textEmbeddingService, positionAbbreviationToNameMap));
        }
    }

    #pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public static async Task PopulateAttributeDataVectorStore(IVectorStore vectorStore, ITextEmbeddingGenerationService textEmbeddingService, string rootPath) {
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        // Choose a collection from the database and specify the type of key and record stored in it via Generic parameters.
        var collection = vectorStore.GetCollection<ulong, AttributeDetails>("attribute_details");

        if (await collection.CollectionExistsAsync()) {
            Console.WriteLine("Attribute Details collection already exists so not repopulating");
            return;
        }

        // Create the collection if it doesn't exist yet.
        await collection.CreateCollectionIfNotExistsAsync();

        AttributeDetailsRecord[] attributeDetails = AttributeDetailsDataParser.GetAttributeDetails(Path.Combine(rootPath, "src/FootySk.Database/attribute_details_data.json"));
        Console.WriteLine($"Populating attribute details collection from database with {attributeDetails.Length} items");

        //See https://github.com/MicrosoftDocs/semantic-kernel-docs/blob/main/semantic-kernel/concepts/vector-store-connectors/vector-search.md
        //    https://learn.microsoft.com/en-us/semantic-kernel/concepts/vector-store-connectors/vector-search?pivots=programming-language-csharp
        //For now just update the first 10 players
        foreach (var attributeDetail in attributeDetails)
        {
            // Create a record and generate a vector for the description using your chosen embedding generation implementation.
            await collection.UpsertAsync(await AttributeDetails.Create(attributeDetail, textEmbeddingService));
        }
    }

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public static async Task PopulatePositionDataVectorStore(IVectorStore vectorStore, ITextEmbeddingGenerationService textEmbeddingService, string rootPath) {
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        // Choose a collection from the database and specify the type of key and record stored in it via Generic parameters.
        var collection = vectorStore.GetCollection<ulong, PositionDetails>("position_details");

        if (await collection.CollectionExistsAsync()) {
            Console.WriteLine("Position Details collection already exists so not repopulating");
            return;
        }

        // Create the collection if it doesn't exist yet.
        await collection.CreateCollectionIfNotExistsAsync();

        PositionDetailsRecord[] positionDetails = PositionDetailsDataParser.GetPositionDetails(Path.Combine(rootPath, "src/FootySk.Database/position_details_data.json"));
        Console.WriteLine($"Populating position details collection from database with {positionDetails.Length} items");

        //See https://github.com/MicrosoftDocs/semantic-kernel-docs/blob/main/semantic-kernel/concepts/vector-store-connectors/vector-search.md
        //    https://learn.microsoft.com/en-us/semantic-kernel/concepts/vector-store-connectors/vector-search?pivots=programming-language-csharp
        //For now just update the first 10 players
        foreach (var positionDetail in positionDetails)
        {
            // Create a record and generate a vector for the description using your chosen embedding generation implementation.
            await collection.UpsertAsync(await PositionDetails.Create(positionDetail, textEmbeddingService));
        }
    }

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public static async Task PopulatePlaystyleDataVectorStore(IVectorStore vectorStore, ITextEmbeddingGenerationService textEmbeddingService, string rootPath) {
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        // Choose a collection from the database and specify the type of key and record stored in it via Generic parameters.
        var collection = vectorStore.GetCollection<ulong, PlaystyleDetails>("playstyle_details");

        if (await collection.CollectionExistsAsync()) {
            Console.WriteLine("Playstyle Details collection already exists so not repopulating");
            return;
        }

        // Create the collection if it doesn't exist yet.
        await collection.CreateCollectionIfNotExistsAsync();

        PlaystyleDetailsRecord[] playstyleDetails = PlaystyleDetailsDataParser.GetPlaystyleDetails(Path.Combine(rootPath, "src/FootySk.Database/playstyle_details_data.json"));
        Console.WriteLine($"Populating playstyle details collection from database with {playstyleDetails.Length} items");

        //See https://github.com/MicrosoftDocs/semantic-kernel-docs/blob/main/semantic-kernel/concepts/vector-store-connectors/vector-search.md
        //    https://learn.microsoft.com/en-us/semantic-kernel/concepts/vector-store-connectors/vector-search?pivots=programming-language-csharp
        //For now just update the first 10 players
        foreach (var playstyleDetail in playstyleDetails)
        {
            // Create a record and generate a vector for the description using your chosen embedding generation implementation.
            await collection.UpsertAsync(await PlaystyleDetails.Create(playstyleDetail, textEmbeddingService));
        }
    }
}