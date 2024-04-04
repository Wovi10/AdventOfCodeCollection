using AdventOfCode2023_1.Models.Day17;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day17 : DayBase
{
    protected override async Task PartOne()
    {
        var result = await GetMinimalHeatLoss();
        SharedMethods.PrintAnswer(result);
    }

    protected override Task PartTwo()
    {
        throw new NotImplementedException();
    }

    private async Task<int> GetMinimalHeatLoss()
    {
        var cityMap = new CityMap(Input);
        var minimalHeatLoss = await cityMap.GetMinimalHeatLoss();
        
        return 0;
    }
}