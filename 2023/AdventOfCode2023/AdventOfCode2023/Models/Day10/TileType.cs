namespace AdventOfCode2023_1.Models.Day10;

public enum TileType
{
    NorthSouth,
    EastWest,
    NorthEast,
    NorthWest,
    SouthWest,
    SouthEast,
    Ground,
    StartingPosition
}

public static class TileTypeExtensions
{
    public static char ToChar(this TileType tileType)
    {
        return tileType switch
        {
            TileType.NorthSouth => '|',
            TileType.EastWest => '-',
            TileType.NorthEast => 'L',
            TileType.NorthWest => 'J',
            TileType.SouthWest => '7',
            TileType.SouthEast => 'F',
            TileType.Ground => '.',
            TileType.StartingPosition => 'S',
            _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, "Invalid tile type.")
        };
    }

    public static TileType ToTileType(this char tileInput)
    {
        return tileInput switch
        {
            '|' => TileType.NorthSouth,
            '-' => TileType.EastWest,
            'L' => TileType.NorthEast,
            'J' => TileType.NorthWest,
            '7' => TileType.SouthWest,
            'F' => TileType.SouthEast,
            '.' => TileType.Ground,
            'S' => TileType.StartingPosition,
            _ => throw new ArgumentOutOfRangeException(nameof(tileInput), tileInput, "Invalid tile input.")
        };
    }
}