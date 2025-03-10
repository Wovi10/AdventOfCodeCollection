﻿using _2024.Models.Day08;
using AOC.Utils;

namespace _2024;

public class Day08(): DayBase("08", "Resonant Collinearity")
{
    protected override Task<object> PartOne()
    {
        var result = CalculateUniqueAntinodeLocations();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = CalculateUniqueAntinodeLocations();

        return Task.FromResult<object>(result);
    }

    private int CalculateUniqueAntinodeLocations()
        => new Map(SharedMethods.GetInput(Day)).GetAntinodeCoordinates().Count;
}