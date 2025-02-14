﻿using _2023.Models.Day11;
using AOC.Utils;

namespace _2023;

public class Day11() : DayBase("11", "Cosmic Expansion")
{

    protected override Task<object> PartOne()
    {
        var result = GetSumOfShortestPaths();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetSumOfShortestPaths();

        return Task.FromResult<object>(result);
    }

    private long GetSumOfShortestPaths()
    {
        var universe = new Universe<long>(Input);
        var enlargementFactor = Variables.RunningPartOne ? 1 : 1000000;
        universe.Enlarge(enlargementFactor);
        var galaxyPairs = universe.GetGalaxyPairs();

        return galaxyPairs.Sum(galaxyPair => galaxyPair.ManhattanDistance);
    }
}