using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public abstract class DayBase
{
    public void Run(int day, string title, PartsToRun partsToRun = PartsToRun.Both)
    {
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

    public abstract void PartOne();

    public abstract void PartTwo();
}