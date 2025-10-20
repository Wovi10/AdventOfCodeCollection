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
        throw new NotImplementedException();
    }

    private long GetMinimumStepsToExit()
    {
        var maxDimensions = Constants.IsRealExercise ? 70 : 6;
        var numBytesFallen = Constants.IsRealExercise ? 1024 : 12;
        var byteCoordinates = GetInput().ToCoordinates(maxDimensions, numBytesFallen);
        byteCoordinates.PrintCoordinates();
        var result = byteCoordinates.FindMinimumStepsToExit();

        return 0;
    }
}