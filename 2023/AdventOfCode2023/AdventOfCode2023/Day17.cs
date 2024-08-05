using AdventOfCode2023_1.Models.Day17;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day17() : DayBase("17", "Clumsy Crucible")
{
    protected override Task<object> PartOne()
    {
        var constraints = new Constraints {MinNumberOfMovements = 1, MaxNumberOfMovements = 3};
        var result = GetMinimalHeatLoss(constraints);

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var constraints = new Constraints {MinNumberOfMovements = 4, MaxNumberOfMovements = 10};
        var result = GetMinimalHeatLoss(constraints);

        return Task.FromResult<object>(result);
    }

    private static int GetMinimalHeatLoss(Constraints constraints)
    {
        var cityMap = new CityMap(Input, constraints);
        var minimalHeatLoss = cityMap.GetMinimalHeatLoss();

        return minimalHeatLoss;
    }
}