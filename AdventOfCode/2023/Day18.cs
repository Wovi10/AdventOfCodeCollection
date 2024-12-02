using _2023.Models.Day18;

namespace _2023;

public class Day18() : DayBase("18", "Lavaduct Lagoon")
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