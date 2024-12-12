using System.Globalization;
using AOC.Utils;
using Constants = UtilsCSharp.Utils.Constants;

namespace _2024.Models.Day07;

public class Equation
{
    public long Result { get; }
    private int[] Numbers { get; }
    public Equation(string line)
    {
        var parts = line.Split(Constants.Colon);
        Result = long.Parse(parts[0]);
        var numbers = parts[1].Split(Constants.Space).Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s));
        Numbers = numbers.Select(int.Parse).ToArray();
    }

    public bool Evaluate()
        => Evaluate(Numbers.First(), 1);

    private bool Evaluate(long currentResult, int index)
        => index > Numbers.Length - 1
            ? currentResult == Result
            : Evaluate(currentResult + Numbers[index], index + 1) ||
              Evaluate(currentResult * Numbers[index], index + 1) ||
              (!Variables.RunningPartOne && Evaluate(Concat(currentResult, Numbers[index]), index +1));

    private static long Concat(long a, long b)
        => long.Parse($"{a}{b}");
}