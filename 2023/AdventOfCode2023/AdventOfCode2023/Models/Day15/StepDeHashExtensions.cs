using System.Text;

namespace AdventOfCode2023_1.Models.Day15;

public static class StepDeHashExtensions
{
    public static Task<int> DeHash(this string step)
    {
        var stepAsBytes = Encoding.ASCII.GetBytes(step);
        
        var currentValue = 0;
        foreach (var character in stepAsBytes)
        {
            currentValue += character;
            currentValue *= 17;

            currentValue = currentValue % 256;
        }
        
        return Task.FromResult(currentValue);
    }
}