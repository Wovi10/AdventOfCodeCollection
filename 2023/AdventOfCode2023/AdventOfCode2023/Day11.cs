using AdventOfCode2023_1.Models.Day11;
using AdventOfCode2023_1.Shared;
using NUnit.Framework;

namespace AdventOfCode2023_1;

public class Day11 : DayBase
{
    private Universe? _universe;

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
        _universe = new Universe(Input);
        var enlargementFactor = Variables.RunningPartOne ? 1 : 1000000;
        _universe.Enlarge(enlargementFactor);
        var galaxyPairs = _universe.GetGalaxyPairs();
        foreach (var galaxyPair in galaxyPairs)
        {
            galaxyPair.SetManhattanDistance();
        }

        return galaxyPairs.Sum(galaxyPair => galaxyPair.ManhattanDistance);
    }
}