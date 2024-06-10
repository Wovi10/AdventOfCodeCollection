using System.Diagnostics;
using AdventOfCode2023_1.Shared;
using NUnit.Framework;

namespace AdventOfCode2023_1;

public abstract class DayBase
{
    protected static List<string> Input = new();
    private object _expectedAnswer = 0;
    protected string Day = "01";

    public async Task Run(string day, string title)
    {
        Day = day;
        WriteStopwatchStartText();
        var watch = new Stopwatch();
        watch.Start();

        SharedMethods.WriteBeginText(day, title);
        switch (Constants.PartToRun)
        {
            case PartsToRun.Part1:
                await RunPartOne();
                break;
            case PartsToRun.Part2:
                await RunPartTwo();
                break;
            case PartsToRun.Both:
                await RunPartOne();
                await RunPartTwo();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        watch.Stop();
        WriteStopwatchText(watch.ElapsedMilliseconds);

        Console.WriteLine();
    }

    private static void WriteStopwatchStartText()
    {
        if (Constants.IsDebug)
            Console.WriteLine($"Started at {DateTime.Now:HH:mm:ss}");
    }

    private static void WriteStopwatchText(long watchElapsedMilliseconds)
    {
        if (Constants.IsDebug)
            Console.WriteLine($"Elapsed time: {watchElapsedMilliseconds} ms");
    }

    private async Task RunPartOne()
    {
        Variables.RunningPartOne = true;
        Input = SharedMethods.GetInput(Day);
        _expectedAnswer = Answers.GetExpectedAnswer(Day, true);

        var result = await PartOne();
        SharedMethods.PrintAnswer(result);

        Assert.That(result, Is.EqualTo(_expectedAnswer));
    }

    private async Task RunPartTwo()
    {
        Variables.RunningPartOne = false;
        Input = SharedMethods.GetInput(Day);
        _expectedAnswer = Answers.GetExpectedAnswer(Day, false);

        var result = await PartTwo();
        SharedMethods.PrintAnswer(result);

        Assert.That(result, Is.EqualTo(_expectedAnswer));
    }

    protected abstract Task<object> PartOne();

    protected abstract Task<object> PartTwo();
}