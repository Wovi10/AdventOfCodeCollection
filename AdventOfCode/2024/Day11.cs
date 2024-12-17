using _2024.Models.Day11;
using AOC.Utils;

namespace _2024;

public class Day11(): DayBase("11", "Plutonian Pebbles")
{
    protected override async Task<object> PartOne()
    {
        var result = await CountStonesAfterBlinking(25);

        return result;
    }

    protected override async Task<object> PartTwo()
    {
        var result = await CountStonesAfterBlinking(75);

        return result;
    }

    private async Task<long> CountStonesAfterBlinking(int timesBlinked)
    {
        return await SharedMethods.GetInput(Day).First().ToListOfLongs().StartBlinking(timesBlinked);
    }
}