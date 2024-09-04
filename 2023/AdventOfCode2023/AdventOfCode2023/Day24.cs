using AdventOfCode2023_1.Models.Day24;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day24() : DayBase("24", "Never Tell Me The Odds")
{
    protected override Task<object> PartOne()
    {
        var result = GetNumberOfCrossingPaths();
        
        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetInitialPointStoneSum();
        
        return Task.FromResult<object>(result);
    }

    private static int GetNumberOfCrossingPaths() 
        => new HailStorm(Input).GetNumberOfCrossingPaths();

    private static double GetInitialPointStoneSum() 
        => Math
            .Round(
                new HailStorm(
                    Input.Take(3).ToList())
                    .GetIntersection());
}