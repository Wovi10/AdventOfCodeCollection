using _2024.Models;
using AOC.Utils;
using UtilsCSharp;
using Constants = UtilsCSharp.Utils.Constants;

namespace _2024;

public class Day01() : DayBase("01", "Historian Hysteria")
{
    protected override Task<object> PartOne()
    {
        var result = GetSumDistances();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetSimilarityScore();

        return Task.FromResult<object>(result);
    }

    private long GetSumDistances()
        => SharedMethods
            .GetInput(Day)
            .GetPairs()
            .GetIdLists()
            .Sort()
            .GetDistances()
            .Sum();

    private long GetSimilarityScore()
        => SharedMethods
            .GetInput(Day)
            .GetPairs()
            .GetIdLists()
            .GetAppearanceCountList()
            .GetSimilarityList()
            .Sum();
}