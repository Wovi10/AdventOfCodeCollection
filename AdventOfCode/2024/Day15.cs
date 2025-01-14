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
        var result = GetSumBoxesGpsCoordinatesAfterRunningPart2();

        return Task.FromResult<object>(result);
    }

    private long GetSumBoxesGpsCoordinatesAfterRunning()
        => GetInput()
            .ToWarehouseWithInstructions()
            .RunInstructions()
            .Warehouse
            .GetGpsLocationBoxes()
            .Sum();

    private long GetSumBoxesGpsCoordinatesAfterRunningPart2()
        => GetInput()
            .ToWarehouseWithInstructionsPart2()
            .RunInstructions()
            .Warehouse
            .GetGpsLocationBoxes()
            .Sum();
}