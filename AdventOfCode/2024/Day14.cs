using _2024.Models.Day14;
using AOC.Utils;

namespace _2024;

public class Day14() : DayBase("14", "Restroom Redoubt")
{
    protected override Task<object> PartOne()
    {
        const int secondsToWait = 100;
        const int width = Constants.IsRealExercise ? 101 : 11;
        const int height = Constants.IsRealExercise ? 103 : 7;
        var result = GetSafetyFactor(secondsToWait, width, height);

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private long GetSafetyFactor(int secondsToWait, int width, int height)
        => GetInput()
            .CreateRobots()
            .RunAll(secondsToWait, width, height)
            .FilterCenterRobots(width, height)
            .GroupByQuadrant(width, height)
            .Select(g => g.Count())
            .Aggregate(1L, (acc, count) => acc * count);
}