using _2024.Models.Day15;
using AOC.Utils;

namespace _2024;

public class Day15():DayBase("15", "Warehouse Woes")
{
    protected override Task<object> PartOne()
    {
        var result = GetSumBoxesGpsCoordinatesAfterRunning();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private long GetSumBoxesGpsCoordinatesAfterRunning()
        => GetInput()
            .ToWarehouseWithInstructions()
            .RunInstructions()
            .Warehouse
            .GetGpsLocationBoxes()
            .Sum();
}