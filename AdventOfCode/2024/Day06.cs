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
        var result = GetObstaclePositionsForLoop();

        return Task.FromResult<object>(result);
    }

    private long GetDistinctGuardPositions()
        => SharedMethods
            .GetInput(Day)
            .ToMap()
            .StartRunning()
            .CountVisited();

    private long GetObstaclePositionsForLoop()
        => SharedMethods
            .GetInput(Day)
            .ToMap()
            .GetNumWorkingPositions();
}