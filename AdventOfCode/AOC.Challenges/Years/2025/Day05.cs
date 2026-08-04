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

            allBounds.Add((lowerBound, upperBound));
        }

        return 0;
    }

    private static long CountTotal(List<(long low, long high)> bounds)
    {
        var hasChanges = true;
        while (hasChanges)
            bounds = RewriteBounds(bounds, out hasChanges);

        return bounds.Sum(b => b.high - b.low + 1);
    }

    private static List<(long low, long high)> RewriteBounds(List<(long low, long high)> bounds, out bool hasChanges)
    {
        var boundsWithIndex = bounds
                .OrderBy(b => b.low)
                .ThenBy(b => b.high)
                .Distinct()
                .Select((b, index) => (b.low, b.high, index))
                .ToList();

        var result  = new List<(long low, long high)>();
        var toSkip = new HashSet<int>();
        foreach (var (currentLow, currentHigh, index) in boundsWithIndex)
        {
            if (toSkip.Contains(index))
                continue;

            // currentLow is the lowest available (Maybe another is equal)
            var compareWith = boundsWithIndex.Where(b => !toSkip.Contains(b.index)).ToArray();

            if (compareWith.Any(b => b.index != index && b.low == currentLow))
            {
                // low is equal, high is different
                var equalLowDifferentHigh = compareWith.Where(b => b.low == currentLow).ToArray();
                result.Add((currentLow, equalLowDifferentHigh.MaxBy(b => b.high).high));
                toSkip.UnionWith(equalLowDifferentHigh.Select(b => b.index));
                continue;
            }

            // currentLow is lowest

            if (compareWith
                .Any(b =>
                    b.index != index &&
                    b.low <= currentHigh))
            {
                var lowSmallerThanHigh =
                    compareWith
                        .Where(b => b.low <= currentHigh)
                        .ToArray();
                result.Add((currentLow, lowSmallerThanHigh.MaxBy(b => b.high).high));
                toSkip.UnionWith(lowSmallerThanHigh.Select(b => b.index));

                continue;
            }

            // currentHigh is lowest

            toSkip.Add(index);
            result.Add((currentLow, currentHigh));
        }

        hasChanges = result.Count != bounds.Count;
        return result;
    }
}
