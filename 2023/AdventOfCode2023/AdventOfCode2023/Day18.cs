using System.Numerics;
using AdventOfCode2023_1.Models.Day18;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day18 : DayBase
{
    protected override Task PartOne()
    {
        var result = CalculateHoleSize();
        SharedMethods.PrintAnswer(result);

        return Task.CompletedTask;
    }

    protected override Task PartTwo()
    {
        var result = CalculateHoleSize();
        SharedMethods.PrintAnswer(result);

        return Task.CompletedTask;
    }

    private static  long CalculateHoleSize()
    {
        var excavationSite = new ExcavationSite(Input);
        return excavationSite.CalculateArea();
    }
}