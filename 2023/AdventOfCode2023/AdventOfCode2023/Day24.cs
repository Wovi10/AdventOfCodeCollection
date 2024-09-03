using AdventOfCode2023_1.Models.Day24;

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
        throw new NotImplementedException();
    }

    private int GetNumberOfCrossingPaths()
    {
        var hailStorm = new HailStorm(Input);

        return 0;
    }
}