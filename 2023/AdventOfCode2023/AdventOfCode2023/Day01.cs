using System.Text.RegularExpressions;

namespace AdventOfCode2023_1;

public static class Day01
{
    // private static string sCurrentDirectory = Directory.GetCurrentDirectory();  
    private static readonly string FilePath = Path.Combine("../../..", "Input/Day01.in");
    private static readonly string FullPath = Path.Combine(Directory.GetCurrentDirectory(), FilePath);  
    // private static string sFilePath = Path.GetFullPath(sFile);
    private static readonly string InputFile = File.ReadAllText(FullPath);

    public static void Run()
    {
        Console.WriteLine("Starting day 1 challenge: Trebuchet?!");
        PartOne();
        PartTwo();
        Console.WriteLine();
    }

    private static void PartOne()
    {
        var result = GetCalibrationSum();
        Console.WriteLine($"Answer of this part 1 is: \n{result}");
    }

    private static void PartTwo()
    {
        var result = GetCalibrationSum(@"\d|one|two|three|four|five|six|seven|eight|nine");
        Console.WriteLine($"Answer of this part 2 is: \n{result}");
    }

    private static int GetCalibrationSum(string regexMatch = @"\d")
    {
        var calList = GetCalibrationList(regexMatch); 
        return calList.Sum();
    }

    private static List<int> GetCalibrationList(string regex = @"\d")
    {
        var tempLine = InputFile.Split("\n");
        var tempFirst = tempLine.Select(inputLine => new {inputLine, firstNumber = Regex.Match(inputLine, regex)}).First().firstNumber;
        var tempLast = tempLine.Select(inputLine => new {inputLine, lastNumber = Regex.Match(inputLine, regex, RegexOptions.RightToLeft)}).First().lastNumber;
        var tempOutput = ParseMatch(tempFirst.Value) * 10 + ParseMatch(tempLast.Value);
        return InputFile.Split("\n")
            .Select(inputLine => new {inputLine, firstNumber = Regex.Match(inputLine, regex)})
            .Select(t => new {t, lastNumber = Regex.Match(t.inputLine, regex, RegexOptions.RightToLeft)})
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