using AdventOfCode2023_1.Models.Day11;
using AdventOfCode2023_1.Shared;
using NUnit.Framework;

namespace AdventOfCode2023_1;

public class Day11 : DayBase
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

        foreach (var galaxyPair in galaxyPairs) 
            galaxyPair.SetManhattanDistance();

        return galaxyPairs.Sum(galaxyPair => galaxyPair.ManhattanDistance);
    }
}