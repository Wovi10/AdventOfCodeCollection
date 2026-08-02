using AOC.Utils;

namespace AOC.Challenges.Years._2025;

public class Day05() : DayBase("05", "Cafeteria")
{
    protected override Task<object> PartOne()
    {
        var result = CountFreshIngredients();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private long CountFreshIngredients()
    {
        throw new NotImplementedException();
    }
}
