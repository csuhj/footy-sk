namespace FootySk.Database;

using System.Text.Json;

public class AttributeDetailsDataParser
{
    public static AttributeDetailsRecord[] GetAttributeDetails(string filename)
    {
        AttributeDetailsData? attributeDetailsData;
        using (var reader = new StreamReader(filename))
        {
            attributeDetailsData = JsonSerializer.Deserialize<AttributeDetailsData>(reader.BaseStream);
        }

        return attributeDetailsData?.Data ?? [];
    }
}