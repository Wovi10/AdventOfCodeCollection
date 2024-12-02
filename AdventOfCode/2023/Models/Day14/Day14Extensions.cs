namespace _2023.Models.Day14;

public static class Day14Extensions
{
    public static bool? ToRockType(this char rockChar)
    {
        return rockChar switch
        {
            '.' => null,
            'O' => true,
            '#' => false,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}