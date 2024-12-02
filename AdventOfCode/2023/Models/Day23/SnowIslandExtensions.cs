using _2023.Models.Day23.TileTypes;

namespace _2023.Models.Day23;

public static class SnowIslandExtensions
{
    public static SnowIsland CreateSnowIsland(this List<string> input)
    {
        var snowIsland = new SnowIsland(input);

        return snowIsland;
    }

    public static ITileType ToTileType(this char type)
    {
        return type switch
        {
            '#' => new Forest(),
            '.' => new DefaultPath(),
            'v' => new DownWardsSlope(),
            '>' => new RightWardsSlope(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}