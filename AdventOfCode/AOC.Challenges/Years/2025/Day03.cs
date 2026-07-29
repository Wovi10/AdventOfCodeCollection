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

        return Task.FromResult<object>(result);
    }

    private long GetTotalJoltage()
    {
        var input = GetInput();

        return input.Sum(GetJoltage);
    }

    private int GetJoltage(string line)
    {
        if (Variables.RunningPartOne)
        {
            var usableNumbers = line[..^1].ToCharArray();
            var highestNumber = usableNumbers.Max();
            var indexHighest = line.IndexOf(highestNumber) + 1;
            var secondHighest = line[indexHighest..].ToCharArray().Max();

            return int.Parse($"{highestNumber}{secondHighest}");
        }

        return 0;
    }
}
