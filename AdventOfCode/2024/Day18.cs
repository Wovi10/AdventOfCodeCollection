using _2024.Models.Day18;
using AOC.Utils;

namespace _2024;

public class Day18():DayBase("18", "RAM Run")
{
    protected override Task<object> PartOne()
    {
        var result = GetMinimumStepsToExit();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetFirstImpossibleMaze();

        return Task.FromResult<object>(result);
    }

    private long GetMinimumStepsToExit()
        =>
            GetInput()
                .ToCoordinates(Constants.IsRealExercise ? 70 : 6, Constants.IsRealExercise ? 1024 : 12)
                .FindMinimumStepsToExit();

    private string GetFirstImpossibleMaze()
    {
        return GetInput()
            .ToArray()
            .GetFirstImpossibleMaze(Constants.IsRealExercise ? 70 : 6, Constants.IsRealExercise ? 1024 : 12);
    }
}