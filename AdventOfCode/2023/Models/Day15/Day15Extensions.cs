using System.Text;

namespace _2023.Models.Day15;

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

    public static bool ToOperation(this char operation)
    {
        return operation switch
        {
            '=' => true,
            '-' => false,
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
        };
    }
}