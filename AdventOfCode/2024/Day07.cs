using _2024.Models.Day07;
using AOC.Utils;

namespace _2024;

public class Day07(): DayBase("07", "Bridge repair")
{
    protected override Task<object> PartOne()
    {
        var result = GetTotalCalibration();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private long GetTotalCalibration()
        => SharedMethods
            .GetInput(Day)
            .ToEquations()
            .SolveEquations()
            .Sum();
}