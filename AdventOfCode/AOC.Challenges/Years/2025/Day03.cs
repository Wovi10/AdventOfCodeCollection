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

        return Task.FromResult<object>(result); // > 170775065764953
    }

    private long GetTotalJoltage()
    {
        var input = GetInput();

        return input.Sum(GetJoltage);
    }

    private const int JoltageLength = 12;
    private long GetJoltage(string line)
    {
        if (Variables.RunningPartOne)
        {
            var usableNumbers = line[..^1];
            var highestNumber = usableNumbers.Max();
            var indexHighest = line.IndexOf(highestNumber) + 1;
            var secondHighest = line[indexHighest..].Max();

            return long.Parse($"{highestNumber}{secondHighest}");
        }

        var maxIndexNumber = line.Length - JoltageLength;
        var highest = line[..maxIndexNumber].Max();
        var indexHigh = line.IndexOf(highest);
        var leftOver = line[indexHigh..];

        var length = leftOver.Length;
        while (length > JoltageLength)
        {
            var lowest = leftOver.Min();
            leftOver = leftOver.Remove(leftOver.IndexOf(lowest), 1);

            length = leftOver.Length;
        }

        return long.Parse(leftOver);
    }
}
