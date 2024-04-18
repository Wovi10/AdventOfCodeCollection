using AdventOfCode2023_1.Models.Day17;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day17 : DayBase
{
    protected override async Task PartOne()
    {
        var constraints = new Constraints
        {
            MinNumberOfMovements = 0,
            MaxNumberOfMovements = 3
        };

        var result = GetMinimalHeatLoss(constraints);
        SharedMethods.PrintAnswer(result);

        await Task.CompletedTask;
    }

    protected override async Task PartTwo()
    {
        var constraints = new Constraints
        {
            MinNumberOfMovements = 4,
            MaxNumberOfMovements = 10
        };

        var result = GetMinimalHeatLoss(constraints);
        SharedMethods.PrintAnswer(result);

        await Task.CompletedTask;
    }

    private int GetMinimalHeatLoss(Constraints constraints)
    {
        var cityMap = new CityMap(Input);
        var minimalHeatLoss = cityMap.GetMinimalHeatLoss(constraints);
        
        return minimalHeatLoss;
    }
}