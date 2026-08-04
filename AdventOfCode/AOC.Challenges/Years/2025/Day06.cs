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
        var result = RevisedGetGrandTotalCephalopodMath();

        return Task.FromResult<object>(result);
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

    private long RevisedGetGrandTotalCephalopodMath()
    {
        var input = GetInput(false).ToArray();
        var indexSymbols = GetIndexSymbolLengthList(input.Last());

        var result = 0L;
        for (var i = 0; i < indexSymbols.Count; i++)
        {
            var indexSymbol = indexSymbols[i];
            int? nextSymbolIndex = indexSymbols.Count > i + 1 ? indexSymbols[i + 1].index-1 : null;
            var column = input.Select(l => l[indexSymbol.index..(nextSymbolIndex ?? ^0)]).ToArray();
            var length = column.MaxBy(c => c.Length)?.Length ?? 0;

            var symbol = column[^1][0];
            var numsToUse = column[..^1];
            var numbers = new string[length];
            for (var j = 0; j < length; j++)
            {
                numbers[j] = numsToUse
                    .Where(c => j <= c.Length-1 && !string.IsNullOrWhiteSpace(c[j].ToString()))
                    .Select(c => c[j].ToString())
                    .Aggregate(string.Empty, (current, next) => current + next)
                    .ToString();
            }

            result += symbol == '+'
                ? numbers.Select(long.Parse).Sum()
                : numbers.Select(long.Parse).Aggregate(1L, (a, b) => a * b);
        }

        return result;
    }

    private List<(int index, char symbol)> GetIndexSymbolLengthList(string input)
    {
        var result = new List<(int index, char symbol)>();

        for (var i = 0; i < input.Length; i++)
        {
            var currentSymbol = input[i];
            if (currentSymbol is '+' or '*')
                result.Add((i, currentSymbol));
        }

        return result;
    }
}
