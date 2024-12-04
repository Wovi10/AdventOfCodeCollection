using UtilsCSharp.Utils;

namespace _2024.Models.Day03;

public static class Day03Extensions
{
    public static string ToSingleString(this IEnumerable<string> input)
        => string.Join("", input);

    public static IEnumerable<string> FindAllMultiplicationStrings(this string input)
    {
        for (var i = 0; i < input.Length; i++)
        {
            var nextFourChars = string.Join(string.Empty, input.Skip(i).Take(4));
            if (nextFourChars != "mul(")
                continue;

            i += 4;

            var nextClosingBracketIndex = input.IndexOf(')', i);
            if (nextClosingBracketIndex == -1)
                break;

            if (nextClosingBracketIndex - i > 7)
                continue;

            var possibleNumbers = input[i..nextClosingBracketIndex].Split(Constants.Comma);

            if (possibleNumbers.Length == 2 && possibleNumbers.First().IsCorrectNumber() &&
                possibleNumbers.Last().IsCorrectNumber())
                yield return input[i..nextClosingBracketIndex];
        }
    }

    public static IEnumerable<(int, int)> ToNumberPairs(this IEnumerable<string> input)
        => input
            .Select(multiplication => multiplication.Split(Constants.Comma))
            .Select(numbers => (int.Parse(numbers.First()), int.Parse(numbers.Last())));

    public static IEnumerable<long> MultiplyAll(this IEnumerable<(int, int)> input)
        => input.Select(multiplication => (long)multiplication.Item1 * multiplication.Item2);

    private static bool IsCorrectNumber(this string input)
        => input.Length is >= 1 and <= 3 && int.TryParse(input, out _);
}