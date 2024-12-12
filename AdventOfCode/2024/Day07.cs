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
    {
        return SharedMethods
            .GetInput(Day)
            .ToEquations()
            .SolveEquations()
            .Sum();

        var input = SharedMethods.GetInput(Day);
        var calibration = 0;
        foreach (var line in input)
        {
            calibration += int.Parse(line);
        }

        return calibration;
    }
}