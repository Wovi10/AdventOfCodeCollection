using AdventOfCode2023_1.Models.Day17;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day17 : DayBase
{
    protected override async Task PartOne()
    {
        var result = GetMinimalHeatLoss();
        SharedMethods.PrintAnswer(result);

        await Task.CompletedTask;
    }

    protected override Task PartTwo()
    {
        throw new NotImplementedException();
    }

    private int GetMinimalHeatLoss()
    {
        var cityMap = new CityMap(Input);
        var minimalHeatLoss = cityMap.GetMinimalHeatLoss();
        
        return minimalHeatLoss;
    }
}