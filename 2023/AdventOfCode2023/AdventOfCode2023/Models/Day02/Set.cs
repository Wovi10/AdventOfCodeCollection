using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day02;

public class Set
{
    public Dictionary<char, int> Colours { get; private set; }

    public Set(string colours)
    {
        Colours = DecideColours(colours);
    }

    private Dictionary<char, int> DecideColours(string colours)
    {
        Colours = new Dictionary<char, int>();

        var allColours = colours.Split(Constants.Comma);

        foreach (var colourCombo in allColours)
        {
            var colour = colourCombo.Trim().Split(Constants.Space);
            var key = colour[1][0];
            var amount = int.Parse(colour[0]);

            Colours.TryAdd(key, amount);
        }

        return Colours;
    }
}