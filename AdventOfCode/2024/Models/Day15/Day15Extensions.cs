using UtilsCSharp.Enums;

namespace _2024.Models.Day15;

public static class Day15Extensions
{
    public static WarehouseWithInstructions ToWarehouseWithInstructions(this IEnumerable<string> input)
        => new(input);

    public static Direction? ToDirection(this char c)
        => c switch
        {
            '^' => Direction.Up,
            'v' => Direction.Down,
            '>' => Direction.Right,
            '<' => Direction.Left,
            _ => null
        };
}