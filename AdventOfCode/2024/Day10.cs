using _2024.Models.Day10;
using AOC.Utils;

namespace _2024;

public class Day10(): DayBase("10", "Hoof It")
{
    protected override Task<object> PartOne()
    {
        var result = GetTrailHeadScoresSum();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetTrailHeadScoresSum();

        return Task.FromResult<object>(result);
    }

    private long GetTrailHeadScoresSum()
        => SharedMethods.GetInput(Day).ToMap().FindScore();
}