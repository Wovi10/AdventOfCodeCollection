using AdventOfCode2023_1.Models.Day14;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day14:DayBase
{
    protected override async Task PartOne()
    {
        var result = await CalculateTotalLoad();
        SharedMethods.PrintAnswer(result);
    }

    protected override async Task PartTwo()
    {
        var result = await CalculateTotalLoad();
        SharedMethods.PrintAnswer(result);
    }

    private async Task<long> CalculateTotalLoad()
    {
        var dish = new Dish(Input);

        if (Variables.RunningPartOne)
            await dish.TiltNorth();
        else
            await dish.RunCycles(1000000000);
        dish.CalculateTotalLoad();

        return dish.TotalLoad;
    }
}