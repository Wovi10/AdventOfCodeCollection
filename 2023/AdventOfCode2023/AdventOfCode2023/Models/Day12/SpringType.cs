using AdventOfCode2023_1.Models.Day12.Enums;
using UtilsCSharp.Utils;

namespace AdventOfCode2023_1.Models.Day12;

public static class SpringStateExtensions
{
    public static SpringType ToSpringState(this char stateChar)
    {
        return stateChar.ToString() switch
        {
            Constants.Dot => SpringType.Operational,
            Constants.HashTag => SpringType.Damaged,
            Constants.QuestionMark => SpringType.Unknown,
            _ => throw new ArgumentOutOfRangeException(nameof(stateChar), stateChar, null)
        };
    }

    public static bool IsOperational(this SpringType state)
        => state == SpringType.Operational;

    public static bool IsDamaged(this SpringType state) 
        => state == SpringType.Damaged;
}