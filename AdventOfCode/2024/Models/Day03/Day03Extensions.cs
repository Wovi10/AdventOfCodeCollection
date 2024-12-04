using AOC.Utils;
using UtilsCSharp;
using Constants = UtilsCSharp.Utils.Constants;

namespace _2024.Models.Day03;

public static class Day03Extensions
{
    public static string ToSingleString(this IEnumerable<string> input)
        => string.Join("", input);

    public static IEnumerable<string> FindAllMultiplicationStrings(this string input)
    {
        const string doString = "do()";
        const string dontString = "don't()";
        const string mulString = "mul(";

        var instructionsEnabled = true;

        for (var i = 0; i < input.Length; i++)
        {
            if (!Variables.RunningPartOne)
            {
                switch (instructionsEnabled)
                {
                    case false when input.Skip(i).Take(doString.Length).SequenceEqual(doString):
                        instructionsEnabled = true;
                        i += doString.Length-1;
                        continue;
                    case true when input.Skip(i).Take(dontString.Length).SequenceEqual(dontString):
                        instructionsEnabled = false;
                        i += dontString.Length-1;
                        continue;
                    case false:
                        continue;
                }
            }

            var nextFourChars = string.Join(string.Empty, input.Skip(i).Take(mulString.Length));
            if (nextFourChars != mulString)
                continue;

            i += mulString.Length; // not -1 because of )

            var nextClosingBracketIndex = input.IndexOf(')', i);
            if (nextClosingBracketIndex == -1)
                break;

            var possibleNumbers = input[i..nextClosingBracketIndex].Split(Constants.Comma);

            if (possibleNumbers.Length == 2 && possibleNumbers.First().IsCorrectNumber() &&
                possibleNumbers.Last().IsCorrectNumber())
                yield return input[i..nextClosingBracketIndex];
        }
    }

    public static IEnumerable<(int, int)> AsNumberPairs(this IEnumerable<string> input)
        => input
            .Select(multiplication => multiplication.Split(Constants.Comma))
            .Select(numbers => (int.Parse(numbers.First()), int.Parse(numbers.Last())));

    public static IEnumerable<long> MultiplyAll(this IEnumerable<(int, int)> input)
        => input.Select(multiplication => (long)multiplication.Item1 * multiplication.Item2);

    private static bool IsCorrectNumber(this string input)
        => input.Length.IsBetween(1, 3) && int.TryParse(input, out _);
}