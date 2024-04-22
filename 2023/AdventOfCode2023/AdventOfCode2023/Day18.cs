using AdventOfCode2023_1.Models.Day18;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day18:DayBase
{
    protected override Task PartOne()
    {
        var result = CalculateHoleSize();
        SharedMethods.PrintAnswer(result);
        
        return Task.CompletedTask;
    }

    protected override Task PartTwo()
    {
        throw new NotImplementedException();
    }

    private long CalculateHoleSize()
    {
        var excavationSite = new ExcavationSite(Input);
        excavationSite.ExecuteDigPlan();

        return 0;
    }
}