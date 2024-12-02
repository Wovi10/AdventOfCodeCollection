using System.Diagnostics;
using AOC.Utils.Enums;

namespace AOC.Utils;

public abstract class DayBase(string day, string title)
{
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
        => await RunPart(PartOne, true);

    private async Task RunPartTwo()
        => await RunPart(PartTwo, false);

    private async Task RunPart(Func<Task<object>> partToRun, bool runningPartOne)
    {
        Variables.RunningPartOne = runningPartOne;
        Input = SharedMethods.GetInput(Day);

        var result = await partToRun();
        SharedMethods.PrintAnswer(result);

#if !DEBUG
        var expectedAnswer = Answers.GetExpectedAnswer(Day);
        Assert.That(result, Is.EqualTo(expectedAnswer));
#endif
    }

    protected abstract Task<object> PartOne();

    protected abstract Task<object> PartTwo();
}