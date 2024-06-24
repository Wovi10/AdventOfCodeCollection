using AdventOfCode2023_1.Models.Day19;

namespace AdventOfCode2023_1;

public class Day19 : DayBase
{
    protected override Task<object> PartOne()
    {
        var result = SumRatingNumbers();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private static long SumRatingNumbers()
    {
        var aplenty = new Aplenty(Input);
        aplenty.Process();

        return aplenty.GetRatings();
    }
}