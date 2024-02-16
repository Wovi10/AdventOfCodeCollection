namespace AdventOfCode2023_1.Models.Day12;

public enum SpringState
{
    Operational,
    Damaged,
    Unknown
}

public static class SpringStateExtensions
{
    public static SpringState ToSpringState(this char stateChar)
    {
        return stateChar switch
        {
            '.' => SpringState.Operational,
            '#' => SpringState.Damaged,
            '?' => SpringState.Unknown,
            _ => throw new ArgumentOutOfRangeException(nameof(stateChar), stateChar, null)
        };
    }
}