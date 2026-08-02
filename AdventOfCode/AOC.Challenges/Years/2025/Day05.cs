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
        long result = TotalCountFreshIngredients();

        return Task.FromResult<object>(result);
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

    private long TotalCountFreshIngredients()
    {
        var input = GetInput();

        var allBounds = new List<(long low, long high)>();
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
                return CountTotal(allBounds);

            var bounds = line.Split("-");

            var lowerBound = long.Parse(bounds[0]);
            var upperBound = long.Parse(bounds[1]);

            if (allBounds.Any(b => b.low <= lowerBound && b.high >= upperBound))
                continue;

            allBounds = allBounds.Except(allBounds.Where(b => b.low > lowerBound && b.high < upperBound)).ToList();
            allBounds.Add((lowerBound, upperBound));

            // Only leaves bounds that are totally separate or overlap on 1 side
        }

        return 0;
    }

    private long CountTotal(List<(long low, long high)> bounds)
    {
        throw new NotImplementedException();
    }
}
