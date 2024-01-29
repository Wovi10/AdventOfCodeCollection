using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public abstract class DayBase
{
    protected static bool IsMock = true;
    public void Run(int day, string title, PartsToRun partsToRun = PartsToRun.Both, bool isMock = false)
    {
        IsMock = isMock;
        SharedMethods.WriteBeginText(day, title);
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
                PartTwo();
                break;
        }
        Console.WriteLine();
    }

    protected abstract void PartOne();

    protected abstract void PartTwo();
}