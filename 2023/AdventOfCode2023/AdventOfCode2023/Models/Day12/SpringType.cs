﻿namespace AdventOfCode2023_1.Models.Day12;

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

    public static char ToChar(this SpringType state)
    {
        return state switch
        {
            SpringType.Operational => '.',
            SpringType.Damaged => '#',
            SpringType.Unknown => '?',
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }

    private static bool IsOperational(this SpringType state)
        => state == SpringType.Operational;

    public static bool IsDamaged(this SpringType state) => state == SpringType.Damaged;

    public static bool IsPossible(this SpringType currentSpringType, SpringType? previousSpring, SpringType? nextSpring,
        SpringType? lastInLength, List<SpringType> followingSprings, SpringType? firstAfterLength)
    {
        if (lastInLength == null || followingSprings.Count == 0)
            return false;

        if (previousSpring?.IsDamaged() ?? false)
            return false;

        if (currentSpringType.IsOperational())
            return false;

        if (followingSprings.Count == 1)
            return !(nextSpring?.IsDamaged() ?? false);

        if (firstAfterLength?.IsDamaged() ?? false)
            return false;

        return !followingSprings.Any(spring => spring.IsOperational());
    }
}