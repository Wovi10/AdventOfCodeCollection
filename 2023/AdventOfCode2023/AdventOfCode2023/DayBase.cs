using System.Diagnostics;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public abstract class DayBase
{
    protected static List<string> Input = [];

    public void Run(string day, string title, PartsToRun partToRun = Constants.PartToRun)
    {
        var watch = new Stopwatch();
        watch.Start();
        
        Input = SharedMethods.GetInput(day);
        SharedMethods.WriteBeginText(day, title);
        Variables.RunningPartOne = true;
        switch (partToRun)
        {
            case PartsToRun.Part1:
                PartOne();
                break;
            case PartsToRun.Part2:
                Variables.RunningPartOne = false;
                PartTwo();
                break;
            case PartsToRun.Both:
                PartOne();
                Variables.RunningPartOne = false;
                PartTwo();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        watch.Stop();
        WriteStopwatchText(watch.ElapsedMilliseconds);

        Console.WriteLine();
    }

    private static void WriteStopwatchText(long watchElapsedMilliseconds)
    {
        if (Constants.IsDebug) 
            Console.WriteLine($"Elapsed time: {watchElapsedMilliseconds} ms");
    }

    protected abstract void PartOne();

    protected abstract void PartTwo();
}