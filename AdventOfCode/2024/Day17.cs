using _2024.Models.Day17;
using AOC.Utils;

namespace _2024;

public class Day17() :DayBase("17", "Chronospatial Computer")
{
    protected override Task<object> PartOne()
    {
        var result = GetOutInstructionsAsString();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetCorrectedRegisterA();

        return Task.FromResult<object>(result);
    }

    private string GetOutInstructionsAsString()
        => GetInput().InitializeComputer().GetOutputAsString();

    private long GetCorrectedRegisterA()
        => GetInput().InitializeComputer().TryDifferentValuesOfA().RegisterA;
}