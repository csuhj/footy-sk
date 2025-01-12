using Microsoft.Extensions.VectorData;
using FootySk.Database;
using Microsoft.SemanticKernel.Embeddings;

namespace FootySk.Core;

public class AttributeDetails
{
    [VectorStoreRecordKey]
    public ulong Id {get; set;}

    [VectorStoreRecordData(IsFilterable = true)]
    public string Title { get; set; }

   [VectorStoreRecordData(IsFilterable = true)]
    public string Category { get; set; }

   [VectorStoreRecordData(IsFilterable = true)]
    public string Weighting { get; set; }

    [VectorStoreRecordData(IsFullTextSearchable = true)]
    public string Description { get; set; }

    [VectorStoreRecordVector(Dimensions: 1536, DistanceFunction.CosineDistance, IndexKind.Hnsw)]
    public ReadOnlyMemory<float>? DescriptionEmbedding { get; set; }

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public static async Task<AttributeDetails> Create(AttributeDetailsRecord attributeDetailsRecord, ITextEmbeddingGenerationService textEmbeddingGenerationService)
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    {
        return new AttributeDetails()
        {
            Id = (ulong)attributeDetailsRecord.Id,
            Title = attributeDetailsRecord.Title ?? "",
            Category = attributeDetailsRecord.Category ?? "",
            Weighting = attributeDetailsRecord.Weighting ?? "",
            Description = attributeDetailsRecord.Description ?? "",
            DescriptionEmbedding = await textEmbeddingGenerationService.GenerateEmbeddingAsync(attributeDetailsRecord.Description ?? "")
        };
    }
}