using AdventOfCode2023_1.Models.Day17;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day17 : DayBase
{
    protected override async Task PartOne()
    {
        Constraints.MinNumberOfMovements = 0;
        Constraints.MaxNumberOfMovements = 3;

        var result = GetMinimalHeatLoss();
        SharedMethods.PrintAnswer(result);

        await Task.CompletedTask;
    }

    protected override async Task PartTwo()
    {
        Constraints.MinNumberOfMovements = 4;
        Constraints.MaxNumberOfMovements = 10;

        var result = GetMinimalHeatLoss();
        SharedMethods.PrintAnswer(result);

        await Task.CompletedTask;
    }

    private static int GetMinimalHeatLoss()
    {
        var cityMap = new CityMap(Input);
        var minimalHeatLoss = cityMap.GetMinimalHeatLoss();
        
        return minimalHeatLoss;
    }
}