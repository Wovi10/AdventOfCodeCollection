using _2024.Models.Day19;
using AOC.Utils;

namespace _2024;

public class Day19(): DayBase("19", "Linen Layout")
{
    protected override Task<object> PartOne()
    {
        var result = GetNumberOfPossibleDesigns();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetTotalNumberOfPossibleDesignOptions();

        return Task.FromResult<object>(result);
    }

    private long GetNumberOfPossibleDesigns()
        => GetInput()
            .ToArray()
            .ToTowelDesignIssue()
            .GetNumberOfPossibleDesigns();

    private long GetTotalNumberOfPossibleDesignOptions()
        => GetInput()
            .ToArray()
            .ToTowelDesignIssue()
            .GetTotalNumberOfPossibleDesignOptions();
}