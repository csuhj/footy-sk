namespace FootySk.Database.PlaystyleDetails;

using System.Text.Json;

public class PlaystyleDetailsDataParser
{
    public static PlaystyleDetailsRecord[] GetPlaystyleDetails(string filename)
    {
        PlaystyleDetailsData? playstyleDetailsData;
        using (var reader = new StreamReader(filename))
        {
            playstyleDetailsData = JsonSerializer.Deserialize<PlaystyleDetailsData>(reader.BaseStream);
        }

        return playstyleDetailsData?.Data ?? [];
    }
}