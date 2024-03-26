namespace AdventOfCode2023_1.Models.Day14;

public static class Day14Extensions
{
    public static RockType ToRockType(this char rockChar)
    {
        return rockChar switch
        {
            '.' => RockType.None,
            'O' => RockType.Round,
            '#' => RockType.Square,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}