using System.ComponentModel;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Data;

namespace FootySk.Core;

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
public class PlayerSearchPlugin
{
    private VectorStoreTextSearch<Player> playerTextSearch;

    public PlayerSearchPlugin(VectorStoreTextSearch<Player> playerTextSearch)
    {
        this.playerTextSearch = playerTextSearch;
    }

    [KernelFunction("get_players")]
    [Description("Gets details about football players that match the query")]
    [return: Description("An array of details of football players that match the query")]
    public async Task<List<string>> GetPlayers(
        [Description("The query that describes how the user wants to search for players, which can include details about how good they are or what type of player they are, etc.")] string query
    )
    {
        List<string> playerNames = new List<string>();
        var results = await playerTextSearch.GetTextSearchResultsAsync(query, new() { Top = 5, Skip = 0});
        await foreach (var result in results.Results)
        {
            if (result?.Name != null)
                playerNames.Add(result.Name);
        }
        return playerNames;
    }
}
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.