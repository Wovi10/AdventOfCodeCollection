using System.Diagnostics;
using AdventOfCode2023_1.Shared;
using AdventOfCode2023_1.Shared.Enums;

namespace AdventOfCode2023_1;

public abstract class DayBase(string day, string title)
{
    public const bool IsReal = true;
    protected static List<string> Input = new();
    protected string Day { get; } = day;

    public async Task Run(PartsToRun partToRun = PartsToRun.Both)
    {
        WriteStopwatchStartText();
        var watch = new Stopwatch();
        watch.Start();

        SharedMethods.WriteBeginText(Day, title);
        switch (partToRun)
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
#if DEBUG
            Console.WriteLine($"Started at {DateTime.Now:HH:mm:ss}");
#endif
    }

    private static void WriteStopwatchText(long watchElapsedMilliseconds)
    {
#if DEBUG
        Console.WriteLine($"Elapsed time: {watchElapsedMilliseconds} ms");
#endif
    }

    private async Task RunPartOne() 
        => await RunPart(true, PartOne);

    private async Task RunPartTwo()
        => await RunPart(false, PartTwo);

    private async Task RunPart(bool runningPartOne, Func<Task<object>> partToRun)
    {
        Variables.RunningPartOne = runningPartOne;
        Input = SharedMethods.GetInput(Day);
        GetExpectedAnswer(runningPartOne);

        var result = await partToRun();
        SharedMethods.PrintAnswer(result);

#if !DEBUG
        Assert.That(result, Is.EqualTo(_expectedAnswer));
#endif
    }

    protected abstract Task<object> PartOne();

    protected abstract Task<object> PartTwo();

    private object GetExpectedAnswer(bool partOne)
        => Answers.GetExpectedAnswer(Day, partOne);
}