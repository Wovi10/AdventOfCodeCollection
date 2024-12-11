using _2024.Models.Day06;
using AOC.Utils;

namespace _2024;

public class Day06(): DayBase("06", "Guard Gallivant")
{
    protected override Task<object> PartOne()
    {
        var result = GetDistinctGuardPositions();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private long GetDistinctGuardPositions()
    {
        var input = SharedMethods.GetInput(Day);
        var map = new Map(input);
        map.StartRunning();

        return map.CountVisited();
    }
}