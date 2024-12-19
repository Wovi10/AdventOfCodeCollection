using _2024.Models.Day12;
using AOC.Utils;

namespace _2024;

public class Day12(): DayBase("12", "Garden Groups")
{
    protected override Task<object> PartOne()
    {
        var result = GetSumFencingPrices();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetSumFencingPrices();

        return Task.FromResult<object>(result);
    }

    private long GetSumFencingPrices()
        => GetInput().ToGarden().GetFencingPrice();
}