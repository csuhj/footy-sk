using Microsoft.Extensions.VectorData;
using FootySk.Database.Player;
using FootySk.Database.AttributeDetails;
using Microsoft.SemanticKernel.Embeddings;
using FootySk.Database.PositionDetails;
using FootySk.Database.PlaystyleDetails;

namespace FootySk.Core;

public class VectorStoreHelper
{
    public static IVectorStoreRecordCollection<ulong, Player> GetPlayersCollection(IVectorStore vectorStore) {
        return vectorStore.GetCollection<ulong, Player>("players");
    }

    public static IVectorStoreRecordCollection<ulong, AttributeDetails> GetAttributeDetailsCollection(IVectorStore vectorStore) {
        return vectorStore.GetCollection<ulong, AttributeDetails>("attribute_details");
    }

    public static IVectorStoreRecordCollection<ulong, PositionDetails> GetPositionDetailsCollection(IVectorStore vectorStore) {
        return vectorStore.GetCollection<ulong, PositionDetails>("position_details");
    }

    public static IVectorStoreRecordCollection<ulong, PlaystyleDetails> GetPlaystyleDetailsCollection(IVectorStore vectorStore) {
        return vectorStore.GetCollection<ulong, PlaystyleDetails>("playstyle_details");
    }

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public static async Task PopulatePlayersVectorStore(IVectorStore vectorStore, ITextEmbeddingGenerationService textEmbeddingService, string rootPath, Dictionary<string, string> positionAbbreviationToNameMap) {
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        var collection = GetPlayersCollection(vectorStore);

        if (await collection.CollectionExistsAsync()) {
            Console.WriteLine("Players collection already exists so not repopulating");
            return;
        }

        await collection.CreateCollectionIfNotExistsAsync();

        PlayerRecord[] players = await new PlayerCsvParser(Path.Combine(rootPath, "src/FootySk.Database/all_players.csv")).Parse();
        Console.WriteLine($"Got a total of {players.Length} players");

        Func<PlayerRecord, bool> playerFilter = p => p.League == "Premier League";
        Console.WriteLine($"Populating players collection from database with {players.Count(playerFilter)} players");

        foreach (var player in players.Where(playerFilter))
        {
            await collection.UpsertAsync(await Player.Create(player, textEmbeddingService, positionAbbreviationToNameMap));
        }
    }

    #pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public static async Task PopulateAttributeDataVectorStore(IVectorStore vectorStore, ITextEmbeddingGenerationService textEmbeddingService, string rootPath) {
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        var collection = GetAttributeDetailsCollection(vectorStore);

        if (await collection.CollectionExistsAsync()) {
            Console.WriteLine("Attribute Details collection already exists so not repopulating");
            return;
        }

        await collection.CreateCollectionIfNotExistsAsync();

        AttributeDetailsRecord[] attributeDetails = AttributeDetailsDataParser.GetAttributeDetails(Path.Combine(rootPath, "src/FootySk.Database/attribute_details_data.json"));
        Console.WriteLine($"Populating attribute details collection from database with {attributeDetails.Length} items");

        foreach (var attributeDetail in attributeDetails)
        {
            await collection.UpsertAsync(await AttributeDetails.Create(attributeDetail, textEmbeddingService));
        }
    }

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public static async Task PopulatePositionDataVectorStore(IVectorStore vectorStore, ITextEmbeddingGenerationService textEmbeddingService, string rootPath) {
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        var collection = GetPositionDetailsCollection(vectorStore);

        if (await collection.CollectionExistsAsync()) {
            Console.WriteLine("Position Details collection already exists so not repopulating");
            return;
        }

        await collection.CreateCollectionIfNotExistsAsync();

        PositionDetailsRecord[] positionDetails = PositionDetailsDataParser.GetPositionDetails(Path.Combine(rootPath, "src/FootySk.Database/position_details_data.json"));
        Console.WriteLine($"Populating position details collection from database with {positionDetails.Length} items");

        foreach (var positionDetail in positionDetails)
        {
            await collection.UpsertAsync(await PositionDetails.Create(positionDetail, textEmbeddingService));
        }
    }

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public static async Task PopulatePlaystyleDataVectorStore(IVectorStore vectorStore, ITextEmbeddingGenerationService textEmbeddingService, string rootPath) {
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        var collection = GetPlaystyleDetailsCollection(vectorStore);

        if (await collection.CollectionExistsAsync()) {
            Console.WriteLine("Playstyle Details collection already exists so not repopulating");
            return;
        }

        await collection.CreateCollectionIfNotExistsAsync();

        PlaystyleDetailsRecord[] playstyleDetails = PlaystyleDetailsDataParser.GetPlaystyleDetails(Path.Combine(rootPath, "src/FootySk.Database/playstyle_details_data.json"));
        Console.WriteLine($"Populating playstyle details collection from database with {playstyleDetails.Length} items");

        foreach (var playstyleDetail in playstyleDetails)
        {
            await collection.UpsertAsync(await PlaystyleDetails.Create(playstyleDetail, textEmbeddingService));
        }
    }
}