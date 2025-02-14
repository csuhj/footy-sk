using System.ComponentModel;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Data;

namespace FootySk.Core;

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
public class TeamBuilderPlugin
{
    private List<KeyValuePair<string, string>> playersAndPositions; 

    public TeamBuilderPlugin()
    {
        playersAndPositions = new List<KeyValuePair<string, string>>();
    }

    [KernelFunction("get_team")]
    [Description("Gets the list of players currently in the team and their positions")]
    [return: Description("An array of players and their positions")]
    public List<string> GetTeam()
    {
        return playersAndPositions.Select(playerAndPosition => $"{playerAndPosition.Key} - {playerAndPosition.Value}").ToList();
    }

    [KernelFunction("add_player_to_team")]
    [Description("Adds a player to the team in the specified position")]
    public void AddPlayerToTeam(
        [Description("The name of the player to add to the team")] string playerName,
        [Description("The position the player should play in, such as striker, goalkeeper, centre back, etc.")] string position
    )
    {
        playersAndPositions.Add(new KeyValuePair<string, string>(playerName, position));
    }
}
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.