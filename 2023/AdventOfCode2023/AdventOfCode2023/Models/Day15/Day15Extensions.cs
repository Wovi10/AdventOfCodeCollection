using System.Text;

namespace AdventOfCode2023_1.Models.Day15;

public static class Day15Extensions
{
    public static Task<int> Hash(this string step)
    {
        var stepAsBytes = Encoding.ASCII.GetBytes(step);

        var currentValue = 0;
        foreach (var character in stepAsBytes)
        {
            currentValue += character;
            currentValue *= 17;
            currentValue %= 256;
        }

        return Task.FromResult(currentValue);
    }

    public static Operation ToOperation(this char operation)
    {
        return operation switch
        {
            '=' => Operation.Add,
            '-' => Operation.Remove,
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
        };
    }
}