namespace AdventOfCode2023_1.Models.Day23;

public enum TileType
{
    Forest,
    Path,
    SlopeToSouth,
    SlopeToEast
}

public static class TileTypeExtensions
{
    public static char ToChar(this TileType tileType)
        => tileType switch
        {
            TileType.Forest => '#',
            TileType.Path => '.',
            TileType.SlopeToSouth => 'v',
            TileType.SlopeToEast => '>',
            _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
        };

    public static TileType ToTileType(this char tileType)
        => tileType switch
        {
            '#' => TileType.Forest,
            '.' => TileType.Path,
            'v' => TileType.SlopeToSouth,
            '>' => TileType.SlopeToEast,
            _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
        };
}