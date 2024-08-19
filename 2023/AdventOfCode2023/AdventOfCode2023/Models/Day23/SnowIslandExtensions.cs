namespace AdventOfCode2023_1.Models.Day23;

public static class SnowIslandExtensions
{
    public static SnowIsland CreateSnowIsland(this List<string> input)
    {
        var snowIsland = new SnowIsland(input);

        return snowIsland;
    }
}