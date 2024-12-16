using _2024.Models.Day11;
using AOC.Utils;

namespace _2024;

public class Day11(): DayBase("11", "Plutonian Pebbles")
{
    protected override Task<object> PartOne()
    {
        var result = CountStonesAfterBlinking(25);

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = 0;

        return Task.FromResult<object>(result);
    }

    private long CountStonesAfterBlinking(int timesBlinked)
    {
        return SharedMethods.GetInput(Day).First().ToListOfLongs().StartBlinking(timesBlinked);
    }
}