using AOC.Utils;

namespace AOC.Challenges.Years._2025;

public class Day03() : DayBase("03", "Lobby")
{
    protected override Task<object> PartOne()
    {
        var result = GetTotalJoltage();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetTotalJoltage();

        return Task.FromResult<object>(result);
        // > 170880833511431
    }

    private long GetTotalJoltage()
        => GetInput().Select(GetJoltage).ToArray().Sum();

    private const int JoltageLength = 12;
    private static long GetJoltage(string line)
    {
        if (Variables.RunningPartOne)
        {
            var usableNumbers = line[..^1];
            var highestNumber = usableNumbers.Max();
            var indexHighest = line.IndexOf(highestNumber) + 1;
            var secondHighest = line[indexHighest..].Max();

            return long.Parse($"{highestNumber}{secondHighest}");
        }

        var highest = line[..^(JoltageLength-1)].Max();
        var indexHigh = line.IndexOf(highest);

        var result = new char[JoltageLength];
        result[0] = highest;
        var windowStart = indexHigh + 1;

        var indexInResult = 1;
        for (var i = 1; i < JoltageLength; i++)
        {
            var remainingAfter = JoltageLength - i - 1;
            var windowEnd = line.Length - remainingAfter;

            var bestIndex = windowStart;
            for (var idx = windowStart + 1; idx < windowEnd; idx++)
                if (line[idx] > line[bestIndex])
                    bestIndex = idx;

            result[indexInResult++] = line[bestIndex];
            windowStart = bestIndex + 1;
        }

        return long.Parse(new string(result));
    }
}
