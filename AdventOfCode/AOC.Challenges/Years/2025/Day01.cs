using AOC.Utils;

namespace AOC.Challenges.Years._2025;

public class Day01() : DayBase("01", "Secret Entrance")
{
    private const char Right = 'R';
    private const int MaxDialValue = 99;
    private const int DialRange = 100;

    protected override Task<object> PartOne()
    {
        var result = FindPassword();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var  result = NewPasswordMethod();
        return Task.FromResult<object>(result);
    }

    private long FindPassword()
    {
        var input = GetInput();
        var timesPointedAtZero = 0L;
        var start = 50;
        foreach (var line in input)
        {
            start = line[0] == Right
                ? Add(start, int.Parse(line[1..]))
                : Add(start, -int.Parse(line[1..]));
            if (start == 0)
                timesPointedAtZero++;
        }

        return timesPointedAtZero;
    }

    private static int Add(int start, int valueToAdd)
    {
        var result = start + (valueToAdd % DialRange);
        return result switch
        {
            >= 0 and <= MaxDialValue => result,
            > MaxDialValue => result - DialRange,
            _ => DialRange + result
        };
    }

    private long NewPasswordMethod()
    {
        var input = GetInput();
        var timesPassedZero = 0L;
        var start = 50;

        foreach (var line in input)
        {
            var value = int.Parse(line[1..]);
            timesPassedZero += value / DialRange;
            var rest = value % DialRange;

            timesPassedZero +=
                line[0] == Right
                    ? Add(start, rest, out start)
                    : Add(start, -rest, out start);
        }

        return timesPassedZero;
    }

    private static int Add(int start, int valueToAdd, out int newStart)
    {
        var result = start + valueToAdd;

        switch (result)
        {
            case >= 0 and <= MaxDialValue:
                newStart = result;
                return newStart == 0 ? 1 : 0; // Ends on 0
            case > MaxDialValue:
                newStart = result - DialRange;
                break;
            default:
                newStart = DialRange + result;
                break;
        }

        return start == 0 ? 0 : 1;
    }
}
