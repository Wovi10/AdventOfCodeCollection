namespace _2024.Models.Day20;

public enum TileType
{
    Track,
    Start,
    End,
    Wall
}

public static class TileTypeExtensions
{
    public static char ToChar(this TileType tileType)
        => tileType switch
        {
            TileType.Track => '.',
            TileType.Start => 'S',
            TileType.End => 'E',
            TileType.Wall => '#',
            _ => ' '
        };

    public static TileType FromChar(this char c)
        => c switch
        {
            '.' => TileType.Track,
            'S' => TileType.Start,
            'E' => TileType.End,
            '#' => TileType.Wall,
            _ => throw new ArgumentOutOfRangeException(nameof(c), $"Character '{c}' is not a valid TileType character.")
        };
}