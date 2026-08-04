using System.Diagnostics;
using System.Runtime.CompilerServices;
using AOC.Utils.Enums;

namespace AOC.Utils;

public abstract class DayBase(string day, string title, [CallerFilePath] string sourceFilePath = "")
{
    protected static List<string> Input = new();
    protected string Day { get; } = day;

    // Captured from the concrete DayXX class's own source file, so input files resolve
    // correctly no matter what process/working directory ends up running the day.
    private string ProjectRoot { get; } = Path.GetDirectoryName(sourceFilePath)!;

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

    protected IEnumerable<string> GetInput(bool trimStart = true)
        => SharedMethods.GetInput(Day, trimStart, ProjectRoot);

    // Plain result retrieval for callers that just want the answer (e.g. a GUI "Run"
    // button), without the console printing/DEBUG-assertion side effects of Run().
    public async Task<object> RunPartForResult(PartsToRun part, bool useMock = false)
    {
        if (part == PartsToRun.Both)
            throw new ArgumentOutOfRangeException(nameof(part), part, "Choose Part1 or Part2.");

        Variables.RunningPartOne = part == PartsToRun.Part1;

        // Some Day classes call SharedMethods.GetInput directly instead of going through
        // DayBase - this override makes those calls respect useMock too, not just the
        // Input field populated below.
        Variables.UseMockInput = useMock;
        try
        {
            Input = SharedMethods.GetInput(Day, false, ProjectRoot, useMock);
            return part == PartsToRun.Part1 ? await PartOne() : await PartTwo();
        }
        finally
        {
            Variables.UseMockInput = null;
        }
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
        Input = SharedMethods.GetInput(Day, false, ProjectRoot);

        var result = await partToRun();

        SharedMethods.PrintAnswer(result);
    }


    protected abstract Task<object> PartOne();

    protected abstract Task<object> PartTwo();
}