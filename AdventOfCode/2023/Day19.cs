using _2023.Models.Day19;

namespace _2023;

public class Day19() : DayBase("19", "Aplenty")
{
    protected override Task<object> PartOne()
    {
        var result = SumPartRatingNumbers();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetTotalCombinations();
        
        return Task.FromResult<object>(result);
    }

    private long GetTotalCombinations()
    {
        var aplenty = new Aplenty(Input);
        return aplenty.CheckCombos([1,1,1,1],[4000,4000,4000,4000], "in");
    }

    private static long SumPartRatingNumbers()
    {
        var aplenty = new Aplenty(Input);
        aplenty.Process();

        return aplenty.GetRatings();
    }
}