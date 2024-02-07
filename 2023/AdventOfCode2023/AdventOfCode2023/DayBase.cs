using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public abstract class DayBase
{
    protected static List<string> Input = new();
    
    public void Run(string day, string title, PartsToRun partsToRun = PartsToRun.Both)
    {
        Input = SharedMethods.GetInput(day);
        SharedMethods.WriteBeginText(day, title);
        Variables.RunningPartOne = true;
        switch (partsToRun)
        {
            case PartsToRun.Part1:
                PartOne();
                break;
            case PartsToRun.Part2:
                Variables.RunningPartOne = false;
                PartTwo();
                break;
            case PartsToRun.Both:
            default:
                PartOne();
                Variables.RunningPartOne = false;
                PartTwo();
                break;
        }
        Console.WriteLine();
    }

    protected abstract void PartOne();

    protected abstract void PartTwo();
}