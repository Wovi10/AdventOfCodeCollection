using System.Diagnostics;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public abstract class DayBase
{
    protected static List<string> Input = new();

    public async Task Run(string day, string title, PartsToRun partToRun = Constants.PartToRun)
    {
        WriteStopwatchStartText();
        var watch = new Stopwatch();
        watch.Start();

        Input = SharedMethods.GetInput(day);
        SharedMethods.WriteBeginText(day, title);
        Variables.RunningPartOne = true;
        switch (partToRun)
        {
            case PartsToRun.Part1:
                await PartOne();
                break;
            case PartsToRun.Part2:
                Variables.RunningPartOne = false;
                await PartTwo();
                break;
            case PartsToRun.Both:
                await PartOne();
                Variables.RunningPartOne = false;
                await PartTwo();
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

    protected abstract Task PartOne();

    protected abstract Task PartTwo();
}