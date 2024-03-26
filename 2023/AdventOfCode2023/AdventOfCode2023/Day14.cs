using AdventOfCode2023_1.Models.Day14;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day14:DayBase
{
    protected override Task PartOne()
    {
        var result = CalculateTotalLoad();
        SharedMethods.PrintAnswer(result);
        return Task.CompletedTask;
    }

    protected override Task PartTwo()
    {
        throw new NotImplementedException();
    }

    private long CalculateTotalLoad()
    {
        var dish = new Dish(Input);

        dish.TiltNorth();
        dish.CalculateTotalLoad();

        return dish.TotalLoad;
    }
}