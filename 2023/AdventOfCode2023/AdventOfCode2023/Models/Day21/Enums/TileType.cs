namespace AdventOfCode2023_1.Models.Day21.Enums;

public enum TileType
{
    StartingPosition,
    GardenPlot,
    Rock
}

public static class TileTypeExtensions
{
    public static TileType ToTileType(this char tileTypeInput)
    {
        return tileTypeInput switch
        {
            'S' => TileType.StartingPosition,
            '.' => TileType.GardenPlot,
            '#' => TileType.Rock,
            _ => throw new NotImplementedException("UnexpectedInput")
        };
    }
}