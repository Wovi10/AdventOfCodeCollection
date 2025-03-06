using _2024.Models.Day16;
using AOC.Utils;

namespace _2024;

public class Day16(): DayBase("16", "Reindeer Maze")
{
    protected override Task<object> PartOne()
    {
        var result = GetLowestResultForMaze();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetBestPathsDistinctTilesCount();

        return Task.FromResult<object>(result);
    }

    private long GetLowestResultForMaze()
        => GetInput()
            .ToMaze()
            // .PrintMaze()
            .GetBestPathResult();

    private int GetBestPathsDistinctTilesCount()
        => GetInput()
            .ToMaze()
            // .PrintMaze()
            .CountAllBestPathsDistinctCount();
}