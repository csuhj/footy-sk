namespace FootySk.Database.PositionDetails;

using System.Text.Json;

public class PositionDetailsDataParser
{
    public static PositionDetailsRecord[] GetPositionDetails(string filename)
    {
        PositionDetailsData? positionDetailsData;
        using (var reader = new StreamReader(filename))
        {
            positionDetailsData = JsonSerializer.Deserialize<PositionDetailsData>(reader.BaseStream);
        }

        return positionDetailsData?.Data ?? [];
    }
}