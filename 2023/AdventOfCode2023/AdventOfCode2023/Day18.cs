using AdventOfCode2023_1.Models.Day18;

namespace AdventOfCode2023_1;

public class Day18 : DayBase
{
    protected override Task<object> PartOne()
    {
        var result = CalculateHoleSize();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = CalculateHoleSize();

        return Task.FromResult<object>(result);
    }

    private static  long CalculateHoleSize()
    {
        var excavationSite = new ExcavationSite(Input);
        return excavationSite.CalculateArea();
    }
}