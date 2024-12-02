using _2023.Models.Day10.Enums;

namespace _2023.Models.Day10;

public static class Day10Extensions
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