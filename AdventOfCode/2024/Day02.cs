using _2024.Models.Day02;
using AOC.Utils;

namespace _2024;

public class Day02():DayBase("02", "Red-Nosed Reports")
{
    protected override Task<object> PartOne()
    {
        var result = GetSafeReportCount();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private long GetSafeReportCount()
        => SharedMethods
            .GetInput(Day)
            .ToReports()
            .Count(report => report.IsSafe());
}