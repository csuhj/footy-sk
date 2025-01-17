using Microsoft.Extensions.VectorData;
using FootySk.Database.Player;
using Microsoft.SemanticKernel.Embeddings;

namespace FootySk.Core;

public class Player
{
    [VectorStoreRecordKey]
    public ulong Id {get; set;}

    [VectorStoreRecordData(IsFilterable = true)]
    public string Name { get; set; }

    [VectorStoreRecordData(IsFullTextSearchable = true)]
    public string Description { get; set; }

    [VectorStoreRecordVector(Dimensions: 1536, DistanceFunction.CosineDistance, IndexKind.Hnsw)]
    public ReadOnlyMemory<float>? DescriptionEmbedding { get; set; }

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public static async Task<Player> Create(PlayerRecord playerRecord, ITextEmbeddingGenerationService textEmbeddingGenerationService)
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    {
        string description = CreateDescription(playerRecord);
        return new Player()
        {
            Id = (ulong)playerRecord.Id,
            Name = playerRecord.Name,
            Description = description,
            DescriptionEmbedding = await textEmbeddingGenerationService.GenerateEmbeddingAsync(description)
        };
    }

    private static string CreateDescription(PlayerRecord playerRecord)
    {
        string gender = playerRecord.Gender == "M" ? "Male" : "Female";
        return 
            $"The {gender} footballer called {playerRecord.Name} is ranked {playerRecord.Rank} in the world.\n"+
            $"They have an overall score of {playerRecord.OVR}, with pace of {playerRecord.PAC}, shooting of {playerRecord.SHO}, passing of {playerRecord.PAS}, dribbling of {playerRecord.DRI}, defending of {playerRecord.DEF} and physicality of {playerRecord.PHY}.\n"+
            $"Their playing position is {playerRecord.Position} and they prefer to play with their {playerRecord.PreferredFoot} foot.\n"+
            $"They are {playerRecord.Age} years old, {playerRecord.HeightInCm}cm tall and they weigh {playerRecord.WeightInKg}kg.\n"+
            $"They are from {playerRecord.Nation}, and play in the league called {playerRecord.League} for {playerRecord.Team}.\n"+
            $"Their styles of play are {string.Join(", ", playerRecord.PlayStyle)}.";
    }
}
