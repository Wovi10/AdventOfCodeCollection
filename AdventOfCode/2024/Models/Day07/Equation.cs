using System.Globalization;
using UtilsCSharp.Utils;

namespace _2024.Models.Day07;

public class Equation
{
    public long Result { get; set; }
    private int[] Numbers { get; set; }
    public Equation(string line)
    {
        var parts = line.Split(Constants.Colon);
        Result = long.Parse(parts[0]);
        var numbers = parts[1].Split(Constants.Space).Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s));
        Numbers = numbers.Select(int.Parse).ToArray();
    }

    public bool Evaluate()
        => Evaluate(Numbers, Numbers[0], 1, Result);

    private bool Evaluate(int[] numbers, int firstNumber, int index, long total)
    {
        if (index >= numbers.Length)
            return firstNumber == total;

        return Evaluate(numbers, firstNumber + numbers[index], index + 1, total)
               || Evaluate(numbers, firstNumber * numbers[index], index + 1, total);
    }
}