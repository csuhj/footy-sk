using Microsoft.Extensions.VectorData;
using FootySk.Database.PlaystyleDetails;
using Microsoft.SemanticKernel.Embeddings;

namespace FootySk.Core;

public class PlaystyleDetails
{
    [VectorStoreRecordKey]
    public ulong Id {get; set;}

    [VectorStoreRecordData(IsFilterable = true)]
    public string Title { get; set; }

   [VectorStoreRecordData(IsFilterable = true)]
    public string Category { get; set; }

    [VectorStoreRecordData(IsFullTextSearchable = true)]
    public string Description { get; set; }

    [VectorStoreRecordVector(Dimensions: 1536, DistanceFunction.CosineDistance, IndexKind.Hnsw)]
    public ReadOnlyMemory<float>? DescriptionEmbedding { get; set; }

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public static async Task<PlaystyleDetails> Create(PlaystyleDetailsRecord playstyleDetailsRecord, ITextEmbeddingGenerationService textEmbeddingGenerationService)
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    {
        return new PlaystyleDetails()
        {
            Id = (ulong)playstyleDetailsRecord.Id,
            Title = playstyleDetailsRecord.Title ?? "",
            Category = playstyleDetailsRecord.Category ?? "",
            Description = playstyleDetailsRecord.Info ?? "",
            DescriptionEmbedding = await textEmbeddingGenerationService.GenerateEmbeddingAsync(playstyleDetailsRecord.Info ?? "")
        };
    }
}