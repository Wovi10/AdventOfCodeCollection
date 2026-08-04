using AOC.Utils;

namespace AOC.Challenges.Years._2025;

public class Day06() : DayBase("06", "Trash Compactor")
{
    protected override Task<object> PartOne()
    {
        var result = GetGrandTotalCephalopodMath();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private long GetGrandTotalCephalopodMath()
    {
        var input =
            GetInput()
                .Select(l => l.Split(" ").Where(item => !string.IsNullOrWhiteSpace(item) && item != " ").ToArray())
            .ToArray();
        var length = input.MaxBy(s => s.Length)?.Length ?? 0;

        var result = 0L;
        for (var i = 0; i < length; i++)
        {
            var allToUse = input.Select(l => l.GetValue(i)?.ToString() ?? "")
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();

            var symbol = allToUse.Last();
            var allNumbers = allToUse[..^1].Select(long.Parse).ToArray();

            result += symbol == "+"
                ? allNumbers.Sum()
                : allNumbers.Aggregate(1L, (a, b) => a * b);
        }

        return result;
    }
}
