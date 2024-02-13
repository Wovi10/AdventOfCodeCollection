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
            _ => TileType.Ground
        };
    }

    public static bool IsOpposite(this TileType tileType, TileType toCompare)
    {
        return tileType switch
        {
            TileType.NorthEast => toCompare == TileType.SouthWest,
            TileType.NorthWest => toCompare == TileType.SouthEast,
            TileType.SouthWest => toCompare == TileType.NorthEast,
            TileType.SouthEast => toCompare == TileType.NorthWest,
            TileType.EastWest => true,
            TileType.NorthSouth => true,
            TileType.Ground => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null),
            TileType.StartingPosition => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null),
            _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
        };
    }
}