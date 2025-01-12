using Microsoft.Extensions.VectorData;
using FootySk.Database.PositionDetails;
using Microsoft.SemanticKernel.Embeddings;

namespace FootySk.Core;

public class PositionDetails
{
    [VectorStoreRecordKey]
    public ulong Id {get; set;}

    [VectorStoreRecordData(IsFilterable = true)]
    public string Title { get; set; }

   [VectorStoreRecordData(IsFilterable = true)]
    public string Abbreviation { get; set; }

    [VectorStoreRecordData(IsFullTextSearchable = true)]
    public string Description { get; set; }

    [VectorStoreRecordVector(Dimensions: 1536, DistanceFunction.CosineDistance, IndexKind.Hnsw)]
    public ReadOnlyMemory<float>? DescriptionEmbedding { get; set; }

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public static async Task<PositionDetails> Create(PositionDetailsRecord positionDetailsRecord, ITextEmbeddingGenerationService textEmbeddingGenerationService)
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    {
        return new PositionDetails()
        {
            Id = (ulong)positionDetailsRecord.Id,
            Title = positionDetailsRecord.Title ?? "",
            Abbreviation = positionDetailsRecord.Abbreviation ?? "",
            Description = positionDetailsRecord.Description ?? "",
            DescriptionEmbedding = await textEmbeddingGenerationService.GenerateEmbeddingAsync(positionDetailsRecord.Description ?? "")
        };
    }
}