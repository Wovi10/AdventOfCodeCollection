using System.Text.RegularExpressions;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day01 : DayBase
{
    protected override Task PartOne()
    {
        var result = GetCalibrationSum();
        SharedMethods.AnswerPart(result);
        return Task.CompletedTask;
    }

    protected override Task PartTwo()
    {
        var result = GetCalibrationSum(@"\d|one|two|three|four|five|six|seven|eight|nine");
        SharedMethods.AnswerPart(result);
        return Task.CompletedTask;
    }

    private static int GetCalibrationSum(string regexMatch = @"\d")
    {
        var calList = GetCalibrationList(regexMatch);
        return calList.Sum();
    }

    private static List<int> GetCalibrationList(string regex = @"\d")
    {
        return Input
            .Select(inputLine => new { inputLine, firstNumber = Regex.Match(inputLine, regex) })
            .Select(t => new { t, lastNumber = Regex.Match(t.inputLine, regex, RegexOptions.RightToLeft) })
            .Select(t => ParseMatch(t.t.firstNumber.Value) * 10 + ParseMatch(t.lastNumber.Value))
            .ToList();
    }

    private static int ParseMatch(string value)
    {
        try
        {
            return value switch
            {
                "one" => 1,
                "two" => 2,
                "three" => 3,
                "four" => 4,
                "five" => 5,
                "six" => 6,
                "seven" => 7,
                "eight" => 8,
                "nine" => 9,
                _ => int.Parse(value)
            };
        }
        catch (Exception)
        {
            return 0;
        }
    }
}