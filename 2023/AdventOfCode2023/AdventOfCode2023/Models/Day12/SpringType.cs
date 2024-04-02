namespace AdventOfCode2023_1.Models.Day12;

public enum SpringType
{
    Operational,
    Damaged,
    Unknown
}

public static class SpringStateExtensions
{
    public static SpringType ToSpringState(this char stateChar)
    {
        return stateChar switch
        {
            '.' => SpringType.Operational,
            '#' => SpringType.Damaged,
            '?' => SpringType.Unknown,
            _ => throw new ArgumentOutOfRangeException(nameof(stateChar), stateChar, null)
        };
    }

    public static bool IsOperational(this SpringType state)
        => state == SpringType.Operational;

    public static bool IsDamaged(this SpringType state) => state == SpringType.Damaged;
}