using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public abstract class DayBase
{
    private static string _day = "";
    private static bool _isMock = true;
    protected static List<string> Input = new();
    protected static bool RunningPartOne = true;
    
    public void Run(string day, string title, PartsToRun partsToRun = PartsToRun.Both, bool isMock = false)
    {
        _day = day;
        _isMock = isMock;
        Input = SharedMethods.GetInput(day, isMock);
        SharedMethods.WriteBeginText(day, title);
        RunningPartOne = true;
        switch (partsToRun)
        {
            case PartsToRun.Part1:
                PartOne();
                break;
            case PartsToRun.Part2:
                PartTwo();
                break;
            case PartsToRun.Both:
            default:
                PartOne();
                RunningPartOne = false;
                PartTwo();
                break;
        }
        Console.WriteLine();
    }

    protected abstract void PartOne();

    protected abstract void PartTwo();
}