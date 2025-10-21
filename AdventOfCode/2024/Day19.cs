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
        throw new NotImplementedException();
    }

    private long GetNumberOfPossibleDesigns()
        => GetInput()
            .ToArray()
            .ToTowelDesignIssue()
            .GetNumberOfPossibleDesigns();
}