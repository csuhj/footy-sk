using Microsoft.Extensions.VectorData;
using FootySk.Database.Player;
using FootySk.Database.AttributeDetails;
using Microsoft.SemanticKernel.Embeddings;
using FootySk.Database.PositionDetails;
using FootySk.Database.PlaystyleDetails;
using Microsoft.Data.Sqlite;

namespace FootySk.Core;

public class DatabaseHelper
{
    public static async Task<Dictionary<string, string>> GetPositionAbbreviationToNameMap(SqliteConnection connection)
    {
        Dictionary<string, string> positionAbbreviationToNameMap = new Dictionary<string, string>();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT Abbreviation, Title FROM position_details";
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    positionAbbreviationToNameMap.Add(reader.GetString(0), reader.GetString(1));
                }
            }
        }

        return positionAbbreviationToNameMap;
    }
}