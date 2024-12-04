using _2024.Models.Day04;
using AOC.Utils;

namespace _2024;

public class Day04() : DayBase("04", "Ceres Search")
{
    protected override Task<object> PartOne()
    {
        var result = GetXmasCount();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetXMasCount();

        return Task.FromResult<object>(result);
    }

    private long GetXmasCount()
        => SharedMethods
            .GetInput(Day)
            .SearchXmas();

    private long GetXMasCount()
        => SharedMethods
            .GetInput(Day)
            .SearchXMas();
}