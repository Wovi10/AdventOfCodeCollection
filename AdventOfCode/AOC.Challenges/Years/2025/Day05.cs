using AOC.Utils;
using UtilsCSharp;

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
        var input = GetInput();

        var doingRanges = true;
        var ranges = new List<(long low, long high)>();
        var result = 0L;
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                // All ranges done, start finding fresh ids
                doingRanges = false;
                continue;
            }

            if (doingRanges)
            {
                var bounds = line.Split("-");
                ranges.Add(new (long.Parse(bounds[0]), long.Parse(bounds[1])));

                continue;
            }

            var number = long.Parse(line);
            result += ranges.Any(r => r.low <= number && r.high >= number) ? 1 : 0;
        }

        return result;
    }
}
